using UnityEngine;
using Zodiac;
using static GameManager;

public class MapManager : MonoBehaviour
{
    public MapNode currentNode;

    public int currentFloor = 1;
    public int maxFloors = 3;

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

    public void AdvanceFloor()
    {
        currentFloor++;

        if (currentFloor > maxFloors)
        {
            Debug.Log("Returning to title");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        GameManager.Instance.savedMap.Clear();

        FindObjectOfType<MapGenerator>().GenerateNewFloor(currentFloor);
    }

    public void SaveMapState()
    {
        GameManager.Instance.savedMap.Clear();

        MapNode[] allNodes = FindObjectsOfType<MapNode>();

        foreach (var node in allNodes)
        {
            GameManager.Instance.savedMap.Add(new MapNodeSaveData
            {
                nodeName = node.name,
                state = node.currentState
            });
        }

        GameManager.Instance.currentNodeName = currentNode.name;
    }

    public void LoadMapState()
    {
        MapNode[] allNodes = FindObjectsOfType<MapNode>();

        foreach(var node in allNodes)
        {
            var data = GameManager.Instance.savedMap.Find(n => n.nodeName == node.name);

            if (data != null)
            {
                node.SetState(data.state);

                if (node.name == GameManager.Instance.currentNodeName)
                {
                    currentNode = node;
                }
            }
        }
    }
}