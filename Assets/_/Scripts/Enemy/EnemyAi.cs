﻿using System;
using _.Scripts.AttackSystem;
using _.Scripts.Core;
using _.Scripts.HealthSystem;
using _.Scripts.Player;
using _.Scripts.World;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        public static int EnemiesAlive;
        
        [SerializeField] private float enemySpeed;
        [SerializeField] private GameObject enemyHealthBarContainer;
        [SerializeField] private Image enemyHealthBar;
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private int enemyRoomIndex;
        [SerializeField] private AttackSystem.AttackSystem attackSystem;
        
        [SerializeField] private SpriteRenderer enemySprite;
        [SerializeField] private bool hasChangingSprite;
        [SerializeField] private Sprite[] spriteToDirection;

        private Transform _playerTransform;
        private Health _health;
        
        private float damageFlashTimeRemaining;
        private float deathAnimationTimeRemaining;
        

        public Health GetHeath() => _health;

        private void Start()
        {
            _playerTransform = PlayerController.Instance.transform;
            enemySpeed = Random.Range(enemyData.EnemySpeedMin, enemyData.EnemySpeedMax);
            agent.speed = enemySpeed;
            _health = new Health(enemyData.EnemyHp, OnDamageTaken);
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            GameController.Instance.RegisterEnemyInArea(enemyRoomIndex);
        }

        private void OnEnable()
        {
            EnemiesAlive++;
        }

        private void OnDisable()
        {
            EnemiesAlive--;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Hit projectile
            if (!col.gameObject.CompareTag(Constants.TagPlayerProjectile)) return;
            AttackObject.Hit(col.transform.parent.gameObject, _health);
        }

        private void Update()
        {
            agent.isStopped = !GameController.Instance.IsGameActive;

            if (GameController.Instance.AreaActive != enemyRoomIndex)
            {
                return;
            }

            if (agent.isStopped)
            {
                return;
            }

            if (DoDeath())
            {
                return;
            }

            DamageFlash();
            
            agent.SetDestination(_playerTransform.position);
            
            DoFacing();

            if (enemyData.HasRangeAttack)
            {
                if (agent.remainingDistance < enemyData.FireDistance)
                {
                    agent.isStopped = true;
                    attackSystem.Attack(0);
                }
                else
                {
                    agent.isStopped = false;
                }
            }
            else
            {
                agent.isStopped = false;
            }

            if (_health.GetHealthPoints() - _health.GetCurrentHealthPoints() < 0.01f)
            {
                enemyHealthBarContainer.SetActive(false);
            }
            else
            {
                enemyHealthBarContainer.SetActive(true);
                float fill = 1f - (_health.GetHealthPoints() - _health.GetCurrentHealthPoints()) / _health.GetHealthPoints();
                if (Mathf.Abs(enemyHealthBar.fillAmount - fill) > 0.1f)
                {
                    enemyHealthBar.fillAmount = fill;
                }
            }
            
            if (_health.GetCurrentHealthPoints() > 0f) return;
            Die();
        }

        private void DoFacing()
        {
            if (hasChangingSprite == false)
            {
                return;
            }
            
            var direction = agent.velocity;

            //We are facing left or right
            if (Math.Abs(direction.y) < 0.1f)
            {
                if (direction.x < 0)
                {
                    enemySprite.sprite = spriteToDirection[6];
                    return;
                }
                
                enemySprite.sprite = spriteToDirection[2];
                return;
            }
            
            //We are facing up or down
            if (Math.Abs(direction.x) < 0.1f)
            {
                if (direction.y < 0)
                {
                    enemySprite.sprite = spriteToDirection[0];
                    return;
                }
                
                enemySprite.sprite = spriteToDirection[4];
                return;
            }

            //Up left
            if (direction.x < 0 && direction.y < 0)
            {
                enemySprite.sprite = spriteToDirection[7];
                return;
            }
            
            //Down Left
            if (direction.x < 0 && direction.y > 0)
            {
                enemySprite.sprite = spriteToDirection[5];
                return;
            }
            
            //Up right
            if (direction.x > 0 && direction.y < 0)
            {
                enemySprite.sprite = spriteToDirection[1];
                return;
            }
            
            //Down Right
            enemySprite.sprite = spriteToDirection[3];
        }

        private void DamageFlash()
        {
            if (damageFlashTimeRemaining <= 0)
            {
                enemySprite.color = Color.white;
                return;
            }

            enemySprite.color = Color.red;
            damageFlashTimeRemaining -= Time.deltaTime;
        }

        private bool DoDeath()
        {
            if (deathAnimationTimeRemaining <= 0)
            {
                return false;
            }

            deathAnimationTimeRemaining -= Time.deltaTime;
            float scale = deathAnimationTimeRemaining / 0.4f;

            transform.localScale = new Vector3(scale, scale, scale);

            if (deathAnimationTimeRemaining <= 0)
            {
                GameController.Instance.UnregisterEnemyInArea(enemyRoomIndex);
                Destroy(gameObject);
            }
            return true;
        }

        private void Die()
        {
            deathAnimationTimeRemaining = 0.4f;
            PlayerController.Instance.RemoveDeadEnemies(gameObject);
        }

        private void OnDamageTaken()
        {
            damageFlashTimeRemaining = 0.15f;
        }
    }
}
