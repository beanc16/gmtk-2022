using UnityEngine;
using TMPro;

public class WinTimeHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    public void SetTimeText(string time = "00:00")
    {
        timeText.SetText(time);
    }
}
