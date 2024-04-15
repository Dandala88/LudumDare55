using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewGamePlusCounter : MonoBehaviour
{
    private TMP_Text tmp;

    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if(CreditsManager.newGames > 0)
            tmp.text = $"+ {CreditsManager.newGames}";
        else
            tmp.text = string.Empty;
    }
}
