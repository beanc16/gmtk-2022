using System;
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
        //[SerializeField] private GameObject rollDicePrefab;

        public bool IsGameActive;
        public double CurrentScore;
        public PlayerAttackType CurrentPlayerAttackType;
        public int AreaActive = 1;
        public Dictionary<int, int> EnemiesInArea = new Dictionary<int, int>();

        //private GameObject rollDiceInstance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;

            //rollDiceInstance = Instantiate(rollDicePrefab);
            RollForRandomEffect();
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
            
            this.RollFinished(Random.Range(1,7));
            //rollDiceInstance.SetActive(true);
        }

        public void RollFinished(int currentFace)
        {
            IsGameActive = true;

            CurrentPlayerAttackType = GetAttackTypeForRoll(currentFace);
            
            //rollDiceInstance.SetActive(false);
        }

        private PlayerAttackType GetAttackTypeForRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                case 2:
                    return PlayerAttackType.Projectile;
                case 3:
                case 4:
                    return PlayerAttackType.Melee;
                case 5:
                case 6:
                    return PlayerAttackType.Bomb;
            }

            return PlayerAttackType.None;
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