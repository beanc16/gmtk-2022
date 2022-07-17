using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _.Scripts.AttackSystem
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] private List<AttackSoBase> attacks;

        private readonly Dictionary<int, bool> _cooldownDictionary = new();
        private bool _started;
        private async UniTaskVoid Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), false,cancellationToken: this.GetCancellationTokenOnDestroy());
            _started = true;
            for (var i = 0; i < attacks.Count; i++)
            {
                var attack = attacks[i];
                attack.EnableAttack();
                _cooldownDictionary.Add(i, false);
            }
        }

        private void OnDestroy()
        {
            foreach (var attack in attacks) attack.DisableAttack();
        }

        public void Attack(int attackNr)
        {
            if (!_started) return;
            if (_cooldownDictionary[attackNr]) return;
            if (attackNr > attacks.Count + 1) return; // value out of bounds
            attacks[attackNr].Shoot(transform);

            if (attacks[attackNr].GetCooldown() > 0f)
            {
                CooldownAsync(attackNr);
            }
        }

        private async void CooldownAsync(int attackNr)
        {
            _cooldownDictionary[attackNr] = true;
            await UniTask.Delay(TimeSpan.FromSeconds(attacks[attackNr].GetCooldown()), ignoreTimeScale: false);
            _cooldownDictionary[attackNr] = false;
        }
    }
}
