using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class EndLevelSequence : MonoBehaviour
{
    public PostProcessVolume volume;
    public AudioSource musicSource;
    private DepthOfField dof;

    void Start()
    {
        volume.profile.TryGetSettings(out dof);
    }

    public void StartFinishSequence(Transform playerTransform)
    {
        StartCoroutine(ExecuteEndSequence(playerTransform));
    }

    IEnumerator ExecuteEndSequence(Transform player)
    {
        if (GetComponent<CameraFollow>() != null)
        {
            GetComponent<CameraFollow>().enabled = false;
        }

        float duration = 2.0f;
        float elapsed = 0;
        float startMusicVolume = musicSource.volume;
        float startSize = Camera.main.orthographicSize;
        float targetSize = 4f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;

            float curve = Mathf.SmoothStep(0, 1, percent);

            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, curve);

            Vector3 targetPos = new Vector3(player.position.x, player.position.y, -10f);
            transform.position = Vector3.Lerp(startPos, targetPos, curve);

            if (dof != null) dof.focusDistance.value = Mathf.Lerp(5f, 0.1f, curve);
            musicSource.volume = Mathf.Lerp(startMusicVolume, 0, curve);

            yield return null;
        }
        GameManager.instance.isLevelCompleted = true;
        GameManager.instance.pauseEndScreenUI.SetActive(true);
        Time.timeScale = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}