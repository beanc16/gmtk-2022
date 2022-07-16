using UnityEngine;

namespace _.Scripts
{
    public class GameSceneSetup : MonoBehaviour
    {
        public GameObject WorldPrefab;
        public GameObject PlayerPrefab;

        public void Awake()
        {
            var world = Instantiate(WorldPrefab);
            Instantiate(PlayerPrefab, world.transform);
        }
    }
}