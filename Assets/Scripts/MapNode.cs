using UnityEngine;

public class MapNode : MonoBehaviour
{
    public MapNode[] connectedNodes;
    public bool isUnlocked = false;

    private MapManager mapManager;

    public void SetManager(MapManager manager)
    {
        mapManager = manager;
    }

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void ClickNode()
    {
        if (!isUnlocked) return;

        Debug.Log("Clicked: " + gameObject.name);

        if (mapManager != null)
        {
            mapManager.MoveToNode(this);
        }
    }
}