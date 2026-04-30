using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Destroy Sorcery & Hex")]
public class DestroySorceriesAndHexes : SorceryEffect
{
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        for(int x = 0; x < GridManager.width; x++)
        {
            for (int y = 0; y <= 3; y = y + 3)
            {
                GridCell cell = gridManager.gridCells[x, y];
                if (!cell.cellFull) continue;
                if (cell.objectInCell.GetComponent<SummonStats>() == null)
                {
                    gridManager.RemoveObjectFromGrid(cell.gridIndex);
                }
            }
        }
    }
}
