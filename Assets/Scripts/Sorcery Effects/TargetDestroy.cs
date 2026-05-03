using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Targeted Destruction")]
public class TargetDestroy : SorceryEffect
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

        if (target.gridIndex.y == 1)
        {
            gridManager.RemoveObjectFromGrid(target.gridIndex, true);
        }
        else
        {
            gridManager.RemoveObjectFromGrid(target.gridIndex, false);
        }
    }
}