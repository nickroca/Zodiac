using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerLIFE : MonoBehaviour
{
    public static int startHP;
    public static int staticHP;
    public int currentHP;
    public TextMeshProUGUI LIFEText;
    public Image healthBar;

    void Start()
    {
        startHP = 40;
        currentHP = 40;
        staticHP = 40;
        healthBar.color = Color.green;
        LIFEText.text = $"40";
    }

    void Update()
    {
        if (currentHP != staticHP)
        {
            int difference = Math.Abs(currentHP - staticHP);

            staticHP = currentHP;
            LIFEText.text = $"{staticHP}";
        }
    }
}
