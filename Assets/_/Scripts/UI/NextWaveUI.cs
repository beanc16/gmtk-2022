﻿using _.Scripts.Player;
using _.Scripts.World;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts.UI
{
    public class NextWaveUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nextWaveText;
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Image playerHealthBar;

        private WaveSpawner waveSpawner;
        private PlayerController playerController;

        private void Awake()
        {
            uiCanvas.worldCamera = Camera.main;
            waveSpawner = FindObjectOfType<WaveSpawner>();
            playerController = PlayerController.Instance;
        }

        private void Update()
        {
            SetPlayerHpBar();
            
            if (waveSpawner.EnemiesLeftAlive > 0)
            {
                nextWaveText.gameObject.SetActive(true);
                nextWaveText.text = "Enemies Remaining :" + waveSpawner.EnemiesLeftAlive.ToString("N0");
                return;
            }
            
            nextWaveText.gameObject.SetActive(waveSpawner.TimeTillNextWave > 0);
            nextWaveText.text = "Next wave In :" + waveSpawner.TimeTillNextWave.ToString("N1");
        }

        private void SetPlayerHpBar()
        {
            float fill = 1f - (playerController.PlayerMaxHp -playerController.PlayerCurrentHp) / playerController.PlayerMaxHp;

            if (Mathf.Abs(playerHealthBar.fillAmount - fill) < 0.1f)
            {
                return;
            }

            playerHealthBar.fillAmount = fill;
        }
    }
}