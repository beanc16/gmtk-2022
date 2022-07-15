using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Beanc16.Common.EditorTools;

public class EditorButtonTest : MonoBehaviour
{
    [EditorButton]
    public void SayHiInConsole()
    {
        Debug.Log("Hi");
    }

    private void OnGUI()
    {
        //EditorGUILayout.LabelField("After clicking the button, check the console");
    }
}
