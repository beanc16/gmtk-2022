using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Beanc16.Common.CameraHelpers
{
    public class ScreenPositionHelper : MonoBehaviour
    {
        [SerializeField]
        private static Camera helperCamera;

        
        private static void TryInitializeCamera()
        {
            if (helperCamera == null)
            {
                helperCamera = FindObjectOfType<Camera>();
            }
        }



        public static Vector3 GetTopLeftOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                0,
                Screen.height,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetMiddleLeftOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                0,
                Screen.height / 2,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetBottomLeftOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                0,
                0,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetTopRightOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                Screen.width,
                Screen.height,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetMiddleRightOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                Screen.width,
                Screen.height / 2,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetBottomRightOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                Screen.width,
                0,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetMiddleTopOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                Screen.width / 2,
                Screen.height,
                helperCamera.nearClipPlane
            ));
        }

        public static Vector3 GetMiddleBottomOfScreen()
        {
            TryInitializeCamera();
            return helperCamera.ScreenToWorldPoint(new Vector3(
                Screen.width / 2,
                0,
                helperCamera.nearClipPlane
            ));
        }
    }
}
