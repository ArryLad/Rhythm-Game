using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    //Health UI uses a slider
    public Slider slider;

    //Text to display helath total
    public TextMeshProUGUI healthText;

    public void SetMaxHealth(int health)
    {
        //At the start of the game ammo is at max so set all variables to ammo
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health + "/" + health;
    }

    public void SetHealth(int health, int maxHealth)
    {
        //slider is alwasy equal to current health
        slider.value = health;

        //text display health out of max health
        healthText.text = health + "/" + maxHealth;
    }
}
