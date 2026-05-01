using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour, IPointerClickHandler
{
    public MapNode[] connectedNodes;
    public bool isUnlocked = false;

    private MapManager mapManager;
    private Image image;

    public enum NodeState
    {
        Current,
        Visited,
        Skipped,
        Available,
        Locked
    }

    public NodeState currentState;

    void Awake()
    {
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError(gameObject.name + " missing Image component"); 
        }
    }

    public void SetManager(MapManager manager)
    {
        mapManager = manager;
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
    }
}