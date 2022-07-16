using _.Scripts.Core;
using _.Scripts.World;
using UnityEngine;

namespace _.Scripts
{
    public class GameSceneSetup : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        //public WaveSpawner WorldPrefab;
        public GameObject PlayerPrefab;
        public GameObject OverlayPrefab;

        public void Awake()
        {
            gameController.CurrentScore = 0;
            
            //var world = Instantiate(WorldPrefab);
            //world.SpawnPlayer(PlayerPrefab);
            Instantiate(OverlayPrefab);
        }
    }
}