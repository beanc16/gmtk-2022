using System;
using UnityEngine;
using TMPro;

public class WinTimeHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    public void SetTimeText(float timeInSeconds)
    {
        var span = TimeSpan.FromSeconds(timeInSeconds);
        timeText.SetText(span.ToString(@"hh\:mm\:ss"));
    }
}
