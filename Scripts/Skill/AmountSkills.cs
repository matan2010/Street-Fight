using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountSkills : MonoBehaviour
{
    private int myAmountSkills = 0;
    public Text myAmount;

    private void Start()
    {
        myAmount.text = myAmountSkills.ToString();
    }
    public void AddSkills()
    {
        myAmountSkills++;
        myAmount.text = myAmountSkills.ToString();
    }
    public int GetAmountSkills()
    {
        return myAmountSkills;
    }
    public Text GetAmount()
    {
        return myAmount;
    }
    public void SetAmount(int amount)
    {
        myAmountSkills -= amount;
        myAmount.text = myAmountSkills.ToString();
    }
}
