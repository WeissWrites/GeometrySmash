using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float xOffset = 2.0f; // Distance from the player on X
    public float yLockedPosition = 0.56f; // The Y level you want the camera to stay at

    void LateUpdate()
    {
        if (player != null)
        {
            // Set camera position: Player's X + Offset, Fixed Y, and Camera's original Z
            transform.position = new Vector3(player.position.x + xOffset, yLockedPosition, transform.position.z);
        }
    }
}
