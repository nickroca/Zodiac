using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MapNode : MonoBehaviour, IPointerClickHandler
{
    public MapNode[] connectedNodes;
    public bool isUnlocked = false;

    private MapManager mapManager;
    private Image image;
    private TextMeshProUGUI label;

    public enum NodeState
    {
        Current,
        Visited,
        Skipped,
        Available,
        Locked
    }

    public enum NodeType
    {
        Battle,
        Pack
    }

    public NodeType nodeType;
    public NodeState currentState;

    void Awake()
    {
        image = GetComponent<Image>();
        label = GetComponentInChildren<TextMeshProUGUI>();

        if (image == null)
            Debug.LogError(gameObject.name + " missing Image component");
    }

    public void SetNodeType(NodeType type)
    {
        nodeType = type;

        if (label != null)
            label.text = type == NodeType.Battle ? "Battle" : "Pack";
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

    public void SetManager(MapManager manager)
    {
        mapManager = manager;
    }

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isUnlocked || currentState != NodeState.Available)
            return;

        mapManager?.MoveToNode(this);

        if (nodeType == NodeType.Battle)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Setup");
        }
        else
        {
            Debug.Log("Pack clicked");
        }
    }
}