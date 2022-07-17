using UnityEngine;

namespace _.Scripts.RollingScene
{
    [CreateAssetMenu(fileName = "DiceRoll", menuName = "GMTK2022/DiceRoll/DiceRoll", order = 0)]
    public class DiceRollScriptableObject : ScriptableObject
    {
        public float TimeToRoll;
        public float TimeForFastChange;
        public float TimeToSlowdown;
        public float FastChangeTime = 0.1f;
        public float MediumChangeTime = 0.4f;
        public float SlowChangeTime = 0.8f;
    }
}