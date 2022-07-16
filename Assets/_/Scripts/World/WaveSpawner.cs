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

        private List<EnemyAi> enemyPool = new List<EnemyAi>();
        private int enemiesLeftInWave;
        private float timeTillNextSpawn;

        private void Awake()
        {
            timeTillNextSpawn = waveScriptableObject.TimeBetweenCycles;
            enemiesLeftInWave = waveScriptableObject.EnemiesInWave;
        }

        private void Update()
        {
            if (disableSpawner)
            {
                return;
            }
            
            timeTillNextSpawn -= Time.deltaTime;

            if (timeTillNextSpawn <= 0)
            {
                SpawnNextWave();
            }
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

            for (int i = 0; i < amountSpawnedInWave; i++)
            {
                var newEnemy = Instantiate(waveScriptableObject.EnemyAi, enemyParent);
                var pos = newEnemy.transform.position;
                pos.x += Random.Range(-waveScriptableObject.PositionRandomizer.x,
                    waveScriptableObject.PositionRandomizer.x);
                pos.y += Random.Range(-waveScriptableObject.PositionRandomizer.y,
                    waveScriptableObject.PositionRandomizer.y);
                newEnemy.transform.position = pos;
                
                newEnemy.Setup(DespawnEnemy, 
                    Random.Range(waveScriptableObject.EnemySpeedMin, waveScriptableObject.EnemySpeedMax));
            }
        }
        
        private void DespawnEnemy(EnemyAi enemyAi)
        {
            enemyPool.Add(enemyAi);
            //Might need to set position as well;
            enemyAi.gameObject.SetActive(false);
        }
    }
}