using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManagerUI : MonoBehaviour
{
    [Header("UI References")]
    public Image fillImage;

    [Header("Track Settings")]
    public Transform player;
    public Transform startPoint;
    public Transform endPoint;

    void Update()
    {
        if (player == null || startPoint == null || endPoint == null || fillImage == null)
            return;

        float totalDistance = endPoint.position.x - startPoint.position.x;
        float currentDistance = player.position.x - startPoint.position.x;
        float progress = Mathf.Clamp01(currentDistance / totalDistance);

        fillImage.fillAmount = progress;
    }
}