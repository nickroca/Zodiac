using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zodiac;
using UnityEngine.EventSystems;

public class SummonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SummonStatsTooltipDisplay tooltip;
    public float fadeTime = 0.1f;

    void Awake()
    {
        tooltip = FindObjectOfType<SummonStatsTooltipDisplay>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.SetStatsText(GetComponent<SummonStats>());
            StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        }
        else
        {
            tooltip = FindObjectOfType<SummonStatsTooltipDisplay>();
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
