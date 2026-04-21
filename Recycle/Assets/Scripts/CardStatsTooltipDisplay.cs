using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using TMPro;

public class CardStatsTooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI element;
    public TextMeshProUGUI rank;
    public TextMeshProUGUI power;
    public TextMeshProUGUI guard;
    public TextMeshProUGUI cardText;
    public int cardType;

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

    public void SetStatsText(CardStats stats)
    {
        if (stats.cardType == 1)
        {
            cardType = 1;
            nameText.text = $"{stats.cardName}";
            element.text = string.Join(", ", stats.element);
            rank.text = stats.rank.ToString();
            power.text = stats.power.ToString();
            guard.text = stats.guard.ToString();
            cardText.text = $"{stats.text}";
        } 
        else if (stats.cardType == 2)
        {
            cardType = 2;
            nameText.text = $"{stats.cardName}";
            element.text = string.Join(", ", stats.sorceryType);
            cardText.text = $"{stats.text}";
        } 
        else if (stats.cardType == 3)
        {
            cardType = 3;
            nameText.text = $"{stats.cardName}";
            element.text = string.Join(", ", stats.hexType);
            cardText.text = $"{stats.text}";
        }
    }
}
