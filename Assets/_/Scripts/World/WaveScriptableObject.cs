using _.Scripts.Enemy;
using UnityEngine;

namespace _.Scripts.World
{
    [CreateAssetMenu(fileName = "EnemyWave", menuName = "EnemyWave", order = 0)]
    public class WaveScriptableObject : ScriptableObject
    {
        public EnemyAi EnemyAi;
        public float EnemySpeedMin;
        public float EnemySpeedMax;
        public int EnemiesInWave;
        public int AmountSpawnedPerCycle;
        public float TimeBetweenCycles;
        public Vector2 PositionRandomizer;
    }
}