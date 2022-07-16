using UnityEngine;

namespace _.Scripts.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "GMTK2022/Player/Player", order = 0)]
    public class PlayerScriptableObject : ScriptableObject
    {
        public float PlayerSpeed;
        public float PlayerHp;
        public float InvulnerableTime;
    }
}