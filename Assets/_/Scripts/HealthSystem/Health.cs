﻿using _.Scripts.HealthSystem.Interfaces;
using UnityEngine;

namespace _.Scripts.HealthSystem
{
    public class Health : IDamageable<float>
    {
        private readonly float _healthPoints;
        private float _currentHeathPoints;
        
        public float GetHealthPoints() => _healthPoints;
        public float GetCurrentHealthPoints() => _currentHeathPoints;

        public Health(float healthPoints)
        {
            _healthPoints = healthPoints;
            _currentHeathPoints = healthPoints;
        }

        public void Damage(float damageAmount)
        {
            Debug.Log("Damage!");
            _currentHeathPoints -= damageAmount;
        }
    }
}