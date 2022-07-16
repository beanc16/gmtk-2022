using _.Scripts.Core;
using UnityEngine;

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
        private float enemiesTouchingPlayer;
        private float invulnerableTime;
        
        private float playerCurrentHp;
        public float PlayerCurrentHp => playerCurrentHp;
        
        private float playerMaxHp;
        public float PlayerMaxHp => playerMaxHp;

        private void Awake()
        {
            Instance = this;
            playerCurrentHp = playerData.PlayerHp;
            playerMaxHp = playerData.PlayerHp;
        }

        private void Update()
        {
            if (GameController.Instance.IsGameActive == false)
            {
                playerBody2d.velocity = Vector2.zero;
                return;
            }
            
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

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    attackSystem.Attack((int)GameController.Instance.CurrentPlayerAttackType);
                }
            }

            playerBody2d.MovePosition(playerBody2d.position + currentMovement2d);

            DoDamage();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Hit enemy
            if (col.gameObject.CompareTag(Constants.TagEnemy))
            {
                enemiesTouchingPlayer++;
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            // Enemy gotten away from
            if (col.gameObject.CompareTag(Constants.TagEnemy))
            {
                enemiesTouchingPlayer--;
            }
        }

        private void DoDamage()
        {
            invulnerableTime -= Time.deltaTime;
            if (invulnerableTime > 0)
            {
                return;
            }

            invulnerableTime = playerData.InvulnerableTime;
            playerCurrentHp -= enemiesTouchingPlayer;
            if (playerCurrentHp <= 0)
            {
                //Game Over
                GameController.Instance.GameOver();
            }
        }
    }
}