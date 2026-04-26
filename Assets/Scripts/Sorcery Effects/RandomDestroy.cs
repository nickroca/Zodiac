using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Random Destruction")]
public class RandomDestroy : SorceryEffect
{
    private void OnEnable()
    {
        requiresTarget = false;
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