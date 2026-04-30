using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zodiac {
    //[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

    public class Card : ScriptableObject 
    {
        public string cardName;
        public string text;
        public Sprite sprite;
        public GameObject prefab;
        public int ID;
        public int rarity;
        /* Rarities:
         * 1 = Common
         * 2 = Rare
         * 3 = Super Rare
         * 4 = Ultra Rare
         */
    }
}