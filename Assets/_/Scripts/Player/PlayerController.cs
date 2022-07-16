using UnityEngine;
using UnityEngine.InputSystem;

namespace _.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private float playerSpeed;
        [SerializeField] private AttackSystem.AttackSystem attackSystem;
        [SerializeField] private Rigidbody2D playerBody2d;

        private Vector2 currentMovement2d;

        private void Awake()
        {
            Instance = this;
        }

        private void OnMovement(InputValue value)
        {
            currentMovement2d = value.Get<Vector2>() * playerSpeed;
        }

        private void OnAttack(InputValue value)
        {
            attackSystem.Attack(0);
        }

        private void Update()
        {
            playerBody2d.MovePosition(playerBody2d.position + currentMovement2d);
        }
    }
}