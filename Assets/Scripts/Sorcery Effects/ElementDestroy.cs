using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Elemental Destruction")]
public class ElementDestroy : SorceryEffect
{
    public Summon.CardElement element;
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        for(int x = 0; x < GridManager.width; x++)
        {
            for (int y = 1; y < 2; y++)
            {
                GridCell cell = gridManager.gridCells[x, y];
                if (!cell.cellFull) continue;
                SummonStats stats = cell.objectInCell.GetComponent<SummonStats>();
                if (stats != null && stats.element == element)
                {
                    gridManager.RemoveObjectFromGrid(cell.gridIndex);
                }
            }
        }
    }
}
