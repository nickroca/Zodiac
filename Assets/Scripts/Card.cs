using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zodiac {
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]

    public class Card : ScriptableObject 
    {
        public string cardName;
        public CardElement element;
        public int rank;
        public int power;
        public int guard;
        public string text;
        public Sprite sprite;

        public enum CardElement
        {
            FIRE, EARTH, WATER, WIND, LIGHT, DARK
        }
    }
}