using System;
using _.Scripts.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _.Scripts.AttackSystem
{
    [CreateAssetMenu(fileName = "Static", menuName = "GMTK2022/Attack/Static", order = 1)]
    public class AttackStatic : AttackSoBase
    {
        [SerializeField] private float travelTime;
        [SerializeField] private float travelDistance;
        [SerializeField] private AnimationCurve travelCurve;
        [SerializeField] private GameObject onFinishedObject;
        [SerializeField] private bool finishedObjectHasRandomRotation;
        
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

            if (onFinishedObject)
            {
                var finishObject = Instantiate(onFinishedObject, attackObject.transform.position, Quaternion.identity);
                if (finishedObjectHasRandomRotation)
                    finishObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
            }

            onAttackFinished();
        }

        private void ReleaseTarget(GameObject projectile) => Pool.Release(projectile);
    }
}