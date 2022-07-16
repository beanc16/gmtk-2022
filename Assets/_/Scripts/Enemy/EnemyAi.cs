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
        [SerializeField] private float enemySpeed;
        [SerializeField] private float damage;
        [SerializeField] private Rigidbody2D enemyBody2D;
        [SerializeField] private GameObject enemyHealthBarContainer;
        [SerializeField] private Image enemyHealthBar;

        private Transform playerTransform;
        private Health _health;
        private UnityAction<EnemyAi> onDeathAction;
        private bool isAlive;

        private void Awake()
        {
            playerTransform = PlayerController.Instance.transform;
        }

        public void Init(UnityAction<EnemyAi> deathAction, float health, float speed)
        {
            onDeathAction = deathAction;
            enemySpeed = speed;
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
            /*
            if (GameController.Instance.IsGameActive == false)
            {
                enemyBody2D.velocity = Vector2.zero;
                return;
            }
            */
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
            /*
            var enemyPosition = enemyBody2D.position;
            var playerPosition = (Vector2)playerTransform.position;
            var playerDistance = playerPosition - enemyPosition;
            if (Vector2.Distance(enemyPosition, playerPosition) < 1.2f)
            {
                return;
            }
            var normalizedDirection = playerDistance.normalized;
            enemyBody2D.MovePosition(enemyPosition + normalizedDirection * enemySpeed);
            */
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
            var playerDistance = playerPosition - enemyPosition;
            var normalizedDirection = playerDistance.normalized;
            
            enemyBody2D.velocity = normalizedDirection * enemySpeed;
        }
    }
}