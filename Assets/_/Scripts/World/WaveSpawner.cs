using System;
using System.Collections.Generic;
using _.Scripts.Core;
using _.Scripts.Enemy;
using _.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _.Scripts.World
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private bool disableSpawner;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private WaveScriptableObject waveScriptableObject;

        private Queue<EnemyAi> enemyPool = new Queue<EnemyAi>();
        private int enemiesLeftInWave;
        private float timeTillNextSpawn;
        
        private int enemiesLeftAlive;
        public int EnemiesLeftAlive => enemiesLeftAlive;

        private float timeTillNextWave;
        public float TimeTillNextWave => timeTillNextWave;

        private void Awake()
        {
            Reset();
        }

        private void Update()
        {
            if (GameController.Instance.IsGameActive == false)
            {
                return;
            }
            
            if (disableSpawner)
            {
                return;
            }

            if (enemiesLeftInWave <= 0)
            {
                if (enemiesLeftAlive > 0)
                {
                    return;
                }
                
                timeTillNextWave -= Time.deltaTime;
                if (timeTillNextWave <= 0)
                {
                    Reset();
                }
                
                return;
            }
            
            timeTillNextSpawn -= Time.deltaTime;

            if (timeTillNextSpawn <= 0)
            {
                SpawnNextWave();
            }
        }
        
        private void Reset()
        {
            int wave = 0;
            if (GameController.Instance != null)
            {
                wave = GameController.Instance.Wave;
            }
            timeTillNextSpawn = waveScriptableObject.TimeBetweenCycles;
            timeTillNextWave = waveScriptableObject.TimeBetweenWaves;
            enemiesLeftInWave = waveScriptableObject.EnemiesInWave +
                                wave * waveScriptableObject.EnemiesInWaveIncrease;
            enemiesLeftAlive = waveScriptableObject.EnemiesInWave +
                               wave * waveScriptableObject.EnemiesInWaveIncrease;
        }

        private void SpawnNextWave()
        {
            timeTillNextSpawn = waveScriptableObject.TimeBetweenCycles;

            if (enemiesLeftInWave == 0)
            {
                return;
            }

            int amountSpawnedInWave = Math.Min(waveScriptableObject.AmountSpawnedPerCycle, enemiesLeftInWave);
            enemiesLeftInWave -= amountSpawnedInWave;
            int wave = GameController.Instance.Wave;

            for (int i = 0; i < amountSpawnedInWave; i++)
            {
                EnemyAi newEnemy;
                if (enemyPool.Count > 0)
                {
                    newEnemy = enemyPool.Dequeue();
                    newEnemy.gameObject.SetActive(true);
                }
                else
                {
                    newEnemy = Instantiate(waveScriptableObject.EnemyAi, enemyParent);
                }

                var pos = newEnemy.transform.position;
                var playerPosition = PlayerController.Instance.transform.position;
                pos.x += Random.Range(playerPosition.x - waveScriptableObject.PositionRandomizer.x,
                    playerPosition.x + waveScriptableObject.PositionRandomizer.x);
                pos.y += Random.Range(playerPosition.y - waveScriptableObject.PositionRandomizer.y,
                    playerPosition.y + waveScriptableObject.PositionRandomizer.y);
                newEnemy.transform.position = pos;
                
                newEnemy.Setup(DespawnEnemy, 
                    Random.Range(waveScriptableObject.EnemySpeedMin, waveScriptableObject.EnemySpeedMax),
                    waveScriptableObject.EnemyHp + wave * waveScriptableObject.EnemyHpIncrease);
            }
        }
        
        private void DespawnEnemy(EnemyAi enemyAi)
        {
            enemyPool.Enqueue(enemyAi);
            //Might need to set position as well;
            enemyAi.gameObject.SetActive(false);

            enemiesLeftAlive--;
            GameController.Instance.CurrentScore++;

            if (enemiesLeftAlive <= 0 && enemiesLeftInWave <= 0)
            {
                GameController.Instance.RollForRandomEffect();
            }
        }
    }
}