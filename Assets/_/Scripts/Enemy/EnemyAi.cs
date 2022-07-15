using System;
using _.Scripts.Player;
using UnityEngine;

namespace _.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private float enemySpeed;
        [SerializeField] private float damage;
        [SerializeField] private Rigidbody2D enemyBody;

        private Transform playerTransform;
        private Action<EnemyAi> onDeathAction;

        private void Awake()
        {
            playerTransform = PlayerController.Instance.transform;
        }

        private void Update()
        {
            var enemyPosition = enemyBody.position;
            var playerPosition = (Vector2)playerTransform.position;
            var normalizedDirection = (playerPosition - enemyPosition).normalized;
            enemyBody.MovePosition(enemyPosition + normalizedDirection * enemySpeed);
        }

        public void Setup(Action<EnemyAi> deathAction, float speed)
        {
            onDeathAction = deathAction;
            this.enemySpeed = speed;
        }
    }
}