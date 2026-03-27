using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class EndLevelSequence : MonoBehaviour
{
    public PostProcessVolume volume;
    public AudioSource musicSource;
    public GameObject endScreenUI; // Drag your UI Panel here

    private DepthOfField dof;

    void Start()
    {
        volume.profile.TryGetSettings(out dof);
        endScreenUI.SetActive(false);
    }

    public void StartFinishSequence(Transform playerTransform)
    {
        StartCoroutine(ExecuteEndSequence(playerTransform));
    }

    IEnumerator ExecuteEndSequence(Transform player)
    {
        // 0. Disable your normal Camera Follow script here
        // Replace 'CameraFollow' with the actual name of your follow script
        if (GetComponent<CameraFollow>() != null)
        {
            GetComponent<CameraFollow>().enabled = false;
        }

        float duration = 2.0f;
        float elapsed = 0;
        float startMusicVolume = musicSource.volume;
        float startSize = Camera.main.orthographicSize;
        float targetSize = 4f; // Set a specific zoom-in value (lower is closer)
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;

            // Smooth out the movement using a curve
            float curve = Mathf.SmoothStep(0, 1, percent);

            // 1. Zoom Camera
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, curve);

            // 2. Center Camera exactly on Player (keep Z at -10)
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, -10f);
            transform.position = Vector3.Lerp(startPos, targetPos, curve);

            // 3. Blur and Music logic...
            if (dof != null) dof.focusDistance.value = Mathf.Lerp(5f, 0.1f, curve);
            musicSource.volume = Mathf.Lerp(startMusicVolume, 0, curve);

            yield return null;
        }
        endScreenUI.SetActive(true);
        Time.timeScale = 0;
    }

}