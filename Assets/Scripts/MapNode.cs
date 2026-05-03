using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zodiac;
using TMPro;

public class MapNode : MonoBehaviour, IPointerClickHandler
{
    public MapNode[] connectedNodes;
    public bool isUnlocked = false;

    private MapManager mapManager;
    private Image image;

    private TMP_Text nodeName;

    private int type1;
    /* 0 = Enemy
     * 1 = Card
     */
    private int type2;
    /* Only applies if Type 1 is 0
     * 0 = Slime
     * 1 = Skeleton
     * 2 = Rotten
     * 3 = Minotaur (Boss)
     * 4 = Skeleton Knight
     * 5 = Chimera
     * 6 = Puppeteer
     * 7 = Lich King (Boss)
     * 8 = Grim Reaper
     * 9 = Mechanical Angel
     * 10 = Cyclops
     * 11 = Warden (Boss)
     */

    public enum NodeType
    {
        Enemy, 
        Card, 
        Boss
    }

    public enum NodeState
    {
        Current,
        Visited,
        Skipped,
        Available,
        Locked
    }

    public NodeType nodeType;

    public int value;
    // If Enemy, this is the enemy ID
    // If Card, this is # of cards

    public NodeState currentState;

    GameManager gameManager;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetManager(MapManager manager)
    {
        mapManager = manager;
        gameManager = GetComponent<GameManager>();
    }

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void SetState(NodeState state)
    {
        currentState = state;

        if (image == null) return;

        switch (state)
        {
            case NodeState.Current:
                image.color = new Color(0.3f, 0.8f, 0.3f);
                break;

            case NodeState.Visited:
                image.color = new Color(0.6f, 1f, 0.6f);
                break;

            case NodeState.Skipped:
                image.color = new Color(0.75f, 0.75f, 0.75f);
                break;

            case NodeState.Available:
                image.color = new Color(1f, 1f, 0.6f);
                break;

            case NodeState.Locked:
                image.color = new Color(0.75f, 0.75f, 0.75f);
                break;
        }
    }

   //click the node
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isUnlocked || currentState != NodeState.Available)
            return;

        mapManager?.MoveToNode(this);

        ResolveNode();
    }

    void ResolveNode()
    {
        switch(nodeType)
        {
            case NodeType.Enemy:
                //gameManager.currentEnemyID = value;
                StartBattle();
                break;
            case NodeType.Boss:
                gameManager.currentEnemyID = value;
                GameManager.Instance.pendingFloorAdvance = true;
                StartBattle();
                break;
            case NodeType.Card:
                GiveCards();
                break;
        }
        if (GameManager.Instance.pendingFloorAdvance)
        {
            GameManager.Instance.pendingFloorAdvance = false;
            mapManager.AdvanceFloor();
        }
    }

    void StartBattle()
    {
        GameManager.Instance.currentEnemyID = value;
        GameManager.Instance.returnToMap = true;

        mapManager.SaveMapState();

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    void GiveCards()
    {
        for(int i = 0; i < value; i++)
        {
            Card card = GameManager.Instance.GetRandomCard();
            GameManager.Instance.AddCardtoInventory(card);
            Debug.Log($"Earned card: {card.cardName}");
        }
    }
}