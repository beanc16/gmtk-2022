using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Static", menuName = "GMTK2022/Attack/Static", order = 1)]
    public class AttackStatic : AttackSoBase
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float range;
        
        private bool _onCooldown;
        
        public override void Shoot(Transform fromTransform)
        {
            if(_onCooldown) return; 
            Debug.Log("Shoot Static");
            var projectile = Pool.Get();
            projectile.transform.position = fromTransform.position;
            var attackObject = new AttackObject(ReleaseTarget, projectile, this);
            if (attackCooldown > 0f) Cooldown();
        }

        private async void Cooldown()
        {
            _onCooldown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(attackCooldown), ignoreTimeScale: false, cancellationToken: CancellationToken);
            _onCooldown = false;
        }
        
        public override async void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished)
        {
            var time = 0f;
            while (time < lifeTime || !attackObject.activeSelf)
            {
                time += Time.deltaTime;
                await UniTask.Yield();
            }
            
            onAttackFinished();
        }

        private void ReleaseTarget(GameObject projectile) => Pool.Release(projectile);
    }
}