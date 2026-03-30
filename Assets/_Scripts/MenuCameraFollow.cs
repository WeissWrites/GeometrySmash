using UnityEngine;

public class MenuCameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform menuPlayer;
    [Header("Horizontal Settings")]
    public float catchUpThresholdX;
    public float xOffset = 5f;
    [Header("Vertical Box Settings")]
    public float yHomePosition = 0.56f;
    public float yDeadZoneHeight = 2.0f;
    public float yOffset = 0.5f;
    private float currentCameraY;

    private bool isFollowing = false;
    private float initialZ;

    void Start()
    {
        initialZ = transform.position.z;
        currentCameraY = yHomePosition;
        transform.position = new Vector3(transform.position.x, yHomePosition + yOffset, initialZ);
    }

    void LateUpdate()
    {
        if (menuPlayer == null) return;
        if (!isFollowing && menuPlayer.position.x >= catchUpThresholdX)
        {
            isFollowing = true;
        }
        if (isFollowing)
        {
            float deltaY = menuPlayer.position.y - currentCameraY;

            if (deltaY > yDeadZoneHeight)
            {
                currentCameraY = menuPlayer.position.y - yDeadZoneHeight;
            }
            else if (deltaY < -yDeadZoneHeight)
            {
                currentCameraY = menuPlayer.position.y + yDeadZoneHeight;
            }
            if (currentCameraY < yHomePosition)
            {
                currentCameraY = yHomePosition;
            }
            transform.position = new Vector3(menuPlayer.position.x + xOffset, currentCameraY + yOffset, initialZ);
        }
    }
    public void ResetCamera()
    {
        transform.position = new Vector3(-7.22f, currentCameraY, initialZ);
    }

    private void OnDrawGizmos()
    {
        if (Camera.current == null) return;

        Gizmos.color = Color.cyan;
        float boxCenterY = Application.isPlaying ? currentCameraY : yHomePosition;
        Vector3 center = new Vector3(transform.position.x, boxCenterY, 0);
        Gizmos.DrawWireCube(center, new Vector3(20, yDeadZoneHeight * 2, 0.1f));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x - 10, yHomePosition, 0),
                        new Vector3(transform.position.x + 10, yHomePosition, 0));
    }
}