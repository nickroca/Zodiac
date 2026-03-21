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


        void Awake()
        {
            if (prefab == null)
            {
                prefab = (GameObject)Resources.Load("ArtworkPrefabs/Placeholder");
            }
            if (sprite == null)
            {
                prefab = (GameObject)Resources.Load("Artworks/Placeholder");
            }
        }
    }
}