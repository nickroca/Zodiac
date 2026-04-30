using System.Collections;
using System.Collections.Generic;
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
    private SummonSelect selectedAttacker = null;
    public TurnSystem turnSystem;
    public List<GridCell> sacrifices = new List<GridCell>();
    SwordManager swordManager;
    public bool isTargeting = false;
    private System.Action<GridCell> onTargetSelected;
    private System.Func<GridCell, bool> isValidTarget;

    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLIFE>();
        oppLife = FindObjectOfType<OpponentLIFE>();
        discardManager = FindObjectOfType<DiscardManager>();
        turnSystem = FindObjectOfType<TurnSystem>();
        swordManager = FindObjectOfType<SwordManager>();
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
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position, Quaternion.identity);
                SummonSelect selectable = newObj.GetComponent<SummonSelect>();
                selectable.gridPosition = gridPosition;
                selectable.controller = playerCard ? 1 : 2;
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
        for (int i = 0; i < 5; i++)
        {
            Vector2 search = new Vector2(i, 2);
            if (IsCellFull(search))
            {
                summons++;
            }
        }
        if (summons != 5)
        {
            Card[] cardAssets = Resources.LoadAll<Card>("CardData/Summons");
            System.Random rand = new System.Random();
            Summon oppCard = cardAssets[rand.Next(cardAssets.Length)] as Summon;
            Vector2 place = new Vector2(summons, 2);
            if (oppCard.power < oppCard.guard || oppCard.power < 10)
            {
                oppCard.attackPosition = false;
            }
            else
            {
                oppCard.attackPosition = true;
            }
            if (oppCard.attackPosition)
            {
                Debug.Log("Summoning Opponent's Monster in Attack Position");
            }
            else
            {
                Debug.Log("Summoning Opponent's Monster in Defense Position");
            }
            AddObjectToGrid(oppCard.prefab, place, false, oppCard.attackPosition);
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
        if (defender.attackPosition)
        {
            if (attacker.power > defender.power)
            {
                int difference = attacker.power - defender.power;
                Debug.Log("Attacker Wins against Power!");
                if (attackingController == 1)
                {
                    oppLife.currentHP = oppLife.currentHP - difference;
                }
                else
                {
                    playerLife.currentHP = playerLife.currentHP - difference;
                }
                RemoveObjectFromGrid(defenderWins);
            }
            else //if (attacker.power < defender.power)
            {
                Debug.Log("Defender Wins against Attacker!");
                discardManager.AddToDiscard(attacker);
                RemoveObjectFromGrid(attackerWins);
            }
            if (attacker.power == defender.power)
            {
                Debug.Log("Clash Happened");
                discardManager.AddToDiscard(attacker);
                RemoveObjectFromGrid(attackerWins);
                RemoveObjectFromGrid(defenderWins);
            }
        }
        else
        {
            if (attacker.power <= defender.guard)
            {
                Debug.Log("Attacker loses against Guard!");
            }
            else
            {
                Debug.Log("Attacker Wins against Guard!");
                RemoveObjectFromGrid(defenderWins);
            }
        }
    }

    public void OnCellClicked(GridCell clickedCell)
    {
        if (isTargeting)
        {
            if (isValidTarget != null && !isValidTarget(clickedCell))
            {
                Debug.Log("Invalid target");
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
            SummonSelect clickedSummon = clickedCell.objectInCell.GetComponent<SummonSelect>();

            if (clickedSummon != null)
            {
                if (clickedSummon.controller == 1)
                {
                    if (turnSystem.isYourTurn && turnSystem.phaseCount == 2)
                    {
                        if (clickedSummon.GetComponent<SummonStats>().hasAttacked)
                        {
                            Debug.Log("This monster has already attacked");
                            return;
                        }
                        if (selectedAttacker == null)
                        {
                            selectedAttacker = clickedSummon;
                            Debug.Log("Selected attacker: " + selectedAttacker.gameObject.name);
                        }
                        else
                        {
                            // If an attacker is already selected, do nothing
                        }
                    }
                }
                else if (clickedSummon.controller == 2)
                {
                    if (selectedAttacker != null)
                    {
                        Debug.Log("Selected defender: " + clickedSummon.gameObject.name);
                        int attackerX = (int)selectedAttacker.gridPosition.x;
                        int attackerY = (int)selectedAttacker.gridPosition.y;
                        int defenderX = (int)clickedSummon.gridPosition.x;
                        int defenderY = (int)clickedSummon.gridPosition.y;
                        doBattle(attackerX, attackerY, defenderX, defenderY);
                        selectedAttacker.GetComponent<SummonStats>().hasAttacked = true;
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
        if (selectedAttacker != null)
        {
            if (!ControlsSummons(2))
            {
                Summon attacker = selectedAttacker.GetComponent<SummonStats>().summonStartData;
                if (attacker.attackPosition)
                {
                    oppLife.currentHP = oppLife.currentHP - attacker.power;
                    Debug.Log("Attacker direct attack");
                    selectedAttacker.GetComponent<SummonStats>().hasAttacked = true;
                }
                else
                {
                    Debug.Log("Not in attack position");
                }
                selectedAttacker = null;
            }
        }
    }

    public void OnSelectedSummon(SummonSelect selected)
    {
        if (selectedAttacker == null)
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

    public void OpponentAttack()
    {
        // Opponent row is y = 2
        for (int x = 0; x < width; x++)
        {
            Vector2 oppPos = new Vector2(x, 2);

            if (!IsCellFull(oppPos))
                continue;

            Summon attacker = gridCells[x, 2].objectInCell
                .GetComponent<SummonStats>().summonStartData;

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

                Summon defender = gridCells[px, 1].objectInCell
                    .GetComponent<SummonStats>().summonStartData;

                bool canKill = false;

                if (defender.attackPosition)
                {
                    if (attacker.power > defender.power)
                    {
                        Debug.Log("OpponentAttack() Attacker Power Stronger than Defender Power");
                        canKill = true;
                    }
                }
                else
                {
                    if (attacker.power > defender.guard)
                    {
                        Debug.Log("OpponentAttack() Attacker Power Stronger than Defender Guard");
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
                doBattle(x, 2, bestTargetX, 1);
            }
            if (bestTargetX == -1)
            {
                if (!ControlsSummons(1))
                {
                    playerLife.currentHP = playerLife.currentHP - attacker.power;
                    Debug.Log("Opponent direct attack");
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

        SummonSelect summon = cell.objectInCell.GetComponent<SummonSelect>();

        if (summon.controller != 1)
        {
            return;
        }

        if (sacrifices.Contains(cell))
        {
            Debug.Log("Added Sacrifice: " + cell.name);
            sacrifices.Remove(cell);
        }
        else
        {
            Debug.Log("Removed Sacrifice: " + cell.name);
            sacrifices.Add(cell);
        }
    }

    public bool TrySacrifice(int count)
    {
        if (sacrifices.Count != count)
        {
            Debug.Log($"Needs {count} sacrifices. Currently selected: {sacrifices.Count}");
            return false;
        }

        foreach (GridCell cell in sacrifices)
        {
            if (cell != null)
            {
                RemoveObjectFromGrid(cell.gridIndex);
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

        Debug.Log("Select a target...");
    }
}