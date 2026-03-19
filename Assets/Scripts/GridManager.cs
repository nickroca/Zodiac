using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 4;
    public float offsetx;
    public float offsety;
    public GameObject gridCellPrefab;
    public List<GameObject> gridObjects = new List<GameObject>();
    public GameObject[,] gridCells;

    private void Start()
    {
        CreateGrid();
        transform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }

    void CreateGrid()
    {
        gridCells = new GameObject[width, height];
        Vector2 centerOffset = new Vector2(width / 2.0f - 0.5f + offsetx, height / 2.0f - 0.5f + offsety);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 gridPosition = new Vector2(x, y);
                Vector2 spawnPosition = (gridPosition - centerOffset) * 90;
                GameObject gridCell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);
                gridCell.transform.SetParent(transform);
                gridCell.GetComponent<GridCell>().gridIndex = gridPosition;
                gridCells[x, y] = gridCell;
            }
        }
    }

    public bool AddObjectToGrid(GameObject obj, Vector2 gridPosition)
    {
        if (gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height)
        {
            GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();

            if (cell.cellFull)
            {
                return false;
            }
            else
            {
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position, Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.transform.localScale = new Vector3(3,3,1);
                //Debug.Log($"" + (int)gridPosition.x);
                //float offset = 13.5f * (2 - (int)gridPosition.x);
                //Debug.Log($"" + offset);
                //newObj.transform.position = new Vector2(-1262, -834);
                gridObjects.Add(newObj);
                cell.objectInCell = newObj;
                cell.cellFull = true;
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    
    public bool IsCellFull(Vector2 gridPosition)
    {
        GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();
        if (cell.cellFull)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
