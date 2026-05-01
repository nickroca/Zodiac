using System.Collections.Generic;
using UnityEngine;

public class NodeTypeAssigner : MonoBehaviour
{
    public float oddRowPackChance = 0.65f;
    public float evenRowPackChance = 0.35f;

    public void AssignTypes(List<List<MapNode>> mapRows)
    {
        if (mapRows == null || mapRows.Count == 0)
        {
            Debug.LogError("mapRows is empty!");
            return;
        }

        int lastRow = mapRows.Count - 1;

        for (int y = 0; y < mapRows.Count; y++)
        {
            foreach (var node in mapRows[y])
            {
                if (node == null) continue;

                // First + last row ALWAYS battle
                if (y == 0 || y == lastRow)
                {
                    node.SetNodeType(MapNode.NodeType.Battle);
                    continue;
                }

                bool isOdd = y % 2 == 1;
                float roll = Random.value;

                bool isPack =
                    isOdd ? roll < oddRowPackChance
                        : roll < evenRowPackChance;

                node.SetNodeType(isPack
                    ? MapNode.NodeType.Pack
                    : MapNode.NodeType.Battle);
            }
        }

        Debug.Log("Node types assigned successfully.");
    }
}