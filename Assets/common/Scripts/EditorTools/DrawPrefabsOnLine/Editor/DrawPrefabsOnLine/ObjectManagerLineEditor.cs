using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace Beanc16.Common.EditorTools
{
    [CustomEditor(typeof(ObjectManagerLine))]
    public class ObjectManagerLineEditor : Editor
    {
        private ObjectManagerLine objManagerLine;

        private void OnEnable()
        {
            objManagerLine = target as ObjectManagerLine;

            // Hide the handles of the GObj
            Tools.hidden = true;

            // TODO: Make it so the scene updates when variables are changed
            //       (it currently gets upset; find a way to manuall call OnSceneGUI)
            //objManagerLine.OnUpdateVariables += CreateObjects;
        }

        private void OnDisable()
        {
            // Unhide the handles of the GObj
            Tools.hidden = false;
        }



        private void OnSceneGUI()
        {
            CreateObjects();
        }

        private void CreateObjects()
        {
            /*
            * Move the line's start and end positions. Then, add objects 
            * if we have moved one of the positions.
            */

            // End position
            objManagerLine.endOfLinePos = MovePoint(objManagerLine.endOfLinePos);

            // Start position
            objManagerLine.transform.position = MovePoint(objManagerLine.transform.position);
        }

        private Vector3 MovePoint(Vector3 pos)
        {
            // Change position
            if (Tools.current == Tool.Move)
            {
                // Check if we have moved the point
                EditorGUI.BeginChangeCheck();

                // Get & display the new position with an axis (scene view)
                pos = Handles.PositionHandle(pos, Quaternion.identity);

                // The point was moved
                if (EditorGUI.EndChangeCheck())
                {
                    MarkSceneAsDirty();
                    UpdateObjects();
                }
            }

            return pos;
        }

        // Update the objects between the start & end points
        private void UpdateObjects()
        {
            // Kill all current objects on the line
            objManagerLine.KillAllChildren();

            // How many objects can fit between the start & end points?
            
            /*
            * Make sure the size of the object is not zero, otherwise we 
            * can fit an infinite number of objects between the 2 points.
            */
            if ((objManagerLine.objectSize + objManagerLine.spaceBetweenObjects) == 0f)
            {
                return;
            }

            // The num of objs that can fit between the start & end points
            int numOfObjs = GetNumOfObjsThatCanFitBetweenPoints();

            // The direction between the points
            Vector3 directionBetweenPoints = GetDirectionBetweenPoints();

            // Where should we instantiate the first object
            Vector3 firstObjPosition = GetFirstObjPosition();

            // Add the objects
            CreateObjsBetweenPoints(numOfObjs, directionBetweenPoints, 
                                    firstObjPosition);
        }



        private float GetDistanceBetweenPoints()
        {
            return (objManagerLine.endOfLinePos -
                    objManagerLine.transform.position)
                .magnitude;
        }

        private int GetNumOfObjsThatCanFitBetweenPoints()
        {
            float distanceBetween = GetDistanceBetweenPoints();

            /*
            * Divide the distance between the 2 points and the size of one
            * obj to get how many objs we can fit between the 2 points.
            */
            return Mathf.FloorToInt(distanceBetween / 
                                    (objManagerLine.objectSize +
                                    objManagerLine.spaceBetweenObjects));
        }

        private Vector3 GetDirectionBetweenPoints()
        {
            return (objManagerLine.endOfLinePos - 
                    objManagerLine.transform.position)
                .normalized;
        }

        private Vector3 GetFirstObjPosition()
        {
            Vector3 directionBetweenPoints = GetDirectionBetweenPoints();

            return objManagerLine.transform.position + 
                directionBetweenPoints *
                objManagerLine.objectSize * 0.5f;
        }

        private void CreateObjsBetweenPoints(int numOfObjs, 
                                            Vector3 directionBetweenPoints,
                                            Vector3 firstObjPosition)
        {
            for (int i = 0; i < numOfObjs; i++)
            {
                GameObject newGObj = PrefabUtility
                                    .InstantiatePrefab(objManagerLine.prefabGObj)
                                    as GameObject;

                // Parent it so we can delete it by killing all children
                newGObj.transform.parent = objManagerLine.transform;

                // Give it the position
                newGObj.transform.position = firstObjPosition;

                // Orient it by making it look at the position we are going to
                newGObj.transform.forward = directionBetweenPoints;

                // Move to the next object
                firstObjPosition += directionBetweenPoints * 
                                    (objManagerLine.objectSize + 
                                    objManagerLine.spaceBetweenObjects);
            }
        }



        /*
        * Force unity to save changes or Unity may not save when we have 
        * instantiated / removed prefabs despite pressing the save button.
        */
        private void MarkSceneAsDirty()
        {
            UnityEngine.SceneManagement.Scene activeScene = 
                                        UnityEditor.SceneManagement
                                                    .EditorSceneManager
                                                    .GetActiveScene();

            UnityEditor.SceneManagement
                    .EditorSceneManager
                    .MarkSceneDirty(activeScene);
        }
    }
}
