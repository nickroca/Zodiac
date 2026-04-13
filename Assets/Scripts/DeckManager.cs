using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public int startingHandSize = 5;
    public int maxHandSize = 6;
    public int currentHandSize;
    private HandManager handManager;
    private DeckPileManager deckPileManager;
    private bool startBattleRun = true;

    void Start()
    {
        //Load all card assets from the Resources folder
        Card[] cardAssets = Resources.LoadAll<Card>("CardData/Summons");
        System.Random rand = new System.Random();
        Card[] cards = new Card[40];
        for (int i = 0; i < 40; i++)
        {
            cards[i] = cardAssets[rand.Next(cardAssets.Length)];
        }

        //add the loaded cards to the allCards list
        allCards.AddRange(cards);
    }

    void Awake()
    {
        if (deckPileManager == null)
        {
            deckPileManager = FindAnyObjectByType<DeckPileManager>();
        }
        if (handManager == null)
        {
            handManager = FindAnyObjectByType<HandManager>();
        }
    }

    private void Update()
    {
        if (startBattleRun)
        {
            BattleSetup();
        }
    }

    public void BattleSetup()
    {
        handManager.BattleSetup(maxHandSize);
        deckPileManager.MakeDeckPile(allCards);
        deckPileManager.BattleSetup(startingHandSize, maxHandSize);
        startBattleRun = false;
    }
}
