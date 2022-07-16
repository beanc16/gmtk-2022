using System;
using _.Scripts.Core;
using _.Scripts.Enemy;
using _.Scripts.Player;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace _.Scripts.World
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private bool disableSpawner;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Transform enemySpawnPoint;
        [SerializeField] private WaveScriptableObject waveScriptableObject;

        private IObjectPool<EnemyAi> enemyPool;
        private int enemiesLeftInWave;
        private float timeTillNextSpawn;

        private int enemiesLeftAlive;
        public int EnemiesLeftAlive => enemiesLeftAlive;

        private float timeTillNextWave;

        public float TimeTillNextWave => timeTillNextWave;

        private void Awake()
        {
            enemyPool = new ObjectPool<EnemyAi>(()
                    => Instantiate(waveScriptableObject.EnemyAi, enemyParent),
                OnGet,
                OnRelease,
                Destroy,
                false,
                10,
                20);
            Reset();
        }

        public void SpawnPlayer(GameObject playerPrefab)
        {
            Instantiate(playerPrefab, playerSpawnPoint);
        }

        private void OnGet(EnemyAi enemyAi)
        {
            enemyAi.gameObject.SetActive(true);
            enemyAi.transform.position = GetSpawnPosition(enemyAi);
            enemyAi.Init(
                DespawnEnemy,
                waveScriptableObject.EnemyHp + GameController.Instance.Wave * waveScriptableObject.EnemyHpIncrease,
                //Random.Range(waveScriptableObject.EnemySpeedMin, waveScriptableObject.EnemySpeedMax));
                waveScriptableObject.EnemySpeedMin, 
                waveScriptableObject.EnemySpeedMax,
                waveScriptableObject.EnemySpeedChangeMax);
        }

        private void OnRelease(EnemyAi enemyAi)
        {
            enemyAi.gameObject.SetActive(false);
        }

        private void Update()
        {
            /*if (GameController.Instance.IsGameActive == false)
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
            }*/
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

            for (int i = 0; i < amountSpawnedInWave; i++)
            {
                enemyPool.Get();
            }
        }

        private void DespawnEnemy(EnemyAi enemyAi)
        {
            enemyPool.Release(enemyAi);

            enemiesLeftAlive--;
            GameController.Instance.CurrentScore++;

            if (enemiesLeftAlive <= 0 && enemiesLeftInWave <= 0)
            {
                GameController.Instance.RollForRandomEffect();
            }
        }

        private Vector2 GetSpawnPosition(EnemyAi enemyAi)
        {
            var pos = enemyAi.transform.position;
            var playerPosition = PlayerController.Instance.transform.position;

            float xPos = Random.Range(playerPosition.x - waveScriptableObject.PositionRandomizer.x,
                playerPosition.x + waveScriptableObject.PositionRandomizer.x);
            if (xPos < playerPosition.x)
            {
                xPos -= waveScriptableObject.PositionRandomizerMin.x;
            }
            else
            {
                xPos += waveScriptableObject.PositionRandomizerMin.x;
            }
            pos.x += xPos;
            
            float yPos = Random.Range(playerPosition.y - waveScriptableObject.PositionRandomizer.y,
                playerPosition.y + waveScriptableObject.PositionRandomizer.y);
            if (yPos < playerPosition.x)
            {
                yPos -= waveScriptableObject.PositionRandomizerMin.y;
            }
            else
            {
                yPos += waveScriptableObject.PositionRandomizerMin.y;
            }
            pos.y += yPos;
            return pos;
        }
    }
}