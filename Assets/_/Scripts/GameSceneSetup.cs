using _.Scripts.Core;
using _.Scripts.World;
using UnityEngine;

namespace _.Scripts
{
    public class GameSceneSetup : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        public GameObject WorldPrefab;
        public GameObject OverlayPrefab;

        public void Awake()
        {
            gameController.CurrentScore = 0;
            
            var world = Instantiate(WorldPrefab);
            Instantiate(OverlayPrefab);
        }
    }
}