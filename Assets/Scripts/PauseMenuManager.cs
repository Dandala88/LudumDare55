using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menu;

    public void PauseGame()
    {
        if (GameManager.paused)
        {
            ResumeGame();
        }
        else
        {
            GameManager.paused = true;
            Time.timeScale = 0;
            menu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        GameManager.paused = false;
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
