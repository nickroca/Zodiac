using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Zodiac;


public class CardTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardStatsTooltipDisplay tooltip;
    public float fadeTime = 0.1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.SetStatsText(GetComponent<CardStats>());
            StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        }
        else
        {
            tooltip = FindAnyObjectByType<CardStatsTooltipDisplay>();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            StartCoroutine(Utility.FadeOut(tooltip.canvasGroup, 0f, fadeTime));
        }
    }
}
