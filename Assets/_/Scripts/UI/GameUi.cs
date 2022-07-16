using _.Scripts.Core;
using _.Scripts.Enemy;
using _.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts.UI
{
    public class GameUi : MonoBehaviour
    {
        [SerializeField] private PlayerAttackToIconScriptableObject playerAttackIconData;
        [SerializeField] private TextMeshProUGUI nextWaveText;
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Image playerHealthBar;
        [SerializeField] private Image playerTimeToRerollBar;
        [SerializeField] private Image currentAttackIcon;
        [SerializeField] private PlayerController playerController;

        //private PlayerController playerController;
        private PlayerAttackType currentAttackIconType;

        private void Awake()
        {
            uiCanvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            SetPlayerHpBar();
            SetPlayerTimeToRerollBar();

            if (GameController.Instance.CurrentPlayerAttackType != currentAttackIconType)
            {
                currentAttackIcon.gameObject.SetActive(GameController.Instance.CurrentPlayerAttackType != PlayerAttackType.None);
                currentAttackIcon.sprite = playerAttackIconData.GetIconForSprite(GameController.Instance.CurrentPlayerAttackType);
            }
            
            if (EnemyAi.EnemiesAlive > 0)
            {
                nextWaveText.gameObject.SetActive(true);
                nextWaveText.text = "Enemies Remaining: " + EnemyAi.EnemiesAlive.ToString("N0");
                return;
            }
            
            //nextWaveText.gameObject.SetActive(waveSpawner.TimeTillNextWave > 0);
            //nextWaveText.text = "Next wave In: " + waveSpawner.TimeTillNextWave.ToString("N1");
        }

        private void SetPlayerHpBar()
        {
            float fill = 1f - (playerController.GetTotalHp() -playerController.GetCurrentHp()) / playerController.GetTotalHp();

            if (Mathf.Abs(playerHealthBar.fillAmount - fill) < 0.1f)
            {
                return;
            }

            playerHealthBar.fillAmount = fill;
        }
        
        private void SetPlayerTimeToRerollBar()
        {
            float fill = 1f - (playerController.GetRerollAbilityTotalTime() -playerController.GetTimeTillNewAbility()) / playerController.GetRerollAbilityTotalTime();

            if (Mathf.Abs(playerTimeToRerollBar.fillAmount - fill) < 0.1f)
            {
                return;
            }

            playerTimeToRerollBar.fillAmount = fill;
        }
    }
}