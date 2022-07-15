using UnityEngine;

namespace _.Scripts.Player
{
    public class PlayerCameraFollow : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            var pos = transform.position;
            var mainCameraTransform = mainCamera.transform;
            pos.z = mainCameraTransform.position.z; 
            mainCameraTransform.position = pos;
        }
    }
}