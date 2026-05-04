using UnityEngine;
using Zodiac;

[CreateAssetMenu(menuName = "Effects/Destroy All")]
public class DestroyAll : SorceryEffect
{
    public override void Activate(GridManager gridManager, GridCell target = null)
    {
        for (int x = 0; x < GridManager.width; x++)
            for (int y = 1; y < 2; y++)
        {
            Vector2 spot = new Vector2(x, y);
            if (!gridManager.IsCellFull(spot))
            {
                continue;
            } 
            else
            {
                if (gridManager.gridCells[x,y].objectInCell.GetComponent<CardInstance>().controller == 1)
                {
                    gridManager.RemoveObjectFromGrid(spot, true);
                } 
                else
                {
                    gridManager.RemoveObjectFromGrid(spot, false);
                }
            }
        }
    }
}