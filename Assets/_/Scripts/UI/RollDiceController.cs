using System.Collections.Generic;
using _.Scripts.Core;
using _.Scripts.RollingScene;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceController : MonoBehaviour
{
    public static RollDiceController Instance { get; private set; }

    [SerializeField] private DiceRollScriptableObject diceRollData;

    private bool startRolling;
    private float timeRemaining;
    private float timeToChange;
    private int currentFace;
    
    private int diceFaceMax = int.MinValue;
    private int diceFaceMin = int.MaxValue;

    private readonly Dictionary<int, DiceFace> diceFaces = new Dictionary<int, DiceFace>();
    
    public void Awake()
    {
        Instance = this;
        
        var components = GetComponentsInChildren<DiceFace>(true);

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

        currentFace = diceFaceMax;
        diceFaces[diceFaceMax].gameObject.SetActive(true);
    }
    
    public void RollDie()
    {
        GameController.Instance.RollForRandomEffect();
        
        startRolling = true;
        timeRemaining = diceRollData.TimeToRoll;
        timeToChange = diceRollData.FastChangeTime;
    }
    
    private void Update()
    {
        if (startRolling == false)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            startRolling = false;
            GameController.Instance.RollFinished(currentFace);
            return;
        }
        
        timeToChange -= Time.deltaTime;
        if (timeToChange <= 0)
        {
            if (timeRemaining > diceRollData.TimeForFastChange)
            {
                timeToChange = diceRollData.FastChangeTime;
            } else if (timeRemaining < diceRollData.TimeToSlowdown)
            {
                timeToChange = diceRollData.SlowChangeTime;
            }
            else
            {
                timeToChange = diceRollData.MediumChangeTime;
            }
                
            //Need +1 since Random excludes the upper value
            int randomFace = Random.Range(diceFaceMin, diceFaceMax + 1);

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
