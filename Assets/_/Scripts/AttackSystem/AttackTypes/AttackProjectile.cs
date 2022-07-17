using System;
using System.Threading;
using _.Scripts.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "GMTK2022/Attack/Projectile", order = 0)]
    public class AttackProjectile : AttackSoBase
    {
        [SerializeField] private bool doDamageOnce;

        public override void Shoot(Transform fromTransform)
        {
            if (!IsInLineOfSight(fromTransform.position)) return;
            var projectile = Pool.Get();
            projectile.transform.position = attacker == Attacker.PlayerAttack ? fromTransform.position : fromTransform.position + Vector3.up ;
            var attackObject = new AttackObject(ReleaseTarget, projectile, this, doDamageOnce);
        }
        
        public override async void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished)
        {
            var time = 0f;
            var moveDirection = GetMoveDirection(attackObject.transform.position);

            while (time < lifeTime || !attackObject.activeSelf)
            {
                if (GameController && !GameController.IsGameActive) break;
                if (!attackObject || !attackObject.activeSelf) break;

                time += Time.deltaTime;
                attackObject.transform.position += moveDirection * (Time.deltaTime * speed);
                await UniTask.Yield();
            }
            onAttackFinished();
        }

        private void ReleaseTarget(GameObject projectile) => Pool.Release(projectile);
    }
}