using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using TMPro;

public class DeckPileManager : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();
    public int startingHandSize = 5;
    private int currentIndex = 0;
    public int maxHandSize = 6;
    public int currentHandSize;
    private HandManager handManager;
    private DiscardManager discardManager;
    public TextMeshProUGUI drawPileCounter;
    GameManager gameManager;

    void Start()
    {
        handManager = FindObjectOfType<HandManager>();
        gameManager = FindObjectOfType<GameManager>();
        List<int> deck = gameManager.playerDeck;
        for (int i = 0; i < deck.Count; i++)
        {
            drawPile.Add(gameManager.allCards[deck[i]]);
        }
        Utility.Shuffle(drawPile);
        UpdateDeckPileCount();
        BattleSetup(5, 6);
    }

    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void MakeDeckPile()
    {
        List<int> deck = gameManager.playerDeck;
        for (int i = 0; i < deck.Count; i++)
        {
            drawPile.Add(gameManager.allCards[deck[i]]);
        }
        Utility.Shuffle(drawPile);
        UpdateDeckPileCount();
    }

    public void BattleSetup(int numberOfCardsToDraw, int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            DrawCard(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            LoseTheGame();
        }
        if (currentHandSize < maxHandSize)
        {
            Card nextCard = drawPile[currentIndex];
            handManager.AddCardToHand(nextCard);
            drawPile.RemoveAt(currentIndex);
            UpdateDeckPileCount();
            if (drawPile.Count > 0) currentIndex %= drawPile.Count;
        }
    }

    public void AddCardToDeck(Card card)
    {
        drawPile.Add(card);
        Utility.Shuffle(drawPile);
        UpdateDeckPileCount();
    }

    private void UpdateDeckPileCount()
    {
        drawPileCounter.text = drawPile.Count.ToString();
    }

    private void LoseTheGame()
    {
        Debug.Log($"You Lose!");
    }
}
