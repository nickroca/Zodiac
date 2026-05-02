using UnityEngine;
using UnityEngine.EventSystems;
using Zodiac;

public class DeckDropZone : MonoBehaviour, IDropHandler
{
    public enum ZoneType {Deck, Inventory};
    public ZoneType zoneType;

    public DeckBuilderManager manager;

    public void OnDrop(PointerEventData eventData)
    {
        DeckBuilderCardUI card = eventData.pointerDrag.GetComponent<DeckBuilderCardUI>();

        if (card == null)
        {
            return;
        }

        DeckDropZone fromZone = card.GetCurrentZone();

        if (fromZone == this)
        {
            return;
        }

        manager.MoveCard(card.cardID, fromZone.zoneType, zoneType);
    }
}
