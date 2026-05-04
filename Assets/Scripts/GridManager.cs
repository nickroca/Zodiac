using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zodiac;

public class GridManager : MonoBehaviour
{
    public static int width = 5;
    public static int height = 4;
    public float offsetx;
    public float offsety;
    public GameObject gridCellPrefab;
    public List<GameObject> gridObjects = new List<GameObject>();
    public GridCell[,] gridCells = new GridCell[width, height];
    PlayerLIFE playerLife;
    OpponentLIFE oppLife;
    DiscardManager discardManager;
    private GridCell selectedAttackerCell = null;
    public TurnSystem turnSystem;
    public List<GridCell> sacrifices = new List<GridCell>();
    SwordManager swordManager;
    public bool isTargeting = false;
    private System.Action<GridCell> onTargetSelected;
    private System.Func<GridCell, bool> isValidTarget;
    public System.Action<GameObject> OnOpponentSummon;
    public System.Action<GameObject> OnOpponentAttack;
    public System.Action OnOpponentTurnStart;
    public System.Action OnOpponentTurnEnd;
    GameManager gameManager;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLIFE>();
        oppLife = FindObjectOfType<OpponentLIFE>();
        discardManager = FindObjectOfType<DiscardManager>();
        turnSystem = FindObjectOfType<TurnSystem>();
        swordManager = FindObjectOfType<SwordManager>();
        gameManager = FindObjectOfType<GameManager>();
        GetGrid();
        //transform.localScale = new Vector3(0.8f, 0.8f, 1);
    }

    void GetGrid()
    {
        GridCell[] allCells = GetComponentsInChildren<GridCell>();
        Debug.Log($"Total Cells Found: {allCells.Length - 1}");
        int index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 test = new Vector2(x, y);
                gridCells[x, y] = allCells[index];
                index++;
            }
        }

        /*
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
        */
    }

    public GameObject AddObjectToGrid(GameObject obj, Vector2 gridPosition, bool playerCard, bool attackPosition)
    {
        if (gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height)
        {
            GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();

            if (cell.cellFull)
            {
                return null;
            }
            else
            {
                //If grid is spaced out improperly, change the value after cell.GC<>().pos (higher value is more spaced out, smaller is less)
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position, Quaternion.identity);

                newObj.transform.SetParent(cell.transform);
                newObj.transform.localScale = new Vector3(0.9f, 0.9f, 1);
                if (!playerCard)
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
                newObj.transform.position = new Vector2(newObj.transform.position.x, newObj.transform.position.y);
                gridObjects.Add(newObj);
                cell.objectInCell = newObj;
                cell.cellFull = true;
                return newObj;
            }
        }
        else
        {
            return null;
        }
    }

    public void RemoveObjectFromGrid(Vector2 gridPosition, bool sendToDiscard)
    {
        GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y];
        if (cell.objectInCell != null)
        {
            GameObject obj = cell.objectInCell;

            CardInstance instance = obj.GetComponent<CardInstance>();

            if(instance != null && instance.controller == 1 && sendToDiscard)
            {
                discardManager.AddToDiscard(instance.cardData);
            }

            Destroy(cell.objectInCell);
            cell.objectInCell = null;
            cell.cellFull = false;
        }
    }

    public void PlayOpponentCard()
    {
        int summons = 0;
        int spot = 4;
        for (int i = 4; i >= 0; i--)
        {
            Vector2 search = new Vector2(i, 2);
            if (IsCellFull(search))
            {
                summons++;
            } 
            else
            {
                spot = i;
            }
        }
        
        if (summons != 5)
        {
            List<int> cardList = gameManager.enemyDecks[gameManager.currentEnemyID];
            System.Random rand = new System.Random();
            Vector2 place = new Vector2(spot, 2);
            int number = cardList[rand.Next(cardList.Count)];
            Summon oppCard = gameManager.allCards[number] as Summon;
            //Summon oppCard = cardAssets[rand.Next(cardAssets.Length)] as Summon;
            if (oppCard.power < oppCard.guard)
            {
                oppCard.attackPosition = false;
            }
            else
            {
                oppCard.attackPosition = true;
            }
            if (oppCard.attackPosition)
            {
                HandHolder.Instance.ShowMessage("Your opponent played a Summon in Attack mode.");
            }
            else
            {
                HandHolder.Instance.ShowMessage("Your opponent played a Summon in Defense mode.");
            }
            GameObject placedObj = AddObjectToGrid(oppCard.prefab, place, false, oppCard.attackPosition);

            if(placedObj != null)
            {
                CardInstance instance = placedObj.GetComponent<CardInstance>();
                if (instance == null)
                {
                    instance = placedObj.AddComponent<CardInstance>();
                }
                instance.cardData = oppCard;
                instance.controller = 2;

                GridCell oppCell = gridCells[spot, 2];
                oppCell.objectInCell.GetComponent<SummonStats>().summonStartData = oppCard;

                OnOpponentSummon?.Invoke(placedObj);
            }

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

    public void doBattle(GridCell attackerCell, GridCell defenderCell)
    {
        SummonStats attacker = attackerCell.objectInCell.GetComponent<SummonStats>();
        SummonStats defender = defenderCell.objectInCell.GetComponent<SummonStats>();
        CardInstance attackerInstance = defenderCell.objectInCell.GetComponent<CardInstance>();
        CardInstance defenderInstance = defenderCell.objectInCell.GetComponent<CardInstance>();

        if (defender.attackPosition)
        {
            if (attacker.power > defender.power)
            {
                int difference = attacker.power - defender.power;
                HandHolder.Instance.ShowMessage($"{attacker.cardName} won the battle and destroyed {defender.name}. {difference} damage was done.");
                
                if (attackerInstance.controller == 1)
                {
                    playerLife.currentHP -= difference;
                }
                else
                {
                    oppLife.currentHP -= difference;
                }
                RemoveObjectFromGrid(defenderCell.gridIndex, defenderInstance.controller == 1);
            }
            else if (attacker.power < defender.power)
            {
                int difference = defender.power - attacker.power;
                HandHolder.Instance.ShowMessage($"{defender.cardName} won the battle. {difference} damage was done.");
                
                if (attackerInstance.controller == 1)
                {
                    playerLife.currentHP -= difference;
                }
                else
                {
                    oppLife.currentHP -= difference;
                }
                RemoveObjectFromGrid(attackerCell.gridIndex, attackerInstance.controller == 1);
            }
            else
            {
                HandHolder.Instance.ShowMessage($"Both Summons destroyed each other.");
                RemoveObjectFromGrid(attackerCell.gridIndex, attackerInstance.controller == 1);
                RemoveObjectFromGrid(defenderCell.gridIndex, defenderInstance.controller == 1);
            }
        }
        else
        {
            if (attacker.power > defender.guard)
            {
                HandHolder.Instance.ShowMessage($"{attacker.cardName} won the battle.");
                RemoveObjectFromGrid(defenderCell.gridIndex, defenderInstance.controller == 1);
            }
            else
            {
                int difference = defender.guard - attacker.power;
                HandHolder.Instance.ShowMessage($"{defender.cardName} won the battle. {difference} damage was done.");
                if (attackerInstance.controller != 1)
                {
                    RemoveObjectFromGrid(defenderCell.gridIndex, true);
                    oppLife.currentHP -= difference;
                } 
                else
                {
                    RemoveObjectFromGrid(defenderCell.gridIndex, false);
                    playerLife.currentHP -= difference;
                }
            }
        }
    }

    public void OnCellClicked(GridCell clickedCell)
    {
        if (isTargeting)
        {
            if (isValidTarget != null && !isValidTarget(clickedCell))
            {
                HandHolder.Instance.ShowMessage($"Invalid Target.");
                return;
            }
            
            isTargeting = false;
            Time.timeScale = 1f;
            onTargetSelected?.Invoke(clickedCell);
            onTargetSelected = null;
            isValidTarget = null;

            return;
        }

        if (clickedCell.objectInCell != null)
        {
            CardInstance clickedSummon = clickedCell.objectInCell.GetComponent<CardInstance>();
            SummonStats stats = clickedCell.objectInCell.GetComponent<SummonStats>();

            if (clickedSummon != null && stats != null)
            {
                if (clickedSummon.controller == 1)
                {
                    if (turnSystem.isYourTurn && turnSystem.phaseCount == 2)
                    {
                        if (stats.hasAttacked)
                        {
                            HandHolder.Instance.ShowMessage($"This Summon has already attacked.");
                            return;
                        }

                        selectedAttackerCell = clickedCell;
                        HandHolder.Instance.ShowMessage($"Selected attacker: " + stats.cardName);
                        return;
                    }
                }
                else if (clickedSummon.controller == 2)
                {
                    if (selectedAttackerCell != null)
                    {
                        doBattle(selectedAttackerCell, clickedCell);
                        if (selectedAttackerCell.objectInCell != null) {
                            selectedAttackerCell.objectInCell.GetComponent<SummonStats>().hasAttacked = true;
                        }
                        selectedAttackerCell = null;
                    }
                    else
                    {
                        // If no attacker is selected, show an error or prompt to select one
                        HandHolder.Instance.ShowMessage($"Select an attacker first.");
                    }
                }
            }
        }
        if (selectedAttackerCell != null && !ControlsSummons(2))
        {
            SummonStats attacker = selectedAttackerCell.objectInCell.GetComponent<SummonStats>();
            if (attacker.attackPosition)
            {
                oppLife.currentHP -= attacker.power;
                HandHolder.Instance.ShowMessage($"{attacker.cardName} attacked directly. {attacker.power} damage was done.");
                attacker.hasAttacked = true;
            }
            else
            {
                HandHolder.Instance.ShowMessage($"{attacker.cardName} is not in Attack mode.");
            }
            selectedAttackerCell = null;
        }
    }

    public void OpponentAttack()
    {
        // Opponent row is y = 2
        for (int x = 0; x < width; x++)
        {
            Vector2 oppPos = new Vector2(x, 2);

            if (!IsCellFull(oppPos))
                continue;

            SummonStats attacker = gridCells[x, 2].objectInCell.GetComponent<SummonStats>();

            if (!attacker.attackPosition)
                continue;

            int bestTargetX = -1;
            int bestValue = -1;

            // Player row is y = 1
            for (int px = 0; px < width; px++)
            {
                Vector2 playerPos = new Vector2(px, 1);

                if (!IsCellFull(playerPos))
                    continue;

                SummonStats defender = gridCells[px, 1].objectInCell.GetComponent<SummonStats>();

                bool canKill = false;

                if (defender.attackPosition)
                {
                    if (attacker.power > defender.power)
                    {
                        canKill = true;
                    }
                }
                else
                {
                    if (attacker.power > defender.guard)
                    {
                        canKill = true;
                    }
                }

                if (canKill)
                {
                    int value = defender.power;

                    // Prioritize attack position targets
                    if (defender.attackPosition)
                        value += 100;

                    // Pick strongest killable target
                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestTargetX = px;
                    }
                }
            }

            // Attack best target if found
            if (bestTargetX != -1)
            {
                OnOpponentAttack?.Invoke(gridCells[x, 2].objectInCell);
                if (IsCellFull(oppPos)) {
                    doBattle(gridCells[x, 2], gridCells[bestTargetX, 1]);
                    //doBattle(x, 2, bestTargetX, 1);
                }
            }
            if (bestTargetX == -1)
            {
                if (!ControlsSummons(1))
                {
                    if (IsCellFull(oppPos)) {
                        OnOpponentAttack?.Invoke(gridCells[x, 2].objectInCell);
                        playerLife.currentHP = playerLife.currentHP - attacker.power;
                        HandHolder.Instance.ShowMessage($"{attacker.cardName} attacked you directly. {attacker.power} damage was done.");
                    }
                }
            }
        }
    }

    public bool ControlsSummons(int row)
    {
        for (int x = 0; x < width; x++)
        {
            if (IsCellFull(new Vector2(x, row)))
            {
                return true;
            }
        }
        return false;
    }

    public void ResetAttacks()
    {
        foreach (GameObject obj in gridObjects)
        {
            if (obj != null)
            {
                SummonStats stats = obj.GetComponent<SummonStats>();
                if (stats != null)
                {
                    stats.hasAttacked = false;
                }
            }
        }
    }

    public void ToggleSacrifice(GridCell cell)
    {
        if (cell == null || cell.objectInCell == null)
        {
            return;
        }

        CardInstance instance = cell.objectInCell.GetComponent<CardInstance>();

        if (instance.controller != 1)
        {
            return;
        }

        if (sacrifices.Contains(cell))
        {
            HandHolder.Instance.ShowMessage($"{cell.name} selected as a sacrifice.");
            sacrifices.Remove(cell);
        }
        else
        {
            HandHolder.Instance.ShowMessage($"{cell.name} de-selected as a sacrifice.");
            sacrifices.Add(cell);
        }
    }

    public bool TrySacrifice(int count)
    {
        if (sacrifices.Count != count)
        {
            HandHolder.Instance.ShowMessage($"{count} sacrifices required. You currently have {sacrifices.Count} selected.");
            return false;
        }

        foreach (GridCell cell in sacrifices)
        {
            if (cell != null)
            {
                RemoveObjectFromGrid(cell.gridIndex, true);
            }
        }

        sacrifices.Clear();
        return true;
    }

    public bool CanSummonAttack(int row, int column)
    {
        Vector2 check = new Vector2(row, column);
        if (IsCellFull(check))
        {
            if (gridCells[row, column].objectInCell.GetComponent<SummonStats>().hasAttacked)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public void StartTargeting(System.Func<GridCell, bool> isValid, System.Action<GridCell> callback)
    {
        isTargeting = true;
        isValidTarget = isValid;
        onTargetSelected = callback;

        Time.timeScale = 0f;

        HandHolder.Instance.ShowMessage($"Select a target...");
    }
}