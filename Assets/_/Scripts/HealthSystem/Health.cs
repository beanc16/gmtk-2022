using System;
using _.Scripts.HealthSystem.Interfaces;
using UnityEngine;

namespace _.Scripts.HealthSystem
{
    public class Health : IDamageable
    {
        private readonly float _healthPoints;
        private float _currentHeathPoints;
        private Action onDamage;
        
        public float GetHealthPoints() => _healthPoints;
        public float GetCurrentHealthPoints() => _currentHeathPoints;

        public Health(float healthPoints, Action damageAction = null)
        {
            _healthPoints = healthPoints;
            _currentHeathPoints = healthPoints;
            onDamage = damageAction;
        }

        public void Damage(float damageAmount)
        {
            onDamage?.Invoke();
            _currentHeathPoints -= damageAmount;
        }
    }
}