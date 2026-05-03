using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/All Low Destroy")]
public class AllLowDestroy : SorceryEffect
{
    public int amount;

    DiscardManager discardManager = FindObjectOfType<DiscardManager>();

    public override void Activate(GridManager gridManager, GridCell target)
    {
        for (int x = 0; x < GridManager.width; x++)
        {
            for (int y = 1; y < 2; y++)
            {
                if (gridManager.gridCells[x, y].objectInCell == null)
                {
                    continue;
                }
                SummonStats cell = gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>();
                if (cell.power <= amount)
                {
                    if (y == 1)
                    {
                        discardManager.AddToDiscard(gridManager.gridCells[x, y].objectInCell.GetComponent<Summon>());
                    }
                }
            }
        }
    }
}