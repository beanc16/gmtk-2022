using System;
using System.Collections.Generic;
using _.Scripts.Enemy;
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
        
        public float TimeTillNextSpawn;

        private void Awake()
        {
            TimeTillNextSpawn = waveScriptableObject.TimeBetweenCycles;
            enemiesLeftInWave = waveScriptableObject.EnemiesInWave;
        }

        private void Update()
        {
            if (disableSpawner)
            {
                return;
            }
            
            TimeTillNextSpawn -= Time.deltaTime;

            if (TimeTillNextSpawn <= 0)
            {
                SpawnNextWave();
            }
        }

        private void SpawnNextWave()
        {
            TimeTillNextSpawn = waveScriptableObject.TimeBetweenCycles;

            if (enemiesLeftInWave == 0)
            {
                return;
            }

            int amountSpawnedInWave = Math.Min(waveScriptableObject.AmountSpawnedPerCycle, enemiesLeftInWave);
            enemiesLeftInWave -= amountSpawnedInWave;

            for (int i = 0; i < amountSpawnedInWave; i++)
            {
                EnemyAi newEnemy;
                if (enemyPool.Count > 0)
                {
                    newEnemy = enemyPool.Dequeue();
                }
                else
                {
                    newEnemy = Instantiate(waveScriptableObject.EnemyAi, enemyParent);
                }

                var pos = newEnemy.transform.position;
                pos.x += Random.Range(-waveScriptableObject.PositionRandomizer.x,
                    waveScriptableObject.PositionRandomizer.x);
                pos.y += Random.Range(-waveScriptableObject.PositionRandomizer.y,
                    waveScriptableObject.PositionRandomizer.y);
                newEnemy.transform.position = pos;
                
                newEnemy.Setup(DespawnEnemy, 
                    Random.Range(waveScriptableObject.EnemySpeedMin, waveScriptableObject.EnemySpeedMax),
                    waveScriptableObject.EnemyHp);
            }
        }
        
        private void DespawnEnemy(EnemyAi enemyAi)
        {
            enemyPool.Enqueue(enemyAi);
            //Might need to set position as well;
            enemyAi.gameObject.SetActive(false);
        }
    }
}