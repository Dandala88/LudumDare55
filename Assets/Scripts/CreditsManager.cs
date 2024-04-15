using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public static int newGames;
    public void NewGamePlus()
    {
        newGames++;
        Debug.Log(newGames);
        SceneManager.LoadSceneAsync(1);
    }
}
