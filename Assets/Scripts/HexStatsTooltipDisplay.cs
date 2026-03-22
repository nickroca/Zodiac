using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zodiac;

public class HexStatsTooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI type;
    public TextMeshProUGUI cardText;
    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private float xOffset = 200f;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetStatsText(HexStats stats)
    {
        nameText.text = $"{stats.cardName}";
        type.text = string.Join(", ", stats.type);
        cardText.text = $"{stats.text}";
    }
}
