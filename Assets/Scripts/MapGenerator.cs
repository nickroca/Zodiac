using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject nodePrefab;
    public GameObject linePrefab;

    [Header("UI")]
    public Transform canvasTransform;

    [Header("Layout")]
    public int rows = 6;

    public int minNodesPerRow = 2;
    public int maxNodesPerRow = 3;

    public float xSpacing = 250f;
    public float ySpacing = 200f;
    public float startY = 0f;

    private List<List<MapNode>> mapRows = new List<List<MapNode>>();

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        mapRows.Clear();

        for (int y = 0; y < rows; y++)
        {
            int nodeCount;

            // START ROW
            if (y == 0)
                nodeCount = 1;
            // END ROW
            else if (y == rows - 1)
                nodeCount = 1;
            else
                nodeCount = Random.Range(minNodesPerRow, maxNodesPerRow + 1);

            List<MapNode> currentRow = new List<MapNode>();

            for (int x = 0; x < nodeCount; x++)
            {
                GameObject nodeObj = Instantiate(nodePrefab);
                nodeObj.transform.SetParent(canvasTransform, false);

                RectTransform rt = nodeObj.GetComponent<RectTransform>();

                float xOffset = (nodeCount - 1) * xSpacing / 2f;

                rt.anchoredPosition = new Vector2(
                    x * xSpacing - xOffset,
                    startY + (y * ySpacing)
                );

                MapNode node = nodeObj.GetComponent<MapNode>();

                node.name = $"Node_{y}_{x}";
                node.Unlock();

                currentRow.Add(node);
            }

            mapRows.Add(currentRow);

            if (y > 0)
            {
                bool isFirstConnection = (y == 1);
                bool isLastConnection = (y == rows - 1);

                ConnectRows(mapRows[y - 1], currentRow, isFirstConnection, isLastConnection);
            }
        }
    }



    void ConnectRows(List<MapNode> fromRow, List<MapNode> toRow, bool isFirst, bool isLast)
    {
        if (isFirst)
        {
            MapNode start = fromRow[0];

            foreach (MapNode to in toRow)
                AddConnection(start, to);

            return;
        }

        if (isLast)
        {
            foreach (MapNode from in fromRow)
            {
                foreach (MapNode to in toRow)
                {
                    AddConnection(from, to);
                }
            }

            return;
        }

        for (int i = 0; i < fromRow.Count; i++)
        {
            MapNode fromNode = fromRow[i];

            int baseIndex = Mathf.Clamp(i, 0, toRow.Count - 1);
            MapNode primary = toRow[baseIndex];

            AddConnection(fromNode, primary);

            if (Random.value > 0.5f && toRow.Count > 1)
            {
                int offset = Random.Range(-1, 2);
                int extraIndex = Mathf.Clamp(baseIndex + offset, 0, toRow.Count - 1);

                AddConnection(fromNode, toRow[extraIndex]);
            }
        }
    }



    void AddConnection(MapNode a, MapNode b)
    {
        List<MapNode> list = new List<MapNode>(a.connectedNodes);
        list.Add(b);
        a.connectedNodes = list.ToArray();

        DrawLine(a.GetComponent<RectTransform>(), b.GetComponent<RectTransform>());
    }

    void DrawLine(RectTransform from, RectTransform to)
    {
        GameObject line = Instantiate(linePrefab);

    
        line.transform.SetParent(canvasTransform, false);
        line.transform.SetSiblingIndex(0); 

        RectTransform rt = line.GetComponent<RectTransform>();

        Vector2 dir = to.anchoredPosition - from.anchoredPosition;
        float distance = dir.magnitude;

        rt.sizeDelta = new Vector2(distance, 4f);
        rt.anchoredPosition = from.anchoredPosition + dir / 2f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);
    }
}