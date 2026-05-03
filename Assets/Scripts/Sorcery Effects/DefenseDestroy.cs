using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Defense Destroy")]
public class DefenseDestroy : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = true;
    }
    public override void Activate(GridManager gridManager, GridCell target)
    {
        if (target == null || target.objectInCell == null)
        {
            return;
        }

        SummonStats cell = target.objectInCell.GetComponent<SummonStats>();

        if (target.gridIndex.y == 1)
        {
            return;
        }
        else if (!cell.attackPosition)
        {
            gridManager.RemoveObjectFromGrid(target.gridIndex, false);
        }
        else
        {
            return;
        }
    }
}