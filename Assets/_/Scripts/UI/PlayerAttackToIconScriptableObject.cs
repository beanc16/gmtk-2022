using System;
using System.Collections.Generic;
using _.Scripts.Player;
using UnityEngine;

namespace _.Scripts.UI
{
    [CreateAssetMenu(fileName = "PlayerAttackIcon", menuName = "GMTK2022/Attack/PlayerAttackIcon", order = 0)]
    public class PlayerAttackToIconScriptableObject : ScriptableObject
    {
        public List<AttackToIcon> AttackToIcons;

        public Sprite GetIconForSprite(PlayerAttackType attackType)
        {
            foreach (var data in AttackToIcons)
            {
                if (data.AttackType == attackType)
                {
                    return data.AttackIcon;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class AttackToIcon
    {
        public PlayerAttackType AttackType;
        public Sprite AttackIcon;
    }
}