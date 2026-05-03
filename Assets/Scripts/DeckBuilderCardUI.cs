using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zodiac;

public class DeckBuilderCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image cardImage;
    public int cardID;

    private Transform originalParent;
    private DeckBuilderManager manager;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    DeckDropZone currentZone;

    public void Init(int id, DeckDropZone zone)
    {
        cardID = id;
        currentZone = zone;

        Card data = GameManager.Instance.GetCardByID(id);
        cardImage.sprite = data.sprite;
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);

        if (canvasGroup != null) {
            canvasGroup.blocksRaycasts = false;
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
}
