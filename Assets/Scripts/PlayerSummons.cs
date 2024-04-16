using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSummons : MonoBehaviour
{
    public GameObject smoke;
    public AudioClip switchClip;

    public List<Player> summons = new List<Player>();
    private int currentSummonIndex;
    private AudioSource audioSource;

    public delegate void SummonChangeAction(Player newSummon);
    public static event SummonChangeAction OnSummonChange;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        var summonChildren = GetComponentsInChildren<Player>();
        foreach (var child in summonChildren)
        {
            summons.Add(child);
            if (CreditsManager.newGames > 0)
                child.acquired = true;
            else
                child.acquired = GameManager.GetSummonFlag(child.summonType);
            child.gameObject.SetActive(false);
        }

        summons[0].gameObject.SetActive(true);
    }

    public void Summon(int value)
    {
        var lastSummon = summons[currentSummonIndex];
        WrapSummonIndex(value);
        while (!summons[currentSummonIndex].acquired || summons[currentSummonIndex].health <= 0)
        {
            WrapSummonIndex(value);
        }

        var selectedSummon = summons[currentSummonIndex];

        if (selectedSummon == lastSummon) return;
        
        selectedSummon.transform.position = lastSummon.transform.position;
        selectedSummon.movement = lastSummon.movement;
        selectedSummon.direction = lastSummon.direction;
        OnSummonChange.Invoke(selectedSummon);
        selectedSummon.gameObject.SetActive(true);
        lastSummon.gameObject.SetActive(false);
        var smokeClone = Instantiate(smoke, selectedSummon.transform);
        smokeClone.transform.position = selectedSummon.transform.position + Vector3.down;
        audioSource.PlayOneShot(switchClip, GameManager.SfxVolumeScale);
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

    public bool HasSummonsLeft()
    {
        bool hasSummonsLeft = false;
        foreach (var summon in summons)
            if(summon.health > 0 && summon.acquired)
                hasSummonsLeft = true;
        return hasSummonsLeft;
    }

    public void Summon(InputAction.CallbackContext context)
    {
        if (context.started && !summons[currentSummonIndex].attacking)
        {
            var value = (int)Mathf.Sign(context.ReadValue<float>());
            Summon(value);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            summons[currentSummonIndex].Attack();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            summons[currentSummonIndex].Move(context.ReadValue<Vector2>());
        }
    }

}
