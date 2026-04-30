using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionObject : MonoBehaviour
{
    public RectTransform objectToPosition;
    public int widthDivider = 2;
    public int heightDivider = 2;
    public float widthMultiplier = 1f;
    public float heightMultiplier = 1f;
    public bool updatePosition = false;
    
    // Start is called before the first frame update
    void Start()
    {
        SetUIObjectPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(updatePosition)
        {
            SetUIObjectPosition();
        }
    }

    public void SetUIObjectPosition()
    {
        if(objectToPosition != null && widthDivider != 0 && heightDivider != 0)
        {
            float anchorX = widthMultiplier / widthDivider;
            float anchorY = heightMultiplier / heightDivider;

            objectToPosition.anchorMin = new Vector2(anchorX, anchorY);
            objectToPosition.anchorMax = new Vector2(anchorX, anchorY);
            objectToPosition.pivot = new Vector2(0.5f, 0.5f);

            objectToPosition.anchoredPosition = Vector2.zero;
        }
    }
}
