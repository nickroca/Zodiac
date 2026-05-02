using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapNode currentNode;

    public void SetStartNode(MapNode startNode)
    {
        currentNode = startNode;

        currentNode.SetState(MapNode.NodeState.Current);

        foreach (MapNode node in currentNode.connectedNodes)
        {
            node.SetState(MapNode.NodeState.Available);
        }
    }

    public void MoveToNode(MapNode newNode)
    {
        bool valid = false;

        foreach (MapNode node in currentNode.connectedNodes)
        {
            if (node == newNode)
            {
                valid = true;
                break;
            }
        }

        if (!valid) return;

        currentNode.SetState(MapNode.NodeState.Visited);

        foreach (MapNode node in currentNode.connectedNodes)
        {
            if (node != newNode)
            {
                node.SetState(MapNode.NodeState.Skipped);
            }
        }

        currentNode = newNode;
        currentNode.SetState(MapNode.NodeState.Current);

        foreach (MapNode node in currentNode.connectedNodes)
        {
            node.SetState(MapNode.NodeState.Available);
        }
    }
}