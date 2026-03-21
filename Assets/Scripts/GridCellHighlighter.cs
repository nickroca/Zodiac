using UnityEngine;
using Zodiac;

[RequireComponent(typeof(SpriteRenderer))]
public class GridCellHighlighter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color highlightColor = Color.cyan;
    public Color positiveColor = Color.yellow;
    public Color negativeColor = Color.red;
    private Color originalColor;
    public GridCell gridCell;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        gridCell = GetComponent<GridCell>();
    }

    void OnMouseEnter()
    {
        if (!GameManager.Instance.PlayingCard) {
            spriteRenderer.color = highlightColor;
        }
        else if (gridCell.cellFull || gridCell.gridIndex.y != 1)
        {
            spriteRenderer.color = negativeColor;
        } 
        else
        {
            spriteRenderer.color = positiveColor;
        }
    }

    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }
}
