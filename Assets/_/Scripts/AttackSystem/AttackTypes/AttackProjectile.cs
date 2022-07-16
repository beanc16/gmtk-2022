using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "GMTK2022/Attack/Projectile", order = 0)]
    public class AttackProjectile : AttackSoBase
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float speed;

        public override void Shoot(Transform fromTransform)
        {
            Debug.Log(OnCooldown);
            if(OnCooldown) return; 
            Debug.Log("Shoot Projectile");
            var projectile = Pool.Get();
            projectile.transform.position = fromTransform.position;
            var attackObject = new AttackObject(ReleaseTarget, projectile, this);

            if (attackCooldown > 0f) Cooldown();
        }

        private async void Cooldown()
        {
            OnCooldown = true;
            await UniTask.Delay(TimeSpan.FromSeconds(attackCooldown), ignoreTimeScale: false, cancellationToken: CancellationToken);
            OnCooldown = false;
        }
        
        public override async void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished)
        {
            var time = 0f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var moveDirection = worldPosition - attackObject.transform.position;
            moveDirection.z = 0;
            moveDirection = moveDirection.normalized;
            
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