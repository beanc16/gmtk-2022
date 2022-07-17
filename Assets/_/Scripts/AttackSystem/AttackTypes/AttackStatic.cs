using System;
using _.Scripts.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Static", menuName = "GMTK2022/Attack/Static", order = 1)]
    public class AttackStatic : AttackSoBase
    {
        [SerializeField] private float travelTime;
        [SerializeField] private float travelDistance;
        [SerializeField] private AnimationCurve travelCurve;
        
        public override void Shoot(Transform fromTransform)
        {
            if (!IsInLineOfSight(fromTransform.position)) return;
            if(!GameController.IsGameActive) return;
            Debug.Log("Shoot Static");
            var projectile = Pool.Get();
            projectile.transform.position = fromTransform.position;
            var attackObject = new AttackObject(ReleaseTarget, projectile, this);
        }

        public override async void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished)
        {
            var time = 0f;
            var position = attackObject.transform.position;
            
            var moveDirection = GetMoveDirection(position);

            var fromPosition = (Vector2)position;
            Debug.Log("From position = " + fromPosition);
            var toPosition =  (Vector2)position + (Vector2)moveDirection * travelDistance;
            Debug.Log("To Position = " + toPosition);
            
            while (time < lifeTime || !attackObject.activeSelf)
            {
                if (time < travelTime)
                {
                    var lerpTravelTime = Mathf.InverseLerp(0f, travelTime, time);
                    attackObject.transform.position = Vector2.Lerp(fromPosition, toPosition, travelCurve.Evaluate(lerpTravelTime));
                }
                
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