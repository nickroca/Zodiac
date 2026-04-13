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
    DeckPileManager deckManager;
    HandManager handManager;
    GridManager gridManager;
    PositionManager positionManager;
    public Image DP;
    public Image MP1;
    public Image BP;
    public Image MP2;
    private bool switched;
    private bool opponentTurn = false;

    void Start()
    {
        deckManager = FindAnyObjectByType<DeckPileManager>();
        handManager = FindAnyObjectByType<HandManager>();
        gridManager = FindAnyObjectByType<GridManager>();
        positionManager = FindAnyObjectByType<PositionManager>();
        
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
        if(!isYourTurn || (phaseCount == 0 || phaseCount == 2))
        {
            positionManager.attackBoard.SetActive(false);
            positionManager.defenseBoard.SetActive(false);
            switched = false;
        }
        else
        {
            if (!switched) {
                positionManager.SwitchToAttack();
                switched = true;
            }
        }
        if(!isYourTurn && !opponentTurn)
        {
            StartCoroutine(OpponentTurn());
        }
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
                    gridManager.ResetAttacks();
                }
            }
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

    IEnumerator OpponentTurn()
    {
        opponentTurn = true;

        phaseCount = 0;
        UpdatePhaseGraphics();
        yield return new WaitForSeconds(1f);

        phaseCount = 1;
        UpdatePhaseGraphics();
        gridManager.PlayOpponentCard();
        yield return new WaitForSeconds(2f);

        phaseCount = 2;
        UpdatePhaseGraphics();
        gridManager.OpponentAttack();
        yield return new WaitForSeconds(2f);

        phaseCount = 3;
        UpdatePhaseGraphics();
        yield return new WaitForSeconds(1f);

        isYourTurn = true;
        turnCount++;
        summonLimit = 1;
        phaseCount = 0;

        deckManager.DrawCard(handManager);

        opponentTurn = false;
    }
}