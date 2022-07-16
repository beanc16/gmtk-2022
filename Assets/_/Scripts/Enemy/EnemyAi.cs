using System;
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

        /*
        private void OnCollisionEnter2D(Collision2D col)
        {
            // Hit projectile
            if (col.gameObject.name == "SpriteEnemy")
            {
                enemyRemainingHp -= 1;
                if (enemyRemainingHp <= 0)
                {
                    isAlive = false;
                    onDeathAction(this);
                }
                return;
            }

            // Hit player
            Debug.Log("BulletCollision " + col.gameObject.name);
        }
        */

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Hit projectile
            if (col.gameObject.CompareTag("Projectile"))
            {
                Debug.Log("Hit projectile" + col.gameObject.name);
                enemyRemainingHp -= 1;
                if (enemyRemainingHp <= 0)
                {
                    Debug.Log("Killed enemy" + col.gameObject.name);
                    isAlive = false;
                    onDeathAction(this);
                }
                return;
            }

            // Hit player
            //Debug.Log("BulletCollision " + col.gameObject.name);
        }

        private void Update()
        {
            if (isAlive == false)
            {
                return;
            }
            var enemyPosition = enemyBody2D.position;
            var playerPosition = (Vector2)playerTransform.position;
            var normalizedDirection = (playerPosition - enemyPosition).normalized;
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