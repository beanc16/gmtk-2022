using System;
using _.Scripts.Core;
using _.Scripts.Player;
using UnityEngine;

namespace _.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private float enemySpeed;
        [SerializeField] private float damage;
        [SerializeField] private Rigidbody2D enemyBody2D;

        private Transform playerTransform;
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
            if (col.gameObject.CompareTag(Constants.TagPlayerProjectile))
            {
                enemyRemainingHp -= 1;
                if (enemyRemainingHp <= 0)
                {
                    isAlive = false;
                    onDeathAction(this);
                }
                return;
            }
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
            var enemyPosition = enemyBody2D.position;
            var playerPosition = (Vector2)playerTransform.position;
            var playerDistance = playerPosition - enemyPosition;
            if (Math.Abs(playerDistance.x) + Math.Abs(playerDistance.y) < 1.5f)
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
            isAlive = true;
        }
    }
}