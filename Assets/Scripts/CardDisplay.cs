using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zodiac;
using Unity.VisualScripting;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text rankText;
    public TMP_Text powerText;
    public TMP_Text guardText;
    public Image[] element;
    public Image cardSprite;

    public void UpdateCardDisplay()
    {
        for (int i = 0; i < element.Length; i++)
        {
            if (((int)cardData.element) == i)
            {
                element[i].gameObject.SetActive(true);
            }
            else
            {
                element[i].gameObject.SetActive(false);
            }
        }

        nameText.text = cardData.cardName;
        rankText.text = cardData.rank.ToString();
        powerText.text = cardData.power.ToString();
        guardText.text = cardData.guard.ToString();
        cardSprite.sprite = cardData.sprite;
    }
}
