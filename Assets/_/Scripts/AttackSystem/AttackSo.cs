using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "GMTK2022/Attack/Projectile", order = 0)]
    public class AttackSo : AttackSoBase
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float speed;

        public override void Shoot(Transform fromTransform)
        {
            Debug.Log("Shoot Projectile");
            //var projectile = Instantiate(attackPrefab, fromTransform.position, fromTransform.rotation);
            var projectile = Pool.Get();
            projectile.transform.position = fromTransform.position;

            var attackObject = new AttackObject(ReleaseTarget, projectile, this);

            //todo: Do we want it to shoot towards cursor or have character move towards cursor ? 
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