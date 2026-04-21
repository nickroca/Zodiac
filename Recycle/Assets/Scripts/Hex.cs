using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using static Sorcery;

[CreateAssetMenu(fileName = "New Hex", menuName = "Card/Hex")]
public class Hex : Card
{
    public HexType type;

    public enum HexType
    {
        Normal,
        Permanent,
        Artifact
    }
}
