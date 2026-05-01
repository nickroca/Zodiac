using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public Transform canvasTransform;

    public int rows = 6;
    public int minNodesPerRow = 2;
    public int maxNodesPerRow = 3;

    public float xSpacing = 250f;
    public float ySpacing = 200f;
    public float startY = 0f;

    private List<List<MapNode>> mapRows = new List<List<MapNode>>();
    private MapManager mapManager;
    private NodeTypeAssigner typeAssigner;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        typeAssigner = FindObjectOfType<NodeTypeAssigner>();

        if (mapManager == null)
        {
            Debug.LogError("MapManager not found!");
            return;
        }

        if (MapKeepManager.Instance == null)
        {
            Debug.LogError("MapKeepManager.Instance is null!");
            return;
        }

        Debug.Log($"mapGenerated={MapKeepManager.Instance.mapGenerated}, savedNodes={MapKeepManager.Instance.savedNodes.Count}");

        if (MapKeepManager.Instance.mapGenerated)
        {
            LoadSavedMap();
            RestoreCurrentNode();
            Debug.Log("Load path taken - AssignTypes was NOT called");
            return;
        }

        GenerateMap();

        if (typeAssigner != null)
            typeAssigner.AssignTypes(mapRows);

        MapNode start = mapRows[0][0];
        mapManager.InitCurrentNode(start);

        SaveMap();
        MapKeepManager.Instance.mapGenerated = true;
    }


    void GenerateMap()
    {
        mapRows.Clear();

        for (int y = 0; y < rows; y++)
        {
            int nodeCount = (y == 0 || y == rows - 1)
                ? 1
                : Random.Range(minNodesPerRow, maxNodesPerRow + 1);

            List<MapNode> currentRow = new List<MapNode>();

            for (int x = 0; x < nodeCount; x++)
            {
                GameObject obj = Instantiate(nodePrefab, canvasTransform, false);
                RectTransform rt = obj.GetComponent<RectTransform>();

                float offset = (nodeCount - 1) * xSpacing / 2f;

                rt.anchoredPosition = new Vector2(
                    x * xSpacing - offset,
                    startY + (y * ySpacing)
                );

                MapNode node = obj.GetComponent<MapNode>();
                node.row = y;
                node.col = x;
                node.name = $"Node_{y}_{x}";
                node.SetManager(mapManager);
                node.Unlock();
                node.SetState(MapNode.NodeState.Locked);

                currentRow.Add(node);
            }

            mapRows.Add(currentRow);

            if (y > 0)
                ConnectRows(mapRows[y - 1], currentRow, y == 1, y == rows - 1);
        }
    }


    void SaveMap()
    {
        MapKeepManager.Instance.savedNodes.Clear();

        foreach (var row in mapRows)
        {
            foreach (var node in row)
            {
                NodeData data = new NodeData();
                data.row = node.row;
                data.col = node.col;
                data.position = node.GetComponent<RectTransform>().anchoredPosition;
                data.nodeType = node.nodeType.ToString();
                data.state = (int)node.currentState;

                foreach (var connected in node.connectedNodes)
                    data.connections.Add(new Vector2Int(connected.row, connected.col));

                MapKeepManager.Instance.savedNodes.Add(data);
            }
        }

        Debug.Log($"Map saved. Total nodes saved: {MapKeepManager.Instance.savedNodes.Count}");
    }

    public void SaveMapPublic()
    {
        SaveMap();
    }


    void LoadSavedMap()
    {
        mapRows.Clear();
        Dictionary<Vector2Int, MapNode> nodeLookup = new Dictionary<Vector2Int, MapNode>();

        foreach (NodeData data in MapKeepManager.Instance.savedNodes)
        {
            while (mapRows.Count <= data.row)
                mapRows.Add(new List<MapNode>());

            GameObject obj = Instantiate(nodePrefab, canvasTransform, false);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = data.position;

            MapNode node = obj.GetComponent<MapNode>();
            node.row = data.row;
            node.col = data.col;
            node.name = $"Node_{data.row}_{data.col}";
            node.SetManager(mapManager);
            node.Unlock();

            node.SetNodeType(
                (MapNode.NodeType)System.Enum.Parse(
                    typeof(MapNode.NodeType),
                    data.nodeType
                )
            );

            node.SetState((MapNode.NodeState)data.state);

            Debug.Log($"Loaded node [{data.row},{data.col}] state={(MapNode.NodeState)data.state}");

            mapRows[data.row].Add(node);
            nodeLookup[new Vector2Int(data.row, data.col)] = node;
        }

        foreach (NodeData data in MapKeepManager.Instance.savedNodes)
        {
            MapNode node = nodeLookup[new Vector2Int(data.row, data.col)];
            List<MapNode> connected = new List<MapNode>();

            foreach (Vector2Int pos in data.connections)
            {
                if (nodeLookup.TryGetValue(pos, out MapNode target))
                {
                    connected.Add(target);
                    AddLine(node, target);
                }
                else
                {
                    Debug.LogWarning($"Could not find connected node at {pos}");
                }
            }

            node.connectedNodes = connected.ToArray();
        }
    }


    void RestoreCurrentNode()
    {
        MapNode current = null;

        foreach (var row in mapRows)
        {
            foreach (var node in row)
            {
                if (node.currentState == MapNode.NodeState.Current)
                {
                    current = node;
                    break;
                }
            }
            if (current != null) break;
        }

        if (current == null)
        {
            Debug.LogWarning("No Current node found in saved data, defaulting to [0][0]");
            current = mapRows[0][0];
        }

        mapManager.currentNode = current;

        Debug.Log($"Restored current node: row={current.row} col={current.col}");
    }

    MapNode GetNode(NodeData data)
    {
        foreach (var row in mapRows)
            foreach (var node in row)
                if (node.row == data.row && node.col == data.col)
                    return node;
        return null;
    }


    void ConnectRows(List<MapNode> fromRow, List<MapNode> toRow, bool isFirst, bool isLast)
    {
        if (isFirst)
        {
            foreach (var to in toRow)
                AddConnection(fromRow[0], to);
            return;
        }

        if (isLast)
        {
            foreach (var from in fromRow)
                foreach (var to in toRow)
                    AddConnection(from, to);
            return;
        }

        HashSet<MapNode> connected = new HashSet<MapNode>();

        for (int i = 0; i < fromRow.Count; i++)
        {
            int baseIndex = Mathf.Clamp(i, 0, toRow.Count - 1);

            AddConnection(fromRow[i], toRow[baseIndex]);
            connected.Add(toRow[baseIndex]);

            if (Random.value > 0.5f && toRow.Count > 1)
            {
                int offset = Random.Range(-1, 2);
                int extraIndex = Mathf.Clamp(baseIndex + offset, 0, toRow.Count - 1);

                AddConnection(fromRow[i], toRow[extraIndex]);
                connected.Add(toRow[extraIndex]);
            }
        }

        foreach (var to in toRow)
        {
            if (!connected.Contains(to))
            {
                int index = toRow.IndexOf(to);
                int fromIndex = Mathf.Clamp(index, 0, fromRow.Count - 1);
                AddConnection(fromRow[fromIndex], to);
            }
        }
    }

    void AddConnection(MapNode a, MapNode b)
    {
        List<MapNode> list = new List<MapNode>(a.connectedNodes);
        if (!list.Contains(b))
            list.Add(b);
        a.connectedNodes = list.ToArray();

        AddLine(a, b);
    }

    void AddLine(MapNode a, MapNode b)
    {
        GameObject line = Instantiate(linePrefab, canvasTransform, false);
        line.transform.SetSiblingIndex(1);

        RectTransform rt = line.GetComponent<RectTransform>();

        Vector2 dir = b.GetComponent<RectTransform>().anchoredPosition -
                      a.GetComponent<RectTransform>().anchoredPosition;

        rt.sizeDelta = new Vector2(dir.magnitude, 4f);
        rt.anchoredPosition = a.GetComponent<RectTransform>().anchoredPosition + dir / 2f;
        rt.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}