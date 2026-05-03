using UnityEngine;

[CreateAssetMenu(menuName = "Effects/All Power Reduction")]
public class AllPowerReduce : SorceryEffect
{
    public int amount;
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        for (int x = 0; x < GridManager.width; x++)
        {
            gridManager.gridCells[x, 2].objectInCell.GetComponent<SummonStats>().power -= amount;
        }
    }
}
