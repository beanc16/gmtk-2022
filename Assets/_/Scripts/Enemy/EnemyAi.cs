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
            if(_playerTransform == null)
            {
                _playerTransform = PlayerController.Instance.transform;
            }

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

        private void FixedUpdate()
        {
            agent.SetDestination(_playerTransform.position);
            /*
            if (GameController.Instance.IsGameActive == false)
            {
                enemyBody2D.velocity = Vector2.zero;
                return;
            }
            
            var enemyPosition = enemyBody2D.position;
            var playerPosition = (Vector2)_playerTransform.position;

            var playerDistance = playerPosition - enemyPosition;
            
            var normalizedDirection = playerDistance.normalized;
            
            enemyBody2D.velocity = normalizedDirection.normalized * enemySpeed;
            */
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}

/*

            private const float DIST_TO_MIN_VALUE = 5;
        private const float DIST_TO_MAX_VALUE = 12;
        private const float DIST_DIFFERENCE = 7;

            float totalDistance = Vector2.Distance(playerPosition, enemyPosition);
            if (totalDistance < DIST_TO_MIN_VALUE)
            {
                enemySpeed -= enemySpeedMaxChange;
                if (enemySpeed < enemySpeedMin)
                {
                    enemySpeed = enemySpeedMin;
                }
            } else if (totalDistance > DIST_TO_MAX_VALUE)
            {
                enemySpeed += enemySpeedMaxChange;
                if (enemySpeed > enemySpeedMax)
                {
                    enemySpeed = enemySpeedMax;
                }
            }
            else
            {
                totalDistance -= DIST_TO_MIN_VALUE;
                
                float destSpeed = enemySpeedMin +
                                 enemySpeedDifference * (DIST_DIFFERENCE - totalDistance) / DIST_DIFFERENCE;

                if (destSpeed < enemySpeed)
                {
                    enemySpeed -= enemySpeedMaxChange;
                    if (enemySpeed < destSpeed)
                    {
                        enemySpeed = destSpeed;
                    }
                }
                else
                {
                    enemySpeed += enemySpeedMaxChange;
                    if (enemySpeed > destSpeed)
                    {
                        enemySpeed = destSpeed;
                    }
                }
            }
*/