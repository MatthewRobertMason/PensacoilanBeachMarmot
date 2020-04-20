using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Exit()
    {
        Application.Quit(0);
    }

    public void StartEasy()
    {
        if(audioManager) audioManager.PlayGameMusic();
        SceneManager.LoadScene("Appartment");
    }

    public void ReturnToMenu()
    {
        if(audioManager) audioManager.PlayTitleMusic();
        SceneManager.LoadScene("Start");
    }
}
