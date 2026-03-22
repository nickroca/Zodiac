using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using static Hex;
using static Sorcery;

public class HexStats : MonoBehaviour
{
    public Hex hexStartData;

    public string cardName;
    public string text;
    public Sprite sprite;
    public HexType type;

    private bool statsSet = false;

    void Update()
    {
        if (!statsSet && hexStartData != null)
        {
            SetStartStats();
        }
    }

    private void SetStartStats()
    {
        cardName = hexStartData.cardName;
        text = hexStartData.text;
        sprite = hexStartData.sprite;
        type = hexStartData.type;
        statsSet = true;
    }
}
