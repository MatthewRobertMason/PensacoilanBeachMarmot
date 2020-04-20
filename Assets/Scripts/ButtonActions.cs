using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit(0);
    }

    public void StartEasy()
    {
        SceneManager.LoadScene("Appartment");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
