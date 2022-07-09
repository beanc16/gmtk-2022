using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverDemo : MonoBehaviour
{
    public void LogHoverEntered()
    {
        Debug.Log("Started hovering");
    }

    public void LogHoverExited()
    {
        Debug.Log("Stopped hovering");
    }
}
