using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip backgroundMusic;
    public AudioSource musicSource;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.clip = backgroundMusic;
        musicSource.volume = 0.1f;
        musicSource.Play();
    }

    public void PlayJumpSound()
    {
        source.PlayOneShot(jumpSound);
    }

    public void PlayCoinSound()
    {
        source.PlayOneShot(coinSound);
    }
}

