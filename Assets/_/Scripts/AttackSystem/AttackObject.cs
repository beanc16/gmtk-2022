using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    public class AttackObject
    {
        public static void Hit(GameObject gameObject) => ObjectHit?.Invoke(gameObject);
        private static event UnityAction<GameObject> ObjectHit; 

        private readonly UnityAction<GameObject> _onHitTarget;
        private readonly GameObject _thisObject;
        private readonly AttackSoBase _attackData;
        private bool _finished;
        
        public AttackObject(UnityAction<GameObject> onHitAction, GameObject thisObject, AttackSoBase attackData)
        {
            _onHitTarget = onHitAction;
            _thisObject = thisObject;
            _attackData = attackData;
            if(attackData.GetDestroyOnHit()) ObjectHit += ReleaseAttack;
            UpdateAttack();
        }

        private void UpdateAttack() => _attackData.AttackUpdate(_thisObject, OnAttackFinished);

        private void OnAttackFinished() => ReleaseAttack(_thisObject);

        private void ReleaseAttack(GameObject hit)
        {
            if(_finished) return;
            if (hit != _thisObject) return;
            _finished = true;
            _onHitTarget?.Invoke(hit);
        }
    }
}