using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerBar : MonoBehaviour
{
    public Slider slider;
    public void SetBarPower(int Power)
    {
        slider.maxValue = Power;
        slider.value = Power;
    }

    public void SetPowerDown(int Power)
    {
        slider.value -= Power;
    }
    public void SetPowerUp(int Power)
    {
        slider.value += Power;
    }
}
