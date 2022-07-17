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
        [SerializeField] private GameObject rollingAbilityContainer;
        [SerializeField] private GameObject currentAbilityContainer;

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
            
            currentAbilityContainer.SetActive(GameController.Instance.IsRolling == false);
            rollingAbilityContainer.SetActive(GameController.Instance.IsRolling);

            if (GameController.Instance.CurrentPlayerAttackType != currentAttackIconType)
            {
                currentAttackIcon.gameObject.SetActive(GameController.Instance.CurrentPlayerAttackType != PlayerAttackType.None);
                currentAttackIcon.sprite = playerAttackIconData.GetIconForSprite(GameController.Instance.CurrentPlayerAttackType);
                currentAttackIconType = GameController.Instance.CurrentPlayerAttackType;
            }
            
            nextWaveText.gameObject.SetActive(true); 
            nextWaveText.text = "Enemies Remaining: " + EnemyAi.EnemiesAlive.ToString("N0");
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
            if (!GameController.Instance.IsRolling)
            {
                float fill = 1f - (playerController.GetRerollAbilityTotalTime() -playerController.GetTimeTillNewAbility()) / playerController.GetRerollAbilityTotalTime();

                if (Mathf.Abs(playerTimeToRerollBar.fillAmount - fill) < 0.01f)
                {
                    return;
                }

                playerTimeToRerollBar.fillAmount = fill;
            }
        }
    }
}