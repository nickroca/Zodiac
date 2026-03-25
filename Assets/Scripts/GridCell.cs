using UnityEngine;
using Zodiac;

public class GridCell : MonoBehaviour
{
    public Vector2 gridIndex;
    public bool cellFull = false;
    public GameObject objectInCell;

    private GridManager gridmanager;

    void Start()
    {
        gridmanager = FindObjectOfType<GridManager>();
    }

    private void OnMouseDown()
    {
        if (objectInCell != null)
        {
            gridmanager.OnCellClicked(this);
        }
    }
}
