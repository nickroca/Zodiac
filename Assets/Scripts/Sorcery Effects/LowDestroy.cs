using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Low Destroy")]
public class LowDestroy : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = true;
    }

    public int amount;

    DiscardManager discardManager = FindObjectOfType<DiscardManager>();

    public override void Activate(GridManager gridManager, GridCell target)
    {
        if (target == null || target.objectInCell == null)
        {
            return;
        }

        SummonStats cell = target.objectInCell.GetComponent<SummonStats>();

        if (cell.power < amount || cell.guard < amount)
        {
            if (target.gridIndex.y == 1) {
                gridManager.RemoveObjectFromGrid(target.gridIndex, true);
            } else
            {
                gridManager.RemoveObjectFromGrid(target.gridIndex, false);
            }
        }
        else
        {
            return;
        }
    }
}