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
    GameManager gameManager;

    void Start()
    {
        currentHP = 40;
        maxHP = 40;
        staticHP = 40;
        healthBar.color = Color.green;
        LIFEText.text = $"40";
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (currentHP < 0)
        {
            currentHP = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        if (currentHP != staticHP)
        {
            float newWidth = (float)currentHP / maxHP;
            float right = Mathf.Lerp(256f, 5f, newWidth);
            bar.offsetMax = new Vector2(-right, bar.offsetMax.y);
            staticHP = currentHP;
        }
        LIFEText.text = $"{currentHP}";
    }
}