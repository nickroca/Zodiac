using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using static Summon;

public class SummonStats : MonoBehaviour
{
    public Summon summonStartData;

    public string cardName;
    public string text;
    public Sprite sprite;
    public CardElement element;
    public int rank;
    public int power;
    public int guard;
    public bool attackPosition;

    private bool statsSet = false;

    void Update()
    {
        if (!statsSet && summonStartData != null)
        {
            SetStartStats();
        }
    }

    private void SetStartStats()
    {
        cardName = summonStartData.cardName;
        text = summonStartData.text;
        sprite = summonStartData.sprite;
        element = summonStartData.element;
        rank = summonStartData.rank;
        power = summonStartData.power;
        guard = summonStartData.guard;
        attackPosition = summonStartData.attackPosition;
        statsSet = true;
    }
}
