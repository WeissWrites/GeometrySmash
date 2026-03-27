using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clip")]
    public AudioClip levelBackgroundMusic;
    public AudioClip death;
    public AudioClip smashCoin;

    void Start()
    {

    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = levelBackgroundMusic;
        musicSource.Stop();
        musicSource.time = 0;
        musicSource.Play();
    }


}
