using System.Collections.Generic;
using _.Scripts.Core;
using _.Scripts.RollingScene;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceScreenHandler : MonoBehaviour
{
    private const float FAST_CHANGE_TIME = 0.1f;
    private const float MEDIUM_CHANGE_TIME = 0.4f;
    private const float SLOW_CHANGE_TIME = 0.8f;
    
    [SerializeField] private GameObject DicePanel;
    [SerializeField] private DiceRollScriptableObject diceRollData;
    [SerializeField] private Button rollDie;
    [SerializeField] private Image timeRemainingBar;

    private bool startRolling;
    private float timeRemaining;
    private float timeToChange;
    private int currentFace;
    
    private int diceFaceMax = int.MinValue;
    private int diceFaceMin = int.MaxValue;

    private readonly Dictionary<int, DiceFace> diceFaces = new Dictionary<int, DiceFace>();
    
    public void Awake()
    {
        var components = DicePanel.GetComponentsInChildren<DiceFace>();

        foreach (var face in components)
        {
            diceFaces.Add(face.DiceValue, face);
            face.gameObject.SetActive(false);

            if (face.DiceValue > diceFaceMax)
            {
                diceFaceMax = face.DiceValue;
            }

            if (face.DiceValue < diceFaceMin)
            {
                diceFaceMin = face.DiceValue;
            }
        }
        
        rollDie.onClick.AddListener(OnRollDie);

        currentFace = diceFaceMax;
        diceFaces[diceFaceMax].gameObject.SetActive(true);

        timeRemainingBar.fillAmount = 1;
    }
    
    private void OnRollDie()
    {
        startRolling = true;
        timeRemaining = diceRollData.TimeToRoll;
        timeToChange = FAST_CHANGE_TIME;
    }
    
    private void Update()
    {
        if (startRolling == false)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        timeRemainingBar.fillAmount = 1f - (diceRollData.TimeToRoll - timeRemaining) / diceRollData.TimeToRoll;

        if (timeRemaining <= 0)
        {
            startRolling = false;
            GameController.Instance.RollFinished();
            return;
        }
        
        timeToChange -= Time.deltaTime;
        if (timeToChange <= 0)
        {
            if (timeRemaining > diceRollData.TimeForFastChange)
            {
                timeToChange = FAST_CHANGE_TIME;
            } else if (timeRemaining < diceRollData.TimeToSlowdown)
            {
                timeToChange = SLOW_CHANGE_TIME;
            }
            else
            {
                timeToChange = MEDIUM_CHANGE_TIME;
            }
                
            int randomFace = Random.Range(diceFaceMin, diceFaceMax);

            if (randomFace == currentFace)
            {
                return;
            }
                
            diceFaces[currentFace].gameObject.SetActive(false);
            currentFace = randomFace;
            diceFaces[currentFace].gameObject.SetActive(true);
        }
    }
}
