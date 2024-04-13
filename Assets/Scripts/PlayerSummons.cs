using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummons : MonoBehaviour
{
    private List<Player> summons = new List<Player>();
    private int currentSummonIndex;

    private void Awake()
    {
        var summonChildren = GetComponentsInChildren<Player>();
        foreach (var child in summonChildren)
        {
            summons.Add(child);
            child.gameObject.SetActive(false);
        }

        summons[0].gameObject.SetActive(true);
    }

    public void Summon(int value)
    {
        var lastSummon = summons[currentSummonIndex];
        WrapSummonIndex(value);
        while (!summons[currentSummonIndex].acquired)
        {
            WrapSummonIndex(value);
        }

        var selectedSummon = summons[currentSummonIndex];
        selectedSummon.transform.position = lastSummon.transform.position;
        selectedSummon.movement = lastSummon.movement;
        selectedSummon.gameObject.SetActive(true);
        lastSummon.gameObject.SetActive(false);
    }

    private void WrapSummonIndex(int value)
    {
        if (value > 0 && currentSummonIndex == summons.Count - 1)
            currentSummonIndex = 0;
        else if (value < 0 && currentSummonIndex == 0)
            currentSummonIndex = summons.Count - 1;
        else
            currentSummonIndex += value;
    }

}
