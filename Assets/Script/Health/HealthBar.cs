using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    public void SetMaxHealth(float maxHealth)
    {
        totalHealthBar.fillAmount = maxHealth / 10;
    }

    public void SetHealth(float currentHealth)
    {
        currentHealthBar.fillAmount = currentHealth / 10;
    }
}
