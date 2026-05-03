using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zodiac;
using UnityEngine.EventSystems;
using System.ComponentModel;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;


public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private GridManager gridManager;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpTime = 0.3f;
    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private int playPositionYDivider = 1;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 1;
    [SerializeField] private float playPositionXMultiplier = 1f;
    private LayerMask gridLayerMask;
    private LayerMask summonLayerMask;
    private Card cardData;
    private CardDisplay cardDisplay;
    HandManager handManager;
    DiscardManager discardManager;
    TurnSystem turnSystem;
    PositionManager positionManager;
    public bool positionSelected = false;
    HandHolder handHolder;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        //updateCardPlayPosition();
        //updatePlayPosition();
        gridManager = FindObjectOfType<GridManager>();
        handManager = FindObjectOfType<HandManager>();
        discardManager = FindObjectOfType<DiscardManager>();
        cardDisplay = FindObjectOfType<CardDisplay>();
        turnSystem = FindObjectOfType<TurnSystem>();
        positionManager = FindObjectOfType<PositionManager>();
        handHolder = FindObjectOfType<HandHolder>();

        gridLayerMask = LayerMask.GetMask("Grid");
        summonLayerMask = LayerMask.GetMask("Summons");
        cardData = cardDisplay.cardData;
    }

    void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                if (turnSystem.isYourTurn && (turnSystem.phaseCount == 1 || turnSystem.phaseCount == 3))
                {
                    HandleDragState();
                    if (Input.GetMouseButtonDown(1))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, summonLayerMask);

                        if (hit.collider != null && hit.collider.TryGetComponent<GridCell>(out var cell))
                        {
                            GridCell cell2 = hit.collider.GetComponentInParent<GridCell>();
                            if (cell2 != null)
                            {
                                gridManager.ToggleSacrifice(cell);
                            }
                        }
                    }
                    if (!Input.GetMouseButton(0)) //check if mouse button is released
                    {
                        TransitionToState0();
                    }
                }
                break;
            case 3:
                if (turnSystem.isYourTurn && (turnSystem.phaseCount == 1 || turnSystem.phaseCount == 3))
                {
                    HandlePlayState();
                }
                break;
        }

        if (cardData != cardDisplay.cardData)
        {
            cardData = cardDisplay.cardData;
        }
    }

    private void TransitionToState0()
    {
        if (!gridManager.isTargeting)
        {
            currentState = 0;
            GameManager.Instance.PlayingCard = false;
            gridManager.sacrifices.Clear();

            //reset back to original
            rectTransform.localScale = originalScale;
            rectTransform.localRotation = originalRotation;
            rectTransform.localPosition = originalPosition;
            glowEffect.SetActive(false);
            playArrow.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1 && turnSystem.isYourTurn && (turnSystem.phaseCount == 1 || turnSystem.phaseCount == 3) && (turnSystem.summonLimit != 0 || !(cardData is Summon)))
        {
            currentState = 2;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (cardData is Summon summonCard)
            {
                HandHolder.Instance.ShowMessage("Summon");
            }
            else if (cardData is Sorcery sorceryCard)
            {
                HandHolder.Instance.ShowMessage("Sorcery");
            }
            else if (cardData is Hex hexCard)
            {
                HandHolder.Instance.ShowMessage("Hex");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            if (Input.mousePosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);
                rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpTime);
            }
        }
    }

    //Vector3.Lerp(originalPosition, localPointerPosition, lerpTime);


    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        transform.position = mousePos;
        
        //rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpTime); //this doesn't work for some reason
    }

    private void HandlePlayState()
    {
        if (!GameManager.Instance.PlayingCard)
        {
            GameManager.Instance.PlayingCard = true;
        }

        rectTransform.localPosition = playPosition;
        if (positionManager.attackPosition)
        {
            rectTransform.localRotation = Quaternion.identity;
        }
        else
        {
            rectTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayerMask);

            if (hit.collider != null && hit.collider.TryGetComponent<GridCell>(out var cell))
            {
                if (cell.gridIndex.y == 1)
                    gridManager.ToggleSacrifice(cell);
            }
        }

        if (!Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (cardData is Summon summonCard)
            {
                TryToPlaySummonCard(ray, summonCard);
            }
            else if (cardData is Sorcery sorceryCard)
            {
                TryToPlaySorceryCard(ray, sorceryCard);
            }
            else if (cardData is Hex hexCard)
            {
                TryToPlayHexCard(ray, hexCard);
            }
            TransitionToState0();
        }

        if (Input.mousePosition.y < 150)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }

    }

    private void TryToPlaySummonCard(Ray ray, Summon summonCard)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent<GridCell>(out var cell))
        {
            Vector2 targetPos = cell.gridIndex;

            if (cell.gridIndex.y == 1)
            {
                if (!gridManager.TrySacrifice(summonCard.rank))
                {
                    HandHolder.Instance.ShowMessage("Needs more sacrifices");
                    return;
                }

                GameObject placedObj = gridManager.AddObjectToGrid(summonCard.prefab, targetPos, true, positionManager.attackPosition);
                if (placedObj != null)
                {
                    CardInstance instance = placedObj.GetComponent<CardInstance>();
                    if (instance == null)
                    {
                        instance = placedObj.AddComponent<CardInstance>();
                    }
                    instance.cardData = summonCard;
                    instance.controller = 1;

                    summonCard.attackPosition = positionManager.attackPosition;
                    cell.objectInCell.GetComponent<SummonStats>().summonStartData = summonCard;
                    //cell.objectInCell.attackposition = positionManager.attackPosition;
                    handManager.cardsInHand.Remove(gameObject);
                    handManager.UpdateHandVisuals();
                    HandHolder.Instance.ShowMessage($"Played Summon: {summonCard.name}");
                    Destroy(gameObject);
                    turnSystem.summonLimit--;
                }
            }
        }
    }

    private void TryToPlaySorceryCard(Ray ray, Sorcery sorceryCard)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent<GridCell>(out var cell))
        {
            Vector2 targetPos = cell.gridIndex;

            if (cell.gridIndex.y != 0)
            {
                return;
            }

            GameObject placedObj = gridManager.AddObjectToGrid(sorceryCard.prefab, targetPos, true, true);
            
            if (placedObj != null)
            {
                CardInstance instance = placedObj.GetComponent<CardInstance>();
                if (instance == null)
                {
                    instance = placedObj.AddComponent<CardInstance>();
                }
                instance.cardData = sorceryCard;
                instance.controller = 1;
                
                HandHolder.Instance.ShowMessage($"Played Sorcery: {sorceryCard.name}");

                handManager.cardsInHand.Remove(gameObject);
                handManager.UpdateHandVisuals();
                Destroy(gameObject);

                if (sorceryCard.effect.requiresTarget)
                {
                    gridManager.StartTargeting(target => target.cellFull, selectedCell =>
                    {
                        sorceryCard.effect.Activate(gridManager, selectedCell);
                        ResolveSorcery(sorceryCard, placedObj, selectedCell, targetPos);
                    });
                }
                else
                {
                    sorceryCard.effect.Activate(gridManager, null);
                    ResolveSorcery(sorceryCard, placedObj, null, targetPos);
                }
            }
        }
    }

    private void ResolveSorcery(Sorcery sorceryCard, GameObject placedObj, GridCell target, Vector2 placePos)
    {
        HandHolder.Instance.ShowMessage($"Activated Sorcery: {sorceryCard.name}");

        

        if (sorceryCard.type == Sorcery.SorceryType.Normal)
        {
            gridManager.RemoveObjectFromGrid(placePos, true);
        }
        else if (sorceryCard.type == Sorcery.SorceryType.Permanent)
        {
            //not yet implemented
        }
        else if (sorceryCard.type == Sorcery.SorceryType.Artifact)
        {
            //not yet implemented
        }
    }

    private void TryToPlayHexCard(Ray ray, Hex hexCard)
    {
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent<GridCell>(out var cell))
        {
            Vector2 targetPos = cell.gridIndex;

            if (cell.gridIndex.y != 0)
            {
                HandHolder.Instance.ShowMessage("Hex must be placed in the bottom row.");
                return;
            }

            GameObject placedObj = gridManager.AddObjectToGrid(hexCard.prefab, targetPos, true, positionManager.attackPosition);
            if (placedObj != null)
            {
                CardInstance instance = placedObj.GetComponent<CardInstance>();
                if (instance == null)
                {
                    instance = placedObj.AddComponent<CardInstance>();
                }
                instance.cardData = hexCard;
                instance.controller = 1;

                HexHandler handler = placedObj.AddComponent<HexHandler>();
                handler.Init(hexCard, gridManager, targetPos);
                handManager.cardsInHand.Remove(gameObject);
                handManager.UpdateHandVisuals();
                HandHolder.Instance.ShowMessage($"Played Hex: {hexCard.name}");
                Destroy(gameObject);
            }
        }
    }
}