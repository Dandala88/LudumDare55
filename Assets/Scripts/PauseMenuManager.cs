using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject transition;
    public TMP_Text acquiredText;

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

    public void LoadTransition()
    {
        if (CreditsManager.newGames > 0)
            acquiredText.text = "Summon's getting pretty good!";
        GameManager.paused = true;
        Time.timeScale = 0;
        transition.SetActive(true);
    }

    public void TransitionContinue()
    {
        GameManager.paused = false;
        transition.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
