using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoBar : MonoBehaviour
{
    //Ammo UI is a slider
    public Slider slider;

    //Corrosponding ammo text
    public TextMeshProUGUI ammoText;

 
    public void SetMaxAmmo(int ammo)
    {
        //At the start of the game ammo is at max so set all variables to ammo
        slider.maxValue = ammo;
        slider.value = ammo;
        ammoText.text = ammo + "/" + ammo;
    }
   
    public void SetAmmo(int ammo, int maxAmmo)
    {
        //During the game when the player reload ammo is consumed
        //slider.value is alwayys equal to current ammo
        slider.value = ammo;
        //Text reflects amount of ammo away from maximum
        ammoText.text = ammo + "/" + maxAmmo;
    }
}
