using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Horizontal Settings")]
    public float xOffset = 2.0f;

    [Header("Vertical Box Settings")]
    public float yHomePosition = 0.56f;
    public float yDeadZoneHeight = 2.0f;

    private float currentCameraY;

    void Start()
    {
        currentCameraY = yHomePosition;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float targetX = player.position.x + xOffset;
        float deltaY = player.position.y - currentCameraY;
        if (deltaY > yDeadZoneHeight)
        {
            currentCameraY = player.position.y - yDeadZoneHeight;
        }
        else if (deltaY < -yDeadZoneHeight)
        {
            currentCameraY = player.position.y + yDeadZoneHeight;
        }
        if (currentCameraY < yHomePosition)
        {
            currentCameraY = yHomePosition;
        }
        transform.position = new Vector3(targetX, currentCameraY, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        if (Camera.current == null) return;

        Gizmos.color = Color.cyan;
        float boxCenterY = Application.isPlaying ? currentCameraY : yHomePosition;
        Vector3 center = new Vector3(transform.position.x, boxCenterY, 0);
        Gizmos.DrawWireCube(center, new Vector3(10, yDeadZoneHeight * 2, 0.1f));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - 5, yHomePosition, 0),
                        new Vector3(transform.position.x + 5, yHomePosition, 0));
    }
}