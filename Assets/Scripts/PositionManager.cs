using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

public class PositionManager : MonoBehaviour
{
    public bool attackPosition;
    public GameObject attackBoard;
    public GameObject defenseBoard;

    void Start()
    {
        attackPosition = true;
        attackBoard.SetActive(true);
        defenseBoard.SetActive(false);
    }

    public void SwitchToDefense()
    {
        attackPosition = false;
        attackBoard.SetActive(false);
        defenseBoard.SetActive(true);
    }

    public void SwitchToAttack()
    {
        attackPosition = true;
        attackBoard.SetActive(true);
        defenseBoard.SetActive(false);
    }
}