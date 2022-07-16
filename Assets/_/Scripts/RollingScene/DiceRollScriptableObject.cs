using UnityEngine;

namespace _.Scripts.RollingScene
{
    [CreateAssetMenu(fileName = "DiceRoll", menuName = "GMTK2022/DiceRoll/DiceRoll", order = 0)]
    public class DiceRollScriptableObject : ScriptableObject
    {
        public float TimeToRoll;
        public float TimeForFastChange;
        public float TimeToSlowdown;
    }
}