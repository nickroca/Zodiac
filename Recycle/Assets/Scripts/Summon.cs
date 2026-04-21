using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

[CreateAssetMenu(fileName = "New Summon", menuName = "Card/Summon")]
public class Summon : Card
{
    public CardElement element;
    public int rank;
    public int power;
    public int guard;
    public bool attackPosition = true;


    public enum CardElement
    {
        FIRE, EARTH, WATER, WIND, LIGHT, DARK
    }
}
