using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clip")]
    public AudioClip levelBackgroundMusic;
    public AudioClip death;
    public AudioClip smashCoin;

    public static AudioManager instance;
    void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = levelBackgroundMusic;
        musicSource.Stop();
        musicSource.Play();
    }


}
