using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace _.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Rigidbody2D playerBody;

        private Vector2 currentMovement;

        private void Awake()
        {
            playerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        }

        private void OnMovement(InputValue value)
        {
            currentMovement = value.Get<Vector2>() * playerSpeed;
        }

        private void Update()
        {
            playerBody.MovePosition(playerBody.position + currentMovement);
            
            
        }
    }
}