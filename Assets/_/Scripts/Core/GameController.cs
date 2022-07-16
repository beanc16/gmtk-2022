using System;
using _.Scripts.Player;
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
        public PlayerAttackType CurrentPlayerAttackType;
        public int Wave;

        private GameObject rollDiceInstance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;

            rollDiceInstance = Instantiate(rollDicePrefab);
            Wave = 0;
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
            Wave++;
            IsGameActive = false;
            
            rollDiceInstance.SetActive(true);
        }

        public void RollFinished(int currentFace)
        {
            IsGameActive = true;

            CurrentPlayerAttackType = GetAttackTypeForRoll(currentFace);
            
            rollDiceInstance.SetActive(false);
        }

        private PlayerAttackType GetAttackTypeForRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                case 3:
                case 5:
                    return PlayerAttackType.Projectile;
                case 2:
                case 4:
                case 6:
                    return PlayerAttackType.Projectile;
                    //return PlayerAttackType.Bomb;
            }
            /*switch (roll)
            {
                case 1:
                case 2:
                    return PlayerAttackType.Projectile;
                case 3:
                case 4:
                    return PlayerAttackType.Bomb;
                case 5:
                    return PlayerAttackType.Arrow;
                case 6:
                    return PlayerAttackType.AreaOfEffect;
            }*/

            return PlayerAttackType.None;
        }
    }
}