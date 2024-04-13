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

    public static int GetWaveMaxSummon(int index)
    {
        if (index <= 0)
            return 0;
        else if (index == 1)
            return 1;

        int a = 0;
        int b = 1;
        int result = 0;

        for (int i = 2; i <= index; i++)
        {
            result = a + b;
            a = b;
            b = result;
        }

        return result;
    }
}
