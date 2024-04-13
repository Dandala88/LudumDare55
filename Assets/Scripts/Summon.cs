using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    private void OnDestroy()
    {
        WaveManager.currentWaveSummons.Remove(this);
    }
}
