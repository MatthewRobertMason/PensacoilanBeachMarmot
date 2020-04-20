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
        Settings.TotalDays = 10;
        Settings.TotalGrowth = 75;
        Settings.Hints = 3;
        SceneManager.LoadScene("Appartment");
    }

    public void StartMedium()
    {
        if (audioManager) audioManager.PlayGameMusic();
        Settings.TotalDays = 10;
        Settings.TotalGrowth = 100;
        Settings.Hints = 2;
        SceneManager.LoadScene("Appartment");
    }

    public void StartHard()
    {
        if (audioManager) audioManager.PlayGameMusic();
        Settings.TotalDays = 12;
        Settings.TotalGrowth = 125;
        Settings.Hints = 1;
        SceneManager.LoadScene("Appartment");
    }

    public void ReturnToMenu()
    {
        if(audioManager) audioManager.PlayTitleMusic();
        SceneManager.LoadScene("Start");
    }
}
