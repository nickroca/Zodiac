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
    PlayerLIFE playerLife;
    DiscardManager discardManager;
    private SummonSelect selectedAttacker = null;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLIFE>();
        discardManager = FindObjectOfType<DiscardManager>();
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
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position * 0.8f, Quaternion.identity);
                SummonSelect selectable = newObj.GetComponent<SummonSelect>();
                selectable.gridPosition = gridPosition;
                selectable.controller = playerCard ? 1 : 2;
                newObj.transform.SetParent(cell.transform);
                newObj.transform.localScale = new Vector3(0.7f,0.7f,1);
                if(!playerCard)
                {
                    newObj.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    if (!attackPosition)
                    {
                        newObj.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    }
                }
                else
                {
                    if (!attackPosition)
                    {
                        newObj.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    }
                }
                //newObj.transform.localPosition = new Vector2(10f, 10f);
                newObj.transform.position = new Vector2(newObj.transform.position.x - 114f , newObj.transform.position.y - 34f);
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

    public void RemoveObjectFromGrid(Vector2 gridPosition)
    {
        GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y];
        if (cell.objectInCell != null)
        {
            Destroy(cell.objectInCell);
            cell.objectInCell = null;
            cell.cellFull = false;
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
            if(oppCard.power < oppCard.guard || oppCard.power < 10)
            {
                attackPosition = false;
                oppCard.attackPosition = false;
            }
            Debug.Log($"{oppCard.attackPosition}");
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

    public void doBattle(int attacking, int attackingController, int defending, int defendingController)
    {
        Summon attacker = gridCells[attacking, attackingController].objectInCell.GetComponent<SummonStats>().summonStartData;
        Summon defender = gridCells[defending, defendingController].objectInCell.GetComponent<SummonStats>().summonStartData;
        Vector2 attackerWins = new Vector2(attacking, attackingController);
        Vector2 defenderWins = new Vector2(defending, defendingController);
        Debug.Log($"Battle!");
        if (defender.attackPosition)
        {
            if(attacker.power > defender.power)
            {
                RemoveObjectFromGrid(defenderWins);
            } 
            else if (attacker.power < defender.power)
            {
                discardManager.AddToDiscard(attacker);
                RemoveObjectFromGrid(attackerWins);
            }
            else
            {
                discardManager.AddToDiscard(attacker);
                RemoveObjectFromGrid(attackerWins);
                RemoveObjectFromGrid(defenderWins);
            }
        } 
        else
        {
            if(attacker.power <= defender.guard)
            {
                
            }
            else
            {
                RemoveObjectFromGrid(defenderWins);
            }
        }
    }

    public void OnCellClicked(GridCell clickedCell)
    {
        if (clickedCell.objectInCell != null)  // Ensure the cell isn't empty
        {
            SummonSelect clickedSummon = clickedCell.objectInCell.GetComponent<SummonSelect>();

            if (clickedSummon != null)
            {
                if (clickedSummon.controller == 1) // Player's summon
                {
                    // Select this summon as the attacker if it's the player's summon
                    if (selectedAttacker == null)  // If no attacker is selected
                    {
                        selectedAttacker = clickedSummon;
                        Debug.Log("Selected attacker: " + selectedAttacker.gameObject.name);
                    }
                    else
                    {
                        // If an attacker is already selected, do nothing or display a message
                        Debug.Log("You already have an attacker selected.");
                    }
                }
                else if (clickedSummon.controller == 2) // Opponent's summon
                {
                    if (selectedAttacker != null) // Only if an attacker is selected
                    {
                        // Proceed with the battle between attacker and defender
                        Debug.Log("Selected defender: " + clickedSummon.gameObject.name);
                        int attackerX = (int)selectedAttacker.gridPosition.x;
                        int attackerY = (int)selectedAttacker.gridPosition.y;
                        int defenderX = (int)clickedSummon.gridPosition.x;
                        int defenderY = (int)clickedSummon.gridPosition.y;

                        // Trigger the battle
                        doBattle(attackerX, attackerY, defenderX, defenderY);

                        // After the battle, reset the selection
                        selectedAttacker = null;
                    }
                    else
                    {
                        // If no attacker is selected, show an error or prompt to select one
                        Debug.Log("Please select your attacker first.");
                    }
                }
            }
        }
    }

    public void OnSelectedSummon(SummonSelect selected)
    {
        if (selectedAttacker = null)
        {
            if (selected.controller == 1)
            {
                selectedAttacker = selected;
                Debug.Log("Selected Attacker");
            }
            else
            {
                Debug.Log($"Select your own Summon first");
            }
        return;
        }
        if (selected.controller == 2)
        {
            if (IsCellFull(selected.gridPosition))
            {
                doBattle((int)selectedAttacker.gridPosition.x, (int)selectedAttacker.gridPosition.y, (int)selected.gridPosition.x, (int)selected.gridPosition.y);
                selectedAttacker = null;
            }
            else
            {
                return;
            }
        } 
        else
        {
            selectedAttacker = selected;
        }
        if (selectedAttacker == selected)
        {
            selectedAttacker = null;
            return;
        }
    }
}