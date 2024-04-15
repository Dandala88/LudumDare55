using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool paused;
    public const float SfxVolumeScale = 3f;

    public static bool priest = true;
    public static bool wraith;
    public static bool genie;
    public static bool angel;

    public enum SummonType
    {
        Priest,
        Wraith,
        Genie,
        Angel,
    }

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

    public static void SetSummonFlag(SummonType summonType)
    {
        switch(summonType)
        {
            case SummonType.Priest:
                priest = true;
                break;
            case SummonType.Wraith:
                wraith = true;
                break;
            case SummonType.Genie:
                genie = true;
                break;
            case SummonType.Angel:
                angel = true;
                break;
        }
    }

    public static bool GetSummonFlag(SummonType summonType)
    {
        switch (summonType)
        {
            case SummonType.Wraith:
                return wraith;
            case SummonType.Genie:
                return genie;
            case SummonType.Angel:
                return angel;
        }
        return priest;
    }
}
