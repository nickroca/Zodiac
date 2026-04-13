using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zodiac;

public class SorceryTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SorceryStatsTooltipDisplay tooltip;
    public float fadeTime = 0.1f;

    void Awake()
    {
        tooltip = FindAnyObjectByType<SorceryStatsTooltipDisplay>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.SetStatsText(GetComponent<SorceryStats>());
            StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        }
        else
        {
            tooltip = FindAnyObjectByType<SorceryStatsTooltipDisplay>();
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
