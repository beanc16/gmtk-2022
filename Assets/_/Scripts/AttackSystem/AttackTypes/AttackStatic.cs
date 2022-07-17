using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Static", menuName = "GMTK2022/Attack/Static", order = 1)]
    public class AttackStatic : AttackSoBase
    {
        [SerializeField] private float lifeTime;

        public override void Shoot(Transform fromTransform)
        {
            if(!GameController.IsGameActive) return;
            Debug.Log("Shoot Static");
            var projectile = Pool.Get();
            projectile.transform.position = fromTransform.position;
            var attackObject = new AttackObject(ReleaseTarget, projectile, this);
        }

        public override async void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished)
        {
            var time = 0f;
            while (time < lifeTime || !attackObject.activeSelf)
            {
                time += Time.deltaTime;
                await UniTask.Yield();

                if (attackObject == null)
                {
                    break;
                }
            }
            
            onAttackFinished();
        }

        private void ReleaseTarget(GameObject projectile) => Pool.Release(projectile);
    }
}