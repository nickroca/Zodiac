using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Destroy Lowest")]
public class DestroyLowest : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
    }

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        int lowest = 50;
        Vector2 test;
        for (int i = 0; i <= 4; i++)
        {
            test = new Vector2(i, 2);
            if (gridManager.IsCellFull(test))
            {
                if (gridManager.gridCells[i, 2].objectInCell.GetComponent<SummonStats>().power < lowest)
                {
                    lowest = gridManager.gridCells[i, 2].objectInCell.GetComponent<SummonStats>().power;
                    target = gridManager.gridCells[i, 2];
                }
            }
        }
        if (target != null) {
            gridManager.RemoveObjectFromGrid(target.gridIndex);
        }
    }
}