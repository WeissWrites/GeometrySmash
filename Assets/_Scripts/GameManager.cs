using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] DeathCountUI deathCountUI;
    public GameObject pauseEndScreenUI;
    public Vector2 currentSpawn;
    public static GameManager instance;

    public GameObject[] worldCoinObjects;
    public GameObject[] uiCoinImages;
    private bool[] collectedCoins = new bool[3];
    public bool isLevelCompleted = false;

    public void Start()
    {
        currentSpawn = GameObject.FindWithTag("Player").transform.position;
        audioManager.PlayBackgroundMusic();
        pauseEndScreenUI.SetActive(false);
        foreach (GameObject coin in uiCoinImages)
        {
            coin.SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isLevelCompleted)
        {
            PauseMenu();
        }
    }
    public void SetNewSpawn(Vector2 pos)
    {
        currentSpawn = pos;
    }

    public void Restart()
    {
        isLevelCompleted = false;
        Time.timeScale = 1f;
        pauseEndScreenUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GameObject player = GameObject.FindWithTag("Player");

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        player.transform.position = currentSpawn;
        if (player.transform.childCount > 0)
        {
            player.transform.GetChild(0).rotation = Quaternion.identity;
        }
        deathCountUI.IncreaseDeathCounter();
        audioManager.PlayBackgroundMusic();
        for (int i = 0; i < collectedCoins.Length; i++)
        {
            collectedCoins[i] = false;
        }

        foreach (GameObject coin in worldCoinObjects)
        {
            if (coin != null)
            {
                coin.SetActive(true);
                if (coin.TryGetComponent(out Collider2D col))
                {
                    col.enabled = true;
                }
            }
        }
        UpdateCoinUI();
    }

    public void PauseMenu()
    {
        UpdateCoinUI();
        bool isActive = pauseEndScreenUI.activeSelf;
        pauseEndScreenUI.SetActive(!isActive);

        if (!isActive)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudioManager.instance.musicSource.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            AudioManager.instance.musicSource.Play();
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.instance.musicSource.Stop();
        SceneManager.LoadScene(0);
    }

    public void CollectCoin(int index)
    {
        if (index >= 0 && index < collectedCoins.Length)
        {
            collectedCoins[index] = true;
            UpdateCoinUI();
        }
    }

    public void UpdateCoinUI()
    {
        for (int i = 0; i < collectedCoins.Length; i++)
        {
            uiCoinImages[i].SetActive(collectedCoins[i]);
        }
    }
}
