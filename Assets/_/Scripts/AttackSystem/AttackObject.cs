using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    public class AttackObject : MonoBehaviour
    {
        private UnityAction onHitTarget;

        public void SetOnHitTarget(UnityAction onHitAction) => onHitTarget = onHitAction;

        public void AttackHit()
        {
            onHitTarget?.Invoke();
        }
    }
}