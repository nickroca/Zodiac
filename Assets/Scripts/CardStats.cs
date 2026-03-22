using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using static Summon;
using static Sorcery;
using static Hex;

public class CardStats : MonoBehaviour
{
    public Card card;
    public Summon summonStartData;
    public Sorcery sorceryStartData;
    public Hex hexStartData;
    public int cardType;

    public CardDisplay cardStats;
    public string cardName;
    public string text;
    public Sprite sprite;
    public CardElement element;
    public int rank;
    public int power;
    public int guard;
    public SorceryType sorceryType;
    public HexType hexType;

    private bool statsSet = false;

    private void Update()
    {
        if (!statsSet && summonStartData != null && sorceryStartData != null && hexStartData != null)
        {
            if (card is Summon)
            {
                cardType = 1;
                summonStartData = (Summon)card;
                SetSummonStartStats();
            }
            else if (card is Sorcery)
            {
                cardType = 2;
                sorceryStartData = (Sorcery)card;
                SetSorceryStartStats();
            }
            else if (card is Hex)
            {
                cardType = 3;
                hexStartData = (Hex)card;
                SetHexStartStats();
            }
        }
    }

    private void SetSummonStartStats()
    {
        cardName = summonStartData.cardName;
        text = summonStartData.text;
        sprite = summonStartData.sprite;
        element = summonStartData.element;
        rank = summonStartData.rank;
        power = summonStartData.power;
        guard = summonStartData.guard;
        statsSet = true;
    }

    private void SetSorceryStartStats()
    {
        cardName = sorceryStartData.cardName;
        text = sorceryStartData.text;
        sprite = sorceryStartData.sprite;
        sorceryType = sorceryStartData.type;
        statsSet = true;
    }

    private void SetHexStartStats()
    {
        cardName = hexStartData.cardName;
        text = hexStartData.text;
        sprite = hexStartData.sprite;
        hexType = hexStartData.type;
        statsSet = true;
    }
}
