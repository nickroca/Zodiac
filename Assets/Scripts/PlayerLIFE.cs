using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Zodiac;

public class PlayerLIFE : MonoBehaviour
{
    public int staticHP;
    public int maxHP; //not actually maxHP, just to make it so the bar doesn't go past its max width
    public int currentHP;
    public TextMeshProUGUI LIFEText;
    public Image healthBar;
    [SerializeField] private RectTransform bar;

    void Start()
    {
        currentHP = 40;
        maxHP = 40;
        staticHP = 40;
        healthBar.color = Color.green;
        LIFEText.text = $"40";
    }

    void Update()
    {
        if (currentHP != staticHP)
        {
            if (currentHP < maxHP)
            {
                float newWidth = (currentHP / maxHP);
                Debug.Log($"{newWidth}");
                bar.anchorMax = new Vector2(newWidth, 0.5f);
            } 
            else
            {
                bar.anchorMax = new Vector2(1f, 0.5f);
            }
        }
        staticHP = currentHP;
        LIFEText.text = $"{currentHP}";
    }
}
