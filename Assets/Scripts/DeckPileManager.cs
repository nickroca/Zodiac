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

    void Start()
    {
        handManager = FindObjectOfType<HandManager>();
    }

    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void MakeDeckPile(List<Card> cardsToAdd)
    {
        drawPile.AddRange(cardsToAdd);
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

    private void UpdateDeckPileCount()
    {
        drawPileCounter.text = drawPile.Count.ToString();
    }

    private void LoseTheGame()
    {
        Debug.Log($"You Lose!");
    }
}
