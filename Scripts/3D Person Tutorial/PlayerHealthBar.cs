using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public void SetBarHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value -= health;
    }
    public void SetMaxHealth()
    {
        slider.value = slider.maxValue;
    }
}
