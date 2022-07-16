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
            projectile.transform.rotation = fromTransform.rotation;
            projectile.transform.localScale = Vector3.one;

            while (time < lifeTime)
            {
                if (projectile == null)
                {
                    return;
                }
                time += Time.deltaTime;
                projectile.transform.position += projectile.transform.up * (Time.deltaTime * speed);
                await UniTask.Yield();
            }
            
            Pool.Release(projectile);
            Debug.Log($"Projectile life ended after : {(int)time} sec");
        }
    }
}