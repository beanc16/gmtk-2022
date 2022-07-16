using System;
using System.Collections.Generic;
using UnityEngine;

namespace _.Scripts.AttackSystem
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] private List<AttackSo> attacks;

        private void Start()
        {
            foreach (var attack in attacks) attack.EnableAttack();
        }

        private void OnDestroy()
        {
            foreach (var attack in attacks) attack.DisableAttack();
        }

        public void Attack(int attackNr)
        {
            if (attackNr > attacks.Count) return; // value out of bounds
            
            attacks[attackNr].Shoot(transform);
        }
    }
}
