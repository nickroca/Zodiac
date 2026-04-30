using UnityEngine;
using Zodiac;

[CreateAssetMenu(menuName = "Effects/Elemental Power Boost")]
public class ElementPowerBoost : SorceryEffect
{
    public int amount;
    public Summon.CardElement element;
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        if (target == null || target.objectInCell == null)
        {
            return;
        }

        SummonStats stats = target.objectInCell.GetComponent<SummonStats>();

        if (stats.element == element)
        {
            stats.power += amount;
        }
    }
}
