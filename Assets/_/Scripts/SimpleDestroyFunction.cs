using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDestroyFunction : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
