﻿using System.Collections.Generic;
using _.Scripts.Enemy;
using _.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _.Scripts.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private GameObject gameOverPrefab;
        [SerializeField] private GameObject gameWonPrefab;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private float initialDelay;

        public bool IsGameActive;
        public bool IsRolling;
        public double CurrentScore;
        public PlayerAttackType CurrentPlayerAttackType;
        public int AreaActive = 1;
        public Dictionary<int, int> EnemiesInArea = new Dictionary<int, int>();

        private bool gameWon;
        private float timeSpent; 

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            
            pauseScreen.SetActive(false);
        }

        private void Start()
        {
            if (initialDelay == 0)
            {
                RollFinished(Random.Range(1, 3));
                IsGameActive = true;
            }
        }

        private void Update()
        {
            if (gameWon == true)
            {
                return;
            }
            
            if (initialDelay > 0)
            {
                initialDelay -= Time.deltaTime;
                if (initialDelay <= 0)
                {
                    RollFinished(Random.Range(1, 3));
                    IsGameActive = true;
                }
                return;
            }

            if (EnemyAi.EnemiesAlive <= 0)
            {
                WinGame();
                return;
            }

            if (IsGameActive)
            {
                timeSpent += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen.SetActive(!pauseScreen.activeSelf);
                IsGameActive = !IsGameActive;
            }
        }

        public void ResumeGame()
        {
            IsGameActive = true;
        }
        
        public void GameOver()
        {
            IsGameActive = false;

            Instantiate(gameOverPrefab).GetComponentInChildren<GameOverScoreHandler>().SetScoreText(CurrentScore.ToString("N0"));
        }

        public void RollForRandomEffect()
        {
            IsRolling = true;
        }

        public void RollFinished(int currentFace)
        {
            IsRolling = false;

            CurrentPlayerAttackType = GetAttackTypeForRoll(currentFace);
        }

        private PlayerAttackType GetAttackTypeForRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                    return PlayerAttackType.Projectile;
                case 2:
                    return PlayerAttackType.Melee;
                case 3:
                    return PlayerAttackType.Bomb;
            }

            return PlayerAttackType.None;
        }

        public void UpdateAreaActive(int newAres)
        {
            AreaActive = newAres;
        }

        public void RegisterEnemyInArea(int area)
        {
            if (EnemiesInArea.ContainsKey(area) == false)
            {
                EnemiesInArea.Add(area, 0);
            }
            
            EnemiesInArea[area]++;
        }
        
        public void UnregisterEnemyInArea(int area)
        {
            EnemiesInArea[area]--;
        }

        private void WinGame()
        {
            gameWon = true;
            IsGameActive = !IsGameActive;
            
            Instantiate(gameWonPrefab).GetComponentInChildren<WinTimeHandler>().SetTimeText(timeSpent);
        }

        public int GetEnemiesInCurrentArea()
        {
            return EnemiesInArea[AreaActive];
        }
    }
}