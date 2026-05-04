using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Zodiac;
using NPOI.HSSF.Record.Chart;

public class DeckBuilderManager : MonoBehaviour
{
    public Transform deckGrid;
    public Transform inventoryGrid;
    public GameObject cardPrefab;

    private List<int> deck => GameManager.Instance.playerDeck;
    private List<int> inventory => GameManager.Instance.playerInventory;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        Clear(deckGrid);
        Clear(inventoryGrid);

        foreach (int id in deck)
        {
            CreateCardUI(id, deckGrid, DeckDropZone.ZoneType.Deck);
        }

        foreach (int id in inventory)
        {
            CreateCardUI(id, inventoryGrid, DeckDropZone.ZoneType.Inventory);
        }
    }

    void CreateCardUI(int id, Transform parent, DeckDropZone.ZoneType zoneType)
    {
        GameObject obj = Instantiate(cardPrefab, parent);

        RectTransform rekt = obj.GetComponent<RectTransform>();
        rekt.localScale = Vector3.one;
        rekt.localRotation = Quaternion.identity;
        rekt.anchoredPosition = Vector2.zero;

        obj.transform.SetAsLastSibling();

        Card cardData = GameManager.Instance.GetCardByID(id + 1);

        CardDisplay display = obj.GetComponent<CardDisplay>();
        if(display != null)
        {
            display.cardData = cardData;
            display.UpdateCardDisplay();
        }

        DeckBuilderCardUI ui = obj.GetComponent<DeckBuilderCardUI>();

        DeckDropZone zone = parent.GetComponentInParent<DeckDropZone>();

        ui.Init(id, zone);
        ui.SetZone(zone);
    }

    void Clear(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void MoveCard(int cardID, DeckDropZone.ZoneType from, DeckDropZone.ZoneType to)
    {
        if (from == to)
        {
            return;
        }

        if (from == DeckDropZone.ZoneType.Deck && to == DeckDropZone.ZoneType.Inventory)
        {
            deck.Remove(cardID);
            inventory.Add(cardID);
        } 
        else if (from == DeckDropZone.ZoneType.Inventory && to == DeckDropZone.ZoneType.Deck)
        {
            int count = deck.FindAll(id => id == cardID).Count;

            if (count >= 3)
            {
                return;
            }

            inventory.Remove(cardID);
            deck.Add(cardID);
        }
        RefreshUI();
    }

    public bool IsDeckValid()
    {
        if (deck.Count < 40)
        {
            return false;
        }

        Dictionary<int, int> counts = new Dictionary<int, int>();

        foreach (int id in deck)
        {
            if (!counts.ContainsKey(id))
            {
                counts[id] = 0;
            }

            counts[id]++;

            if (counts[id] > 3)
            {
                return false;
            }
        }

        return true;
    }

    public void OnExitDeckBuilder()
    {
        if (!IsDeckValid())
        {
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
