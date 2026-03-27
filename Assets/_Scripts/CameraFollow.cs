using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Horizontal Settings")]
    public float xOffset = 2.0f;

    [Header("Vertical Box Settings")]
    public float yHomePosition = 0.56f; // The absolute lowest the camera center can go
    public float yDeadZoneHeight = 2.0f; // Half-height of the box

    private float currentCameraY;

    void Start()
    {
        // Initialize the camera at your preferred starting height
        currentCameraY = yHomePosition;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 1. HORIZONTAL: Stay locked to Player X + Offset
        float targetX = player.position.x + xOffset;

        // 2. VERTICAL: "Floating Window" Logic
        float deltaY = player.position.y - currentCameraY;

        // If player pushes ABOVE the top edge
        if (deltaY > yDeadZoneHeight)
        {
            currentCameraY = player.position.y - yDeadZoneHeight;
        }
        // If player pushes BELOW the bottom edge
        else if (deltaY < -yDeadZoneHeight)
        {
            currentCameraY = player.position.y + yDeadZoneHeight;
        }

        // 3. THE "FLOOR" LIMIT: Prevent camera from going below your Home Position
        if (currentCameraY < yHomePosition)
        {
            currentCameraY = yHomePosition;
        }

        // 4. APPLY POSITION
        transform.position = new Vector3(targetX, currentCameraY, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        // Only draw if we are actually looking at the scene to prevent Frustum errors
        if (Camera.current == null) return;

        Gizmos.color = Color.cyan;
        float boxCenterY = Application.isPlaying ? currentCameraY : yHomePosition;
        Vector3 center = new Vector3(transform.position.x, boxCenterY, 0);

        // Draw the limits
        Gizmos.DrawWireCube(center, new Vector3(10, yDeadZoneHeight * 2, 0.1f));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - 5, yHomePosition, 0),
                        new Vector3(transform.position.x + 5, yHomePosition, 0));
    }
}