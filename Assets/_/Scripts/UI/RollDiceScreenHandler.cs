using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceScreenHandler : MonoBehaviour
{
    [SerializeField] private List<Image> images;



    public void SetActiveImage(string gameObjectName)
    {
        Image activeImage = images.Find(i => i.gameObject.activeSelf);
        Image inactiveImage = images.Find(i => i.gameObject.name == gameObjectName);

        // Hide the current active image
        if (activeImage != null)
        {
            activeImage.gameObject.SetActive(false);
        }

        // Show the given image
        if (inactiveImage != null)
        {
            inactiveImage.gameObject.SetActive(true);
        }
    }
}
