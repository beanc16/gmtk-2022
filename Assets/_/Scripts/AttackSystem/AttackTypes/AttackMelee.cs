using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "GMTK2022/Attack/Melee", order = 0)]
    public class AttackMelee : AttackSoBase
    {
        public override void Shoot(Transform fromTransform)
        {
            if (!IsInLineOfSight(fromTransform.position)) return;
            if(!GameController.IsGameActive) return;
            Debug.Log("Melee Attack");
            var projectile = Pool.Get();
            projectile.transform.parent = fromTransform;
            projectile.transform.position = projectile.transform.position = attacker == Attacker.PlayerAttack ? fromTransform.position : fromTransform.position + Vector3.up;
            var attackObject = new AttackObject(ReleaseTarget, projectile, this);
        }

        public override async void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished)
        {
            var time = 0f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            attackObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, worldPosition - attackObject.transform.position);
            attackObject.GetComponentInChildren<Animator>().Play("Swipe");

            while (time < lifeTime || !attackObject.activeSelf)
            {
                if (GameController && !GameController.IsGameActive) break;
                if (!attackObject || !attackObject.activeSelf) break;

                time += Time.deltaTime;
                await UniTask.Yield();
            }
            onAttackFinished();
        }
        
        private void ReleaseTarget(GameObject projectile) => Pool.Release(projectile);
    }
}