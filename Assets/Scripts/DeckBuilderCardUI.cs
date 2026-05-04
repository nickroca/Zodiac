using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zodiac;

public class DeckBuilderCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int cardID;

    private Transform originalParent;
    private DeckBuilderManager manager;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private ScrollRect parentScrollRect;

    DeckDropZone currentZone;

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

    //Sorcery/Hex Cards
    public GameObject effectBox;
    public TMP_Text effectText;

    private bool isDragging = false;

    public void Init(int id, DeckDropZone zone)
    {
        cardID = id;
        currentZone = zone;

        Card data = GameManager.Instance.GetCardByID(id);
    }

    void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 3)
        {
            enabled = false;
            return;
        }

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentScrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = false;
        }

        if (parentScrollRect != null)
        {
            parentScrollRect.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = true;
        }

        if (parentScrollRect != null)
        {
            parentScrollRect.enabled = true;
        }
        
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            rectTransform.localPosition = Vector3.zero;
        }
    }

    public DeckDropZone GetCurrentZone()
    {
        return currentZone;
    }

    public void SetZone(DeckDropZone zone)
    {
        currentZone = zone;
    }

    void ReturnToOriginal()
    {
        transform.SetParent(originalParent);
    }

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

    private void UpdateDisplaySummonCard(Summon summonCard)
    {
        sorceryElements.SetActive(false);
        hexElements.SetActive(false);
        summonElements.SetActive(true);
        effectBox.SetActive(false);

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
        effectBox.SetActive(true);
        effectText.text = sorceryCard.text;
        effectText.color = Color.black;
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
        effectBox.SetActive(true);
        effectText.text = hexCard.text;
        for (int i = 0; i < element.Length; i++)
        {
            element[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isDragging)
        {
            rectTransform.position = Input.mousePosition;

            if(Input.GetMouseButtonUp(1))
            {
                EndDrag();
            }

        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BeginDrag();
        }
    }

    void BeginDrag()
    {
        isDragging = true;

        originalParent = transform.parent;
        transform.SetParent(canvas.transform);

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    void EndDrag()
    {
        isDragging = false;

        if(canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        DeckDropZone foundZone = null;

        foreach (var r in results)
        {
            foundZone = r.gameObject.GetComponent<DeckDropZone>();
            if (foundZone != null)
                break;
        }

        if (foundZone != null)
        {
            currentZone.manager.MoveCard(cardID, currentZone.zoneType, foundZone.zoneType);
            SetZone(foundZone);
        }

        transform.SetParent(originalParent);
        rectTransform.localPosition = Vector3.zero;
    }
}