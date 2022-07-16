using System;
using _.Scripts.AttackSystem;
using _.Scripts.Core;
using _.Scripts.HealthSystem;
using _.Scripts.Player;
using _.Scripts.World;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        public static int EnemiesAlive;
        
        [SerializeField] private float enemySpeed;
        //[SerializeField] private Rigidbody2D enemyBody2D;
        [SerializeField] private GameObject enemyHealthBarContainer;
        [SerializeField] private Image enemyHealthBar;
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private NavMeshAgent agent;

        private Transform _playerTransform;
        private Health _health;

        public Health GetHeath() => _health;

        private void Start()
        {
            _playerTransform = PlayerController.Instance.transform;
            enemySpeed = Random.Range(enemyData.EnemySpeedMin, enemyData.EnemySpeedMax);
            agent.speed = enemySpeed;
            _health = new Health(enemyData.EnemyHp);
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        private void OnEnable()
        {
            EnemiesAlive++;
        }

        private void OnDisable()
        {
            EnemiesAlive--;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Hit projectile
            if (!col.gameObject.CompareTag(Constants.TagPlayerProjectile)) return;
            AttackObject.Hit(col.transform.parent.gameObject, _health);
            
            if (_health.GetCurrentHealthPoints() > 0f) return;
            
            Die();
        }

        private void Update()
        {
            agent.isStopped = !GameController.Instance.IsGameActive;
            agent.SetDestination(_playerTransform.position);
            if(_playerTransform == null) _playerTransform = PlayerController.Instance.transform;
            
            if (_health.GetHealthPoints() - _health.GetCurrentHealthPoints() < 0.1f)
            {
                enemyHealthBarContainer.SetActive(false);
            }
            else
            {
                enemyHealthBarContainer.SetActive(true);
                float fill = 1f - (_health.GetHealthPoints() - _health.GetCurrentHealthPoints()) / _health.GetHealthPoints();
                if (Mathf.Abs(enemyHealthBar.fillAmount - fill) > 0.1f)
                {
                    enemyHealthBar.fillAmount = fill;
                }
            }
            
            if (_health.GetCurrentHealthPoints() > 0f) return;
            Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
