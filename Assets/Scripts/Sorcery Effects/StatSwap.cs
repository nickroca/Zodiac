using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Stat Swap")]
public class StatSwap : SorceryEffect
{
    public override void Activate(GridManager gridManager, GridCell target)
    {
        for (int x = 0; x < GridManager.width; x++)
        {
            for (int y = 1; y < 2; y++)
            {
                Vector2 spot = new Vector2(x, y);
                if (gridManager.IsCellFull(spot))
                {
                    int storage = gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().power;
                    gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().power = gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().guard;
                    gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().guard = storage;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
