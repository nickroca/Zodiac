using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public float startY = -525f;
    public float rowSpacing = 200f;
    public float moveSpeed = 5f;

    private float targetY;

    MapManager mapManager;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        UpdateCameraPositionInstant();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, targetY, Time.deltaTime * moveSpeed);
        transform.position = pos;
    }

    public void MoveToRow(int row)
    {
        targetY = startY + (row * rowSpacing);
    }

    public void UpdateCameraPositionInstant()
    {
        if (MapManagerHasNode(out MapNode node))
        {
            targetY = startY + (node.rowIndex * rowSpacing) + 875;

            Vector3 pos = transform.position;
            pos.y = targetY;
            transform.position = pos;
        }
    }

    public bool MapManagerHasNode(out MapNode node)
    {
        if (mapManager != null && mapManager.currentNode != null)
        {
            node = mapManager.currentNode;
            return true;
        }

        node = null;
        return false;
    }
}
