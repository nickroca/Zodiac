using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

[CreateAssetMenu(fileName = "New Sorcery", menuName = "Card/Sorcery")]
public class Sorcery : Card
{
    public SorceryType type;
    public SorceryEffect effect;
    
    public enum SorceryType
    {
        Normal,
        Permanent,
        Artifact
    }
}