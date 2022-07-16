using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "GMTK2022/Attack/Projectile", order = 0)]
    public class AttackSo : AttackSoBase
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float speed;

        public override async void Shoot(Transform fromTransform)
        {
            Debug.Log("Shoot Projectile");
            
            var time = 0f;
            //var projectile = Instantiate(attackPrefab, fromTransform.position, fromTransform.rotation);
            var projectile = Pool.Get();
            projectile.transform.position = fromTransform.position;

            if (projectile.gameObject.TryGetComponent(out AttackObject attackObject))
            {
                attackObject.SetOnHitTarget(HitTarget);
            }
            else
            {
                var newAttackObject = projectile.gameObject.AddComponent(typeof(AttackObject)) as AttackObject;
                // ReSharper disable once Unity.NoNullPropagation
                newAttackObject?.SetOnHitTarget(HitTarget);
            }

            void HitTarget()
            {
                Debug.Log("Happens!");
                Pool.Release(projectile);
            }
            
            
            //todo: Do we want it to shoot towards cursor or have character move towards cursor ? 
            
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var moveDirection = worldPosition - projectile.transform.position;
            moveDirection.z = 0;
            moveDirection = moveDirection.normalized;

            while (time < lifeTime || !projectile.activeSelf)
            {
                if (GameController && !GameController.IsGameActive) break;
                if (!projectile || !projectile.activeSelf) break;

                time += Time.deltaTime;
                projectile.transform.position += moveDirection * (Time.deltaTime * speed);
                await UniTask.Yield();
            }
            
            if(projectile && projectile.activeSelf) Pool.Release(projectile);
            Debug.Log($"Projectile life ended after : {(int)time} sec");
        }
    }
}