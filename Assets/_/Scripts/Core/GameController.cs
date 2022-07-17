using System.Collections.Generic;
using _.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _.Scripts.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private GameObject gameOverPrefab;
        [SerializeField] private GameObject pauseScreen;

        public bool IsGameActive;
        public bool IsRolling;
        public double CurrentScore;
        public PlayerAttackType CurrentPlayerAttackType;
        public int AreaActive = 1;
        public Dictionary<int, int> EnemiesInArea = new Dictionary<int, int>();

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
            RollFinished(Random.Range(1, 3));
            IsGameActive = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //SceneManager.LoadScene(Constants.MainScene);
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
    }
}