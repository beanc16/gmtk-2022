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
        
        private void Awake()
        {
            Instance = this;
            IsGameActive = true;
        }

        public void GameOver()
        {
            IsGameActive = false;

            Instantiate(gameOverPrefab);
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