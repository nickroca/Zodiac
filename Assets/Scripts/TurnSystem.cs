using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zodiac;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public TextMeshProUGUI turnText;
    public int phaseCount;
    /* 0 = Draw Phase
     * 1 = Main Phase 1
     * 2 = Battle Phase
     * 3 = Main Phase 2
     */
    public int summonLimit = 1;
    public int turnCount;
    DeckManager deckManager;
    HandManager handManager;
    GridManager gridManager;
    public Image DP;
    public Image MP1;
    public Image BP;
    public Image MP2;

    void Start()
    {
        isYourTurn = true;
        phaseCount = 0;
        turnCount = 1;
    }

    void Update()
    {
        if(isYourTurn == true)
        {
            turnText.text = "Your Turn";
        } 
        else
        {
            turnText.text = "Opponent Turn";
        }
        UpdatePhaseGraphics();
    }

    public void YourPhaseChange()
    {
        if (isYourTurn)
        {
            if (turnCount == 1)
            {
                if (phaseCount == 0)
                {
                    phaseCount++;
                }
                else
                {
                    isYourTurn = false;
                    turnCount++;
                    phaseCount = 0;
                }
            }
            else
            {
                if (phaseCount != 3)
                {
                    phaseCount += 1;
                }
                else
                {
                    isYourTurn = false;
                    turnCount++;
                    phaseCount = 0;
                }
            }
        }
    }

    public void OpponentPhaseChange()
    {
        if (phaseCount != 3)
        {
            phaseCount += 1;
        }
        else
        {
            isYourTurn = true;
            turnCount++;
            summonLimit = 1;
            phaseCount = 0;
        }
    }

    public void UpdatePhaseGraphics()
    {
        if (phaseCount == 0)
        {
            DP.color = Color.green;
            MP1.color = Color.white;
            BP.color = Color.white;
            MP2.color = Color.white;
        } else if (phaseCount == 1)
        {
            DP.color = Color.white;
            MP1.color = Color.green;
            BP.color = Color.white;
            MP2.color = Color.white;
        }
        else if (phaseCount == 2)
        {
            DP.color = Color.white;
            MP1.color = Color.white;
            BP.color = Color.green;
            MP2.color = Color.white;
        }
        else
        {
            DP.color = Color.white;
            MP1.color = Color.white;
            BP.color = Color.white;
            MP2.color = Color.green;
        }
    }
}
