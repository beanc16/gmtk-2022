using System;
using _.Scripts.AttackSystem;
using _.Scripts.Core;
using _.Scripts.Player;
using UnityEngine;
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
        private float enemyMaxHp;
        private float enemyRemainingHp;
        private Action<EnemyAi> onDeathAction;
        private bool isAlive;

        private void Awake()
        {
            playerTransform = PlayerController.Instance.transform;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Hit projectile
            AttackObject.Hit(col.transform.parent.gameObject);
            if (!col.gameObject.CompareTag(Constants.TagPlayerProjectile)) return;
            enemyRemainingHp -= 1;
            
            if (!(enemyRemainingHp <= 0)) return;
            
            isAlive = false;
            onDeathAction(this);
        }

        private void Update()
        {
            if (GameController.Instance.IsGameActive == false)
            {
                enemyBody2D.velocity = Vector2.zero;
                return;
            }
            
            if (isAlive == false)
            {
                return;
            }

            if (enemyMaxHp - enemyRemainingHp < 0.1f)
            {
                enemyHealthBarContainer.SetActive(false);
            }
            else
            {
                enemyHealthBarContainer.SetActive(true);
                float fill = 1f - (enemyMaxHp - enemyRemainingHp) / enemyMaxHp;
                if (Mathf.Abs(enemyHealthBar.fillAmount - fill) > 0.1f)
                {
                    enemyHealthBar.fillAmount = fill;
                }
            }

            var enemyPosition = enemyBody2D.position;
            var playerPosition = (Vector2)playerTransform.position;
            var playerDistance = playerPosition - enemyPosition;
            if (Vector2.Distance(enemyPosition, playerPosition) < 1.2f)
            {
                return;
            }
            var normalizedDirection = playerDistance.normalized;
            enemyBody2D.MovePosition(enemyPosition + normalizedDirection * enemySpeed);
        }

        public void Setup(Action<EnemyAi> deathAction, float speed, float maxHp)
        {
            onDeathAction = deathAction;
            enemySpeed = speed;
            enemyRemainingHp = maxHp;
            enemyMaxHp = maxHp;
            isAlive = true;
        }
    }
}