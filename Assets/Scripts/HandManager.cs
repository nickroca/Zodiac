using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zodiac;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab; //Assigned in inspector
    public Transform handTransform; //Root of the hand position
    public float fanSpread = 4f;
    public float cardSpacing = -170f;
    public float verticalSpacing = 25f;
    public List<GameObject> cardsInHand = new List<GameObject>(); //Hold a list of the card objects in hand

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddCardToHand(Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        //set the CardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.GetComponent<CardDisplay>().UpdateCardDisplay();

        UpdateHandVisuals();
    }

    void Update()
    {
        UpdateHandVisuals();

    }

    public void BattleSetup(int setMaxHandSize)
    {
        
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        if (cardCount == 3)
        {
            verticalSpacing = 6f;
        } 
        else if (cardCount == 4)
        {
            verticalSpacing = 14f;
        }
        else if (cardCount == 5)
        {
            verticalSpacing = 25f;
        }
        else if (cardCount == 6)
        {
            verticalSpacing = 40f;
        }

            for (int i = 0; i < cardCount; i++)
            {
                float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
                cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

                float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

                float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //normalize card position between -1 and 1

                float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

                //set card positions
                cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
            }
    }
}
