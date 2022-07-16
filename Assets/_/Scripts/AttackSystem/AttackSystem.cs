using System;
using System.Collections.Generic;
using UnityEngine;

namespace _.Scripts.AttackSystem
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] private List<AttackSo> attacks;

        private float timeTillNextAttack;

        private void Start()
        {
            foreach (var attack in attacks) attack.InitAttack();
        }

        private void Update()
        {
            timeTillNextAttack -= Time.deltaTime;
        }

        public void Attack(int attackNr)
        {
            if (timeTillNextAttack > 0)
            {
                return;
            }
            
            if (attackNr > attacks.Count)
            {
                //Failsafe for out of bounds exception
                attackNr = 0;
            }
            attacks[attackNr].Shoot(transform);
            timeTillNextAttack = attacks[attackNr].TimeBetweenAttacks;
        }
    }
}
