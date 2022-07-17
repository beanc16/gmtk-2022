using _.Scripts.Enemy;
using _.Scripts.HealthSystem.Interfaces;
using _.Scripts.Player;
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
        
        private bool doDamageOnce;
        private bool hasDoneDamage;
        
        public AttackObject(UnityAction<GameObject> onHitAction, GameObject thisObject, AttackSoBase attackData, bool doDamageOnce = false)
        {
            this.doDamageOnce = doDamageOnce;
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
            if (_attackData.GetDamageOnHit())
            {
                if (hasDoneDamage == false || doDamageOnce == false)
                {
                    toDamage.Damage(_attackData.GetDamage());
                    hasDoneDamage = true;
                }
            }
            if (!_attackData.GetDestroyOnHit()) return;
            ReleaseAttack(hit);
        }
        
        private void ReleaseAttack(GameObject hit)
        {
            if(_finished) return;
            if (hit != _thisObject) return;
            _finished = true;
            _onHitTarget?.Invoke(hit);
            if(!_attackData.GetDamageOnHit()) DoAoEDamage();
            ObjectHit -= OnHit;
        }

        private void DoAoEDamage()
        {
            var pos = (Vector2)_thisObject.transform.position;
            var hits = Physics2D.CircleCastAll(pos, _attackData.GetAoeRange(), Vector2.up);
            
            foreach (var hit in hits)
            {
                //var damageable = hit.transform.GetComponent<IDamageable>();
                if (hit.transform.TryGetComponent<EnemyAi>(out var enemyAi))
                    enemyAi.GetHeath().Damage(_attackData.GetDamage());
                    
                if (hit.transform.TryGetComponent<PlayerController>(out var player) && _attackData.GetDamagePlayer())
                    player.GetPlayerHealth().Damage(_attackData.GetDamage());
            }
        }
    }
}