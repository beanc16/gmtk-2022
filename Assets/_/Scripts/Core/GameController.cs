using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _.Scripts.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private GameObject gameOverPrefab;

        public bool IsGameActive;
        public double CurrentScore;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            IsGameActive = true;
        }

        public void GameOver()
        {
            IsGameActive = false;

            Instantiate(gameOverPrefab).GetComponentInChildren<GameOverScoreHandler>().SetScoreText(CurrentScore.ToString("N0"));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(Constants.MainScene);
            }
        }
    }
}