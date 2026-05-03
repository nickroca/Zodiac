using UnityEngine;
using Zodiac;

[CreateAssetMenu(menuName = "Effects/Destroy Lowest")]
public class DestroyLowest : SorceryEffect
{
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        int test = 50;
        Vector2 finalSpot = new Vector2();
        bool spotObtained = false;

        for (int x = 0; x < GridManager.width; x++)
        {
            Vector2 spot = new Vector2(x, 2);
            if (!gridManager.IsCellFull(spot))
            {
                continue;
            }

            if (gridManager.gridCells[x, 2].objectInCell.GetComponent<SummonStats>().power < test)
            {
                finalSpot = spot;
                spotObtained = true;
            }
        }

        if (spotObtained)
        {
            gridManager.RemoveObjectFromGrid(finalSpot, false);
        }
    }
}