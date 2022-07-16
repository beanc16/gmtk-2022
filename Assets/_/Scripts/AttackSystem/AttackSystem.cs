using System;
using System.Collections.Generic;
using UnityEngine;

namespace _.Scripts.AttackSystem
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] private List<AttackSoBase> attacks;

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
            Debug.Log(attacks[attackNr].GetAttackName());
            if (attackNr > attacks.Count + 1) return; // value out of bounds
            attacks[attackNr].Shoot(transform);
        }
    }
}
