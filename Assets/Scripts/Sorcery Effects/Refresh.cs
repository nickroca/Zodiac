using UnityEngine;
using Zodiac;

[CreateAssetMenu(menuName = "Effects/Refresh")]
public class Refresh : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
    }

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        DeckPileManager deck = FindObjectOfType<DeckPileManager>();
        HandManager hand = FindObjectOfType<HandManager>();
        int drawAmount = hand.cardsInHand.Count;
        for (int i = 0; i < drawAmount; i++)
        {
            deck.AddCardToDeck(hand.cardsInHand[i].GetComponent<Card>());
        }
        for (int i = 0; i < drawAmount; i++)
        {
            deck.DrawCard(hand);
        }
    }
}