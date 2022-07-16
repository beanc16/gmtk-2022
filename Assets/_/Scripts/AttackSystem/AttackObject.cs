using _.Scripts.HealthSystem.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _.Scripts.AttackSystem
{
    public class AttackObject
    {
        public static void Hit(GameObject gameObject, IDamageable toDamage) 
            => ObjectHit?.Invoke(gameObject, toDamage);
        
        private static event UnityAction<GameObject, IDamageable> ObjectHit;

        private readonly UnityAction<GameObject> _onHitTarget;
        private readonly GameObject _thisObject;
        private readonly AttackSoBase _attackData;
        private bool _finished;
        
        public AttackObject(UnityAction<GameObject> onHitAction, GameObject thisObject, AttackSoBase attackData)
        {
            _onHitTarget = onHitAction;
            _thisObject = thisObject;
            _attackData = attackData;
            ObjectHit += OnHit;
            UpdateAttack();
        }

        private void UpdateAttack() => _attackData.AttackUpdate(_thisObject, OnAttackFinished);

        private void OnAttackFinished() => ReleaseAttack(_thisObject);

        private void OnHit(GameObject hit, IDamageable toDamage)
        {
            toDamage.Damage(_attackData.GetDamage());
            if (!_attackData.GetDestroyOnHit()) return;
            ReleaseAttack(hit);
        }
        
        private void ReleaseAttack(GameObject hit)
        {
            if(_finished) return;
            if (hit != _thisObject) return;
            _finished = true;
            _onHitTarget?.Invoke(hit);
            ObjectHit -= OnHit;
        }
    }
}