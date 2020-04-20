using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip titleMusic;
    public AudioClip gameMusic;
    public AudioClip goodEndMusic;
    public AudioClip groodEndMusic;
    public AudioClip badEndMusic;

    public Slider volumeSlider;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            // One is the instance that there can only be
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(this.gameObject);
        audioSource = this.GetComponent<AudioSource>();

        if (audioSource.clip == null)
            audioSource.clip = titleMusic;

        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetVolume();
    }

    public void SetVolume()
    {
        if (volumeSlider != null)
            audioSource.volume = volumeSlider.value;
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
        else
            audioSource.UnPause();
    }

    public void PlayTitleMusic()
    {
        audioSource.clip = titleMusic;
        audioSource.Play();
    }

    public void PlayGameMusic()
    {
        audioSource.clip = gameMusic;
        audioSource.Play();
    }

    public void PlayGoodEndMusic()
    {
        audioSource.clip = goodEndMusic;
        audioSource.Play();
    }

    public void PlayGroodEndMusic()
    {
        audioSource.clip = groodEndMusic;
        audioSource.Play();
    }

    public void PlayBadEndMusic()
    {
        audioSource.clip = badEndMusic;
        audioSource.Play();
    }
}
