using NPOI.SS.Formula.Functions;
using UnityEngine;
using Zodiac;

[CreateAssetMenu(menuName = "Effects/Element Power Boost")]
public class ElementPowerBoost : SorceryEffect {

    public Summon.CardElement element;
    public int powerAmount;
    public int guardAmount;
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        for (int x = 0; x < GridManager.width; x++)
        {
            for (int y = 1; y < 2; y++)
            {
                Vector2 spot = new Vector2 (x, y);
                if (!gridManager.IsCellFull(spot))
                {
                    continue;
                }
                if (gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().element == this.element)
                {
                    gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().power += powerAmount;
                    gridManager.gridCells[x, y].objectInCell.GetComponent<SummonStats>().power += guardAmount;
                }
            }
        }
    }
}