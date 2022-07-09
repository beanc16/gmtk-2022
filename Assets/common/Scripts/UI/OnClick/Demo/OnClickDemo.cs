using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickDemo : MonoBehaviour
{
    public void LogClickStarted()
    {
        Debug.Log("Started click");
    }

    public void LogClickStopped()
    {
        Debug.Log("Stopped click");
    }
}
