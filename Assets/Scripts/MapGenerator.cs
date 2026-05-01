using System.Collections.Generic;
using UnityEngine;

//used a lot of online guides

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

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        GenerateMap();
    }

    void GenerateMap()
    {
        mapRows.Clear();

        for (int y = 0; y < rows; y++)
        {
            int nodeCount =
                (y == 0 || y == rows - 1)
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

                node.name = $"Node_{y}_{x}";
                node.SetManager(mapManager);
                node.Unlock();

                node.SetState(MapNode.NodeState.Locked);

                currentRow.Add(node);
            }

            mapRows.Add(currentRow);

            if (y > 0)
            {
                ConnectRows(mapRows[y - 1], currentRow, y == 1, y == rows - 1);
            }
        }

        MapNode start = mapRows[0][0];
        mapManager.SetStartNode(start);
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
        var list = new List<MapNode>(a.connectedNodes);
        list.Add(b);
        a.connectedNodes = list.ToArray();

        GameObject line = Instantiate(linePrefab, canvasTransform, false);
        line.transform.SetSiblingIndex(1);

        RectTransform rt = line.GetComponent<RectTransform>();

        Vector2 dir = b.GetComponent<RectTransform>().anchoredPosition- a.GetComponent<RectTransform>().anchoredPosition;

        rt.sizeDelta = new Vector2(dir.magnitude, 4f);
        rt.anchoredPosition = a.GetComponent<RectTransform>().anchoredPosition + dir / 2f;
        rt.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}