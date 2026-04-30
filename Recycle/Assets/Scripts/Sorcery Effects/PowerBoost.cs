using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Power Boost")]
public class PowerBoost : SorceryEffect
{
    public int amount;
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        if (target == null || target.objectInCell == null)
        {
            return;
        }

        SummonStats stats = target.objectInCell.GetComponent<SummonStats>();
        stats.power += amount;
    }
}
