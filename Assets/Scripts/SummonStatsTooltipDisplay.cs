using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using TMPro;

public class SummonStatsTooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI element;
    public TextMeshProUGUI rank;
    public TextMeshProUGUI power;
    public TextMeshProUGUI guard;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI position;

    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private float xOffset = 200f;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetStatsText(SummonStats stats)
    {
        nameText.text = $"{stats.cardName}";
        element.text = string.Join(", ", stats.element);
        rank.text = stats.rank.ToString();
        power.text = stats.power.ToString();
        guard.text = stats.guard.ToString();
        cardText.text = $"{stats.text}";
        if (stats.attackPosition)
        {
            position.text = "Attack";
        }
        else
        {
            position.text = "Defense";
        }
    }
}
