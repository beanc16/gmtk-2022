using System.Collections.Generic;
using _.Scripts.AttackSystem;
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
        [SerializeField] private float rerollAbilityTime;
        public float GetRerollAbilityTotalTime() => rerollAbilityTime;
        
        private float invulnerableTime;
        
        private float timeTillNewAbility;
        public float GetTimeTillNewAbility() => timeTillNewAbility;

        private Health _health;
        public Health GetPlayerHealth() => _health;
        public float GetTotalHp() => _health.GetHealthPoints();
        public float GetCurrentHp() => _health.GetCurrentHealthPoints();

        private List<GameObject> enemiesTouching = new List<GameObject>(); 

        private void Awake()
        {
            Instance = this;
            _health = new Health(playerData.PlayerHp);
        }

        private void Update()
        {
            if (GameController.Instance.IsGameActive == false)
            {
                return;
            }

            if (!GameController.Instance.IsRolling)
            {
                timeTillNewAbility -= Time.deltaTime;
                if (timeTillNewAbility <= 0)
                {
                    timeTillNewAbility = rerollAbilityTime;
                    RollDiceController.Instance.RollDie();
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                attackSystem.Attack((int)GameController.Instance.CurrentPlayerAttackType);
            }
            DoDamage();
        }

        private void FixedUpdate()
        {
            if (GameController.Instance.IsGameActive == false)
            {
                playerBody2d.velocity = Vector2.zero;
                return;
            }
            
            var currentMovement2d = Vector2.zero;

            currentMovement2d.x = Input.GetAxis("Horizontal");
            currentMovement2d.y = Input.GetAxis("Vertical");
            playerBody2d.velocity = currentMovement2d.normalized * playerData.PlayerSpeed;
            
            if (_health.GetCurrentHealthPoints() <= 0) GameController.Instance.GameOver();
            
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            // Hit enemy
            if (col.gameObject.CompareTag(Constants.TagEnemy))
            {
                enemiesTouching.Add(col.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            // Enemy gotten away from
            if (other.gameObject.CompareTag(Constants.TagEnemy))
            {
                enemiesTouching.Remove(other.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("EnemyProjectile"))
            {
                AttackObject.Hit(col.transform.gameObject, _health);
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
            
            int count = enemiesTouching.Count;
            if (count == 0) return;
            _health.Damage(count);

            for (int i = 0; i < count; i++)
            {
                if (enemiesTouching[i] == null)
                {
                    enemiesTouching.Remove(enemiesTouching[i]);
                    count--;
                    i--;
                }
            }
            
            if (_health.GetCurrentHealthPoints() <= 0)
            {
                //Game Over
                GameController.Instance.GameOver();
            }
        }

        public void RemoveDeadEnemies(GameObject enemy)
        {
            enemiesTouching.Remove(enemy);
        }
    }
}