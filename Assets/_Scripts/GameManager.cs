using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public DeathCountUI deathCountUI;
    private Vector2 currentSpawn;

    public void Start()
    {
        currentSpawn = GameObject.FindWithTag("Player").transform.position;
        audioManager.PlayBackgroundMusic();
    }

    public void Update()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastLevel", currentLevelName);
        PlayerPrefs.Save();
    }
    public void SetNewSpawn(Vector2 pos)
    {
        currentSpawn = pos;
    }

    public void Restart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = currentSpawn;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.transform.GetChild(0).rotation = Quaternion.identity;
        deathCountUI.IncreaseDeathCounter();
        audioManager.PlayBackgroundMusic();
    }
}
