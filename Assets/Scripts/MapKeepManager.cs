using System.Collections.Generic;
using UnityEngine;

public class MapKeepManager : MonoBehaviour
{
    public static MapKeepManager Instance;

    public bool mapGenerated = false;

    public int currentRow = 0;
    public int currentCol = 0;

    public List<NodeData> savedNodes = new List<NodeData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class NodeData
{
    public int row;
    public int col;
    public Vector2 position;
    public string nodeType;
    public int state;
    public List<Vector2Int> connections = new List<Vector2Int>();
}