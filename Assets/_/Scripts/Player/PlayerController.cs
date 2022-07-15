using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace _.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private float playerSpeed;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Rigidbody2D playerBody2d;
        [SerializeField] private Rigidbody playerBody;

        private Vector2 currentMovement2d;
        private Vector3 currentMovement3d;

        private void Awake()
        {
            Instance = this;
            playerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        }

        private void OnMovement(InputValue value)
        {
            currentMovement2d = value.Get<Vector2>() * playerSpeed;
            currentMovement3d = value.Get<Vector2>() * playerSpeed;
        }

        private void Update()
        {
            if (playerBody2d != null)
            {
                playerBody2d.MovePosition(playerBody2d.position + currentMovement2d);
                return;
            }
            
            playerBody.MovePosition(playerBody.position + currentMovement3d);
        }
    }
}