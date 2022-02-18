using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacityMine : MonoBehaviour
{
    public int maxAmountMine = 3;
    public int currentAmountMine;
    public Text amountMine;
    void Start()
    {
        amountMine.text = maxAmountMine.ToString();
        currentAmountMine = maxAmountMine;
        amountMine.color = Color.red;
    }

    public void AddMine()
    {
        if (currentAmountMine < maxAmountMine)
        {
            currentAmountMine += 1;
            amountMine.text = currentAmountMine.ToString();
        }
        ColorOfAmount();
    }

    public void ThrowMine()
    {
        if (currentAmountMine > 0)
        {
            currentAmountMine -= 1;
            amountMine.text = currentAmountMine.ToString();
        }
        ColorOfAmount();
    }

    private void ColorOfAmount()
    {
        if (currentAmountMine == maxAmountMine)
        {
            amountMine.color = Color.red;
        }
        else
        {
            amountMine.color = Color.yellow;
        }
    }
}
