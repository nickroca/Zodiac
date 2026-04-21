using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using static Sorcery;

public class SorceryStats : MonoBehaviour
{
    public Sorcery sorceryStartData;

    public string cardName;
    public string text;
    public Sprite sprite;
    public SorceryType type;

    private bool statsSet = false;

    void Update()
    {
        if (!statsSet && sorceryStartData != null)
        {
            SetStartStats();
        }
    }

    private void SetStartStats()
    {
        cardName = sorceryStartData.cardName;
        text = sorceryStartData.text;
        sprite = sorceryStartData.sprite;
        type = sorceryStartData.type;
        statsSet = true;
    }
}
