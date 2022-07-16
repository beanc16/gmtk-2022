using _.Scripts.Core;
using UnityEngine;

namespace _.Scripts.World
{
    public class Door : MonoBehaviour
    {
        public int OpenOnWaveComplete;
        public GameObject ClosedDoor;
        public GameObject OpenDoor;

        private bool forcedClose;

        private void Update()
        {
            if (forcedClose)
            {
                return;
            }
            
            if (GameController.Instance != null)
            {
                if (GameController.Instance.EnemiesInArea[OpenOnWaveComplete] <= 0)
                {
                    ClosedDoor.SetActive(false);
                    OpenDoor.SetActive(true);
                }
                else
                {
                    ClosedDoor.SetActive(true);
                    OpenDoor.SetActive(false);
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag(Constants.TagPlayer)) return;

            forcedClose = true;

            ClosedDoor.gameObject.SetActive(true);
        }
    }
}