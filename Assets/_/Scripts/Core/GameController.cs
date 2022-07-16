using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _.Scripts.Core
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private GameObject gameOverPrefab;
        [SerializeField] private GameObject rollDicePrefab;

        public bool IsGameActive;
        public double CurrentScore;

        private GameObject rollDiceInstance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
            IsGameActive = true;

            rollDiceInstance = Instantiate(rollDicePrefab);
            rollDiceInstance.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(Constants.MainScene);
            }
        }
        
        public void GameOver()
        {
            IsGameActive = false;

            Instantiate(gameOverPrefab).GetComponentInChildren<GameOverScoreHandler>().SetScoreText(CurrentScore.ToString("N0"));
        }

        public void RollForRandomEffect()
        {
            IsGameActive = false;
            
            rollDiceInstance.SetActive(true);
        }

        public void RollFinished()
        {
            IsGameActive = true;
            
            rollDiceInstance.SetActive(false);
        }
    }
}