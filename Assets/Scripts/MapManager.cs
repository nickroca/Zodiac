using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapNode currentNode;

    void Start()
    {
        currentNode.Unlock();
        UnlockNext(currentNode);
    }

    public void MoveToNode(MapNode newNode)
    {
        currentNode = newNode;
        UnlockNext(currentNode);
    }

    void UnlockNext(MapNode node)
    {
        foreach (MapNode n in node.connectedNodes)
        {
            n.Unlock();
        }
    }
}