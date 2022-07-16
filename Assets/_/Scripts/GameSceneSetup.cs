using _.Scripts.Core;
using UnityEngine;

namespace _.Scripts
{
    public class GameSceneSetup : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        public GameObject WorldPrefab;
        public GameObject PlayerPrefab;
        public GameObject OverlayPrefab;

        public void Awake()
        {
            gameController.CurrentScore = 0;
            
            var world = Instantiate(WorldPrefab);
            Instantiate(PlayerPrefab, world.transform);
            Instantiate(OverlayPrefab);
        }
    }
}