using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zodiac;
using Unity.VisualScripting;

public class CardDisplay : MonoBehaviour
{
    //All Card Types
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public Image cardSprite;
    public GameObject summonElements;
    public GameObject sorceryElements;
    public GameObject hexElements;
    public GameObject prefab;

    //Summon Cards
    public TMP_Text rankText;
    public TMP_Text powerText;
    public TMP_Text guardText;
    public Image[] element;

    //Sorcery Cards

    //Hex Cards




    public void UpdateCardDisplay()
    {
        //All Card Changes
        nameText.text = cardData.cardName;
        cardSprite.sprite = cardData.sprite;
        if (cardData.prefab != null)
        {
            prefab = cardData.prefab;
        }

        //Specific Card Changes
        if (cardData is Summon summonCard)
        {
            UpdateDisplaySummonCard(summonCard);
        } 
        else if (cardData is Sorcery sorceryCard)
        {
            UpdateDisplaySorceryCard(sorceryCard);
        } 
        else if (cardData is Hex hexCard)
        {
            UpdateDisplayHexCard(hexCard);
        }
    }

    private void Update()
    {
        UpdateCardDisplay();
    }


    private void UpdateDisplaySummonCard(Summon summonCard)
    {
        sorceryElements.SetActive(false);
        hexElements.SetActive(false);
        summonElements.SetActive(true);
        for (int i = 0; i < element.Length; i++)
        {
            if (((int)summonCard.element) == i)
            {
                element[i].gameObject.SetActive(true);
            }
            else
            {
                element[i].gameObject.SetActive(false);
            }
        }

        rankText.text = summonCard.rank.ToString();
        powerText.text = summonCard.power.ToString();
        guardText.text = summonCard.guard.ToString();
    }

    private void UpdateDisplaySorceryCard(Sorcery sorceryCard)
    {
        sorceryElements.SetActive(true);
        hexElements.SetActive(false);
        summonElements.SetActive(false);
        for (int i = 0; i < element.Length; i++)
        {
            element[i].gameObject.SetActive(false);
        }
    }

    private void UpdateDisplayHexCard(Hex hexCard)
    {
        sorceryElements.SetActive(false);
        hexElements.SetActive(true);
        summonElements.SetActive(false);
        for (int i = 0; i < element.Length; i++)
        {
            element[i].gameObject.SetActive(false);
        }
    }
    /*
     public TMP_Text rankText;
     public TMP_Text powerText;
     public TMP_Text guardText;
     public Image[] element;
     
     */

}
