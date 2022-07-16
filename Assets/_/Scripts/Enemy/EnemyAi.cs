using System;
using _.Scripts.AttackSystem;
using _.Scripts.Core;
using _.Scripts.HealthSystem;
using _.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        private const float DIST_TO_MIN_VALUE = 5;
        private const float DIST_TO_MAX_VALUE = 12;
        private const float DIST_DIFFERENCE = 3;
        
        [SerializeField] private float enemySpeed;
        [SerializeField] private float damage;
        [SerializeField] private Rigidbody2D enemyBody2D;
        [SerializeField] private GameObject enemyHealthBarContainer;
        [SerializeField] private Image enemyHealthBar;

        private Transform playerTransform;
        private Health _health;
        private UnityAction<EnemyAi> onDeathAction;
        private bool isAlive;

        public Health GetHeath() => _health;
        
        private float enemySpeedMin;
        private float enemySpeedMax;
        private float enemySpeedMaxChange;
        private float enemySpeedDifference;

        private void Awake()
        {
            playerTransform = PlayerController.Instance.transform;
        }

        public void Init(UnityAction<EnemyAi> deathAction, float health, float speedMin, float speedMax, float changeMax)
        {
            enemySpeedMin = speedMin;
            enemySpeedMax = speedMax;
            enemySpeedDifference = speedMax - speedMin;
            enemySpeedMaxChange = changeMax;
            onDeathAction = deathAction;
            _health = new Health(health);
            Debug.Log(health);
            isAlive = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Hit projectile
            if (!col.gameObject.CompareTag(Constants.TagPlayerProjectile)) return;
            AttackObject.Hit(col.transform.parent.gameObject, _health);
            
            if (_health.GetCurrentHealthPoints() > 0f) return;
            isAlive = false;
            onDeathAction(this);
        }

        private void Update()
        {
            if (isAlive == false)
            {
                return;
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
            isAlive = false;
            onDeathAction(this);
        }

        private void FixedUpdate()
        {
            if (GameController.Instance.IsGameActive == false)
            {
                enemyBody2D.velocity = Vector2.zero;
                return;
            }
            
            var enemyPosition = enemyBody2D.position;
            var playerPosition = (Vector2)playerTransform.position;

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

            var playerDistance = playerPosition - enemyPosition;
            
            var normalizedDirection = playerDistance.normalized;
            
            enemyBody2D.velocity = normalizedDirection.normalized * enemySpeed;
        }
    }
}