using System;
using _.Scripts.Core;
using _.Scripts.HealthSystem;
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

        //private Vector2 currentMovement2d;
        private float enemiesTouchingPlayer;
        private float invulnerableTime;

        private Health _health;
        public float GetTotalHp() => _health.GetHealthPoints();
        public float GetCurrentHp() => _health.GetCurrentHealthPoints();

        private void Awake()
        {
            Instance = this;
            _health = new Health(playerData.PlayerHp);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackSystem.Attack((int)GameController.Instance.CurrentPlayerAttackType);
            }
            DoDamage();
            return;
            if (GameController.Instance != null && GameController.Instance.IsGameActive == false)
            {
                playerBody2d.velocity = Vector2.zero;
                return;
            }
            
            if (useNewInputSystem == false)
            {
                var currentMovement2d = Vector2.zero;

                currentMovement2d.x = Input.GetAxis("Horizontal");
                currentMovement2d.y = Input.GetAxis("Vertical");
                /*
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
                */

                //Input.GetKeyDown(KeyCode.Space) ||
                
            }

            //note : Only move with MovePosition if rigidbody is kinematic.
            //playerBody2d.MovePosition(playerBody2d.position + currentMovement2d);

            
        }

        private void FixedUpdate()
        {
            var currentMovement2d = Vector2.zero;

            currentMovement2d.x = Input.GetAxis("Horizontal");
            currentMovement2d.y = Input.GetAxis("Vertical");
            playerBody2d.velocity = currentMovement2d * playerData.PlayerSpeed;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            // Hit enemy
            if (col.gameObject.CompareTag(Constants.TagEnemy))
            {
                enemiesTouchingPlayer++;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            // Enemy gotten away from
            if (other.gameObject.CompareTag(Constants.TagEnemy))
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
            if (enemiesTouchingPlayer == 0) return;
            _health.Damage(enemiesTouchingPlayer);
            if (_health.GetCurrentHealthPoints() <= 0)
            {
                //Game Over
                GameController.Instance.GameOver();
            }
        }
    }
}