using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace _.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float playerSpeed;
        [SerializeField] private PlayerInput playerInput;

        private Vector2 currentMovement;

        private void Awake()
        {
            playerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        }

        private void OnMovement(InputValue value)
        {
            currentMovement = value.Get<Vector2>();
        }

        private void Update()
        {
            var playerTransform = transform;
            var position = playerTransform.position;
            position.x += (playerSpeed * currentMovement.x);
            position.y += (playerSpeed * currentMovement.y);
            playerTransform.position = position;
        }
    }
}