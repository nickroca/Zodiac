using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject Canvas;

    private bool isDragging = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private bool isOverDropZone;
    private bool isDraggable = true;


    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        dropZone = GameObject.Find("Player Summon Zone");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }


    public void StartDrag() 
    {
        if (!isDraggable) return;
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;

    }

    public void EndDrag()
    {
        if (!isDraggable) return;
        isDragging = false;
        if (isOverDropZone) {
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
        } else {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    } 

    // Update is called once per frame
    void Update()
    {
        if (isDragging) {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }
}
