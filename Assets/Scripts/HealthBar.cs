using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image bar;

    private void Awake()
    {
        bar = GetComponentInChildren<Image>();
    }

    public void OnEnable()
    {
        Player.OnHealthChange += UpdateHealth;
    }

    public void OnDisable()
    {
        Player.OnHealthChange -= UpdateHealth;
    }

    private void UpdateHealth(float newHealth, float maxHealth)
    {
        bar.fillAmount = newHealth / maxHealth;
    }
}
