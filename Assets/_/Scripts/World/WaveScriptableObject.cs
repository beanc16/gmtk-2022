using _.Scripts.Enemy;
using UnityEngine;

namespace _.Scripts.World
{
    [CreateAssetMenu(fileName = "EnemyWave", menuName = "GMTK2022/EnemyWaves/EnemyWave", order = 0)]
    public class WaveScriptableObject : ScriptableObject
    {
        public EnemyAi EnemyAi;
        public float EnemySpeedMin;
        public float EnemySpeedMax;
        public int EnemiesInWave;
        public int EnemiesInWaveIncrease;
        public int AmountSpawnedPerCycle;
        public float TimeBetweenCycles;
        public float TimeBetweenWaves;
        public float EnemyHp;
        public float EnemyHpIncrease;
        public Vector2 PositionRandomizer;
    }
}