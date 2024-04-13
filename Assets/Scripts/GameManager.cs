using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool paused;

    [ContextMenu("Summon Got It")]
    public void SummonGotIt()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
