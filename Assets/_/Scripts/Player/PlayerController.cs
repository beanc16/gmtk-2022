using UnityEngine;
using UnityEngine.InputSystem;

namespace _.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private PlayerScriptableObject playerData;
        [SerializeField] private AttackSystem.AttackSystem attackSystem;
        [SerializeField] private Rigidbody2D playerBody2d;
        [SerializeField] private bool useNewInputSystem;

        private Vector2 currentMovement2d;
        private float currentHp;

        private void Awake()
        {
            Instance = this;
            currentHp = playerData.PlayerHp;
        }

        private void OnMovement(InputValue value)
        {
            if (useNewInputSystem)
            {
                currentMovement2d = value.Get<Vector2>() * playerData.PlayerSpeed;
            }
        }

        private void OnAttack(InputValue value)
        {
            if (useNewInputSystem)
            {
                attackSystem.Attack(0);
            }
        }

        private void Update()
        {
            if (useNewInputSystem == false)
            {
                currentMovement2d = Vector2.zero;

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    currentMovement2d.y = playerData.PlayerSpeed;
                }

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    currentMovement2d.y = -playerData.PlayerSpeed;
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    currentMovement2d.x = -playerData.PlayerSpeed;
                }

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    currentMovement2d.x = playerData.PlayerSpeed;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    attackSystem.Attack(0);
                }
            }

            playerBody2d.MovePosition(playerBody2d.position + currentMovement2d);
        }
    }
}