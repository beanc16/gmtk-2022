using _.Scripts.World;
using TMPro;
using UnityEngine;

namespace _.Scripts.UI
{
    public class NextWaveUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nextWaveText;
        [SerializeField] private Canvas uiCanvas;

        private WaveSpawner waveSpawner;

        private void Awake()
        {
            uiCanvas.worldCamera = Camera.main;
            waveSpawner = FindObjectOfType<WaveSpawner>();
        }

        private void Update()
        {
            nextWaveText.gameObject.SetActive(waveSpawner.TimeTillNextSpawn > 0);
            nextWaveText.text = "Next set In :" + waveSpawner.TimeTillNextSpawn.ToString("N1");
        }
    }
}