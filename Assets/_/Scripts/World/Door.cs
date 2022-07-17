using _.Scripts.Core;
using UnityEngine;

namespace _.Scripts.World
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private int openOnWaveComplete;
        [SerializeField] private GameObject closedDoorPrev;
        [SerializeField] private GameObject closedDoorNext;
        [SerializeField] private DoorTrigger prevTrigger;
        [SerializeField] private DoorTrigger nextTrigger;

        private bool forcedClose;

        private void Awake()
        {
            prevTrigger.OnTriggerAction(OnBetweenRooms);
            nextTrigger.OnTriggerAction(OnNextRoom);
        }

        private void OnBetweenRooms()
        {
            closedDoorPrev.SetActive(true);
            closedDoorNext.SetActive(false);
        }
        
        private void OnNextRoom()
        {
            closedDoorNext.SetActive(true);
            GameController.Instance.UpdateAreaActive(openOnWaveComplete + 1);
        }

        private void Update()
        {
            if (forcedClose)
            {
                return;
            }
            
            if (GameController.Instance != null)
            {
                if (GameController.Instance.EnemiesInArea[openOnWaveComplete] <= 0)
                {
                    closedDoorPrev.SetActive(false);
                    forcedClose = true;
                }
            }
        }
    }
}