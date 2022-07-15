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
            foreach (var attack in attacks) attack.InitAttack();
        }

        public void Attack(int attackNr)
        {
            attacks[attackNr].Shoot(transform);
        }
    }
}
