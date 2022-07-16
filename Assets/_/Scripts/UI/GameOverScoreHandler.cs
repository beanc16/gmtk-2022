using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScoreHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;



    public void SetScoreText(string score = "0")
    {
        scoreText.SetText(score);
    }
}
