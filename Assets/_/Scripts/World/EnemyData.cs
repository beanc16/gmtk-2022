using _.Scripts.Enemy;
using UnityEngine;

namespace _.Scripts.World
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "GMTK2022/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public float EnemySpeedMin;
        public float EnemySpeedMax;
        public float EnemyHp;
    }
}