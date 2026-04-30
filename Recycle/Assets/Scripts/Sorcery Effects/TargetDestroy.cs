using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Targeted Destruction")]
public class TargetDestroy : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = true;
    }

    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        if (target == null || target.objectInCell == null)
        {
            return;
        }

        gridManager.RemoveObjectFromGrid(target.gridIndex);
    }
}
