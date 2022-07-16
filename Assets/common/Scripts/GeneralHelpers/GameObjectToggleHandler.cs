using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Beanc16.Common.GeneralHelpers
{
    public class GameObjectToggleHandler : MonoBehaviour
    {
        [SerializeField] private GameObject gameObjectToToggle;



        public void Show()
        {
            gameObjectToToggle.SetActive(true);
        }

        public void Hide()
        {
            gameObjectToToggle.SetActive(false);
        }
    }
}
