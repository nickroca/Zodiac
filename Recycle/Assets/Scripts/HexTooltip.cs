using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zodiac;
using System;

public class HexTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public HexStatsTooltipDisplay tooltip;
    public float fadeTime = 0.1f;

    void Awake()
    {
        tooltip = FindObjectOfType<HexStatsTooltipDisplay>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.SetStatsText(GetComponent<HexStats>());
            StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        }
        else
        {
            tooltip = FindObjectOfType<HexStatsTooltipDisplay>();
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
