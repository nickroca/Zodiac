using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Draw Cards")]
public class DrawCardss : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
    }

    public int amount;

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        DeckPileManager deck = FindObjectOfType<DeckPileManager>();
        HandManager hand = FindObjectOfType<HandManager>();
        for (int i = 0; i < amount; i++)
        {
            deck.DrawCard(hand);
        }
    }
}