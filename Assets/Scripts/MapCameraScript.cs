using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 dragOrigin;

    public float minY = 230f;
    public float maxY = 1170f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)
            );
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)
            );

            Vector3 difference = dragOrigin - currentPos;

            //Only move Y
            transform.position += new Vector3(0f, difference.y, 0f);
        }

        // Clamp + lock position
        Vector3 pos = transform.position;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = 500f;   // Lock X

        transform.position = pos;
    }
}