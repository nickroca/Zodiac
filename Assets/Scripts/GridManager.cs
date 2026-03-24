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
    public GridCell[,] gridCells;

    private void Start()
    {
        CreateGrid();
        transform.localScale = new Vector3(0.8f, 0.8f, 1);
    }

    void CreateGrid()
    {
        gridCells = new GridCell[width, height];
        Vector2 centerOffset = new Vector2(width / 2.0f - 0.5f + offsetx, height / 2.0f - 0.5f + offsety);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 gridPosition = new Vector2(x, y);
                Vector2 spawnPosition = (gridPosition - centerOffset) * 90;
                GridCell gridCell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity).GetComponent<GridCell>();
                gridCell.transform.SetParent(transform);
                gridCell.GetComponent<GridCell>().gridIndex = gridPosition;
                gridCells[x, y] = gridCell;
            }
        }
    }

    public bool AddObjectToGrid(GameObject obj, Vector2 gridPosition, bool playerCard, bool attackPosition)
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
                //If grid is spaced out improperly, change the value after cell.GC<>().pos (higher value is more spaced out, smaller is less)
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position * 0.83f, Quaternion.identity);
                newObj.transform.SetParent(cell.transform);
                newObj.transform.localScale = new Vector3(0.75f,0.75f,1);
                if(!playerCard)
                {
                    if (!attackPosition)
                    {
                        newObj.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    }
                    newObj.transform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    if (!attackPosition)
                    {
                        newObj.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    }
                }
                //newObj.transform.localPosition = new Vector2(10f, 10f);
                newObj.transform.position = new Vector2(newObj.transform.position.x + 63f , newObj.transform.position.y + 35f);
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

    public void PlayOpponentCard()
    {
        int summons = 0;
        for(int i = 0; i < 5; i++)
        {
            Vector2 search = new Vector2(i, 2);
            if (IsCellFull(search))
            {
                summons++;
            }
        }
        if(summons != 5)
        {
            Card[] cardAssets = Resources.LoadAll<Card>("CardData/Summons");
            System.Random rand = new System.Random();
            Summon oppCard = cardAssets[rand.Next(cardAssets.Length)] as Summon;
            Vector2 place = new Vector2(summons, 2);
            bool attackPosition = true;
            Debug.Log($"{oppCard.power}");
            AddObjectToGrid(oppCard.prefab, place, false, attackPosition);
            GridCell oppCell = gridCells[summons, 2];
            oppCell.objectInCell.GetComponent<SummonStats>().summonStartData = oppCard;

            //cell.objectInCell.GetComponent<SummonStats>().summonStartData = summonCard;
        }
    }
    
    public bool IsCellFull(Vector2 gridPosition)
    {
        GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();
        if (cell.cellFull)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
