using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Beanc16.Common.EditorTools
{
    /*
    * Instructions:
    * Add to an empty GameObject in Unity
    */
    public class ObjectManagerLine : MonoBehaviour
    {
        //The object we want to add
        public GameObject prefabGObj;

        //Whats the size of the prefab we want to add?
        //You can increase the size if you want to have a gap between the objects
        public float objectSize = 1;
        public float spaceBetweenObjects = 0;

        //Where is the line ending? It starts at the position of the 
        //gameobject the script is attached to
        public Vector3 endOfLinePos;

        // 
        public delegate void OnValidateEvent();
        public event OnValidateEvent OnUpdateVariables;



        //Kill all children to this gameobject
        public void KillAllChildren()
        {
            //Get an array with all children to this transform
            GameObject[] allChildren = GetAllChildren();

            //Now destroy them
            foreach (GameObject child in allChildren)
            {
                DestroyImmediate(child);
            }
        }

        //Get an array with all children to this GO
        private GameObject[] GetAllChildren()
        {
            //This array will hold all children
            GameObject[] allChildren = new GameObject[transform.childCount];

            //Fill the array
            int childCount = 0;
            foreach (Transform child in transform)
            {
                allChildren[childCount] = child.gameObject;
                childCount += 1;
            }

            return allChildren;
        }



        // When a variable is changed in the inspector
        private void OnValidate()
        {
            if (OnUpdateVariables != null)
            {
                OnUpdateVariables();
            }
        }
    }
}
