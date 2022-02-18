using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    [SerializeField]
    private Text mineAmount;
    [SerializeField]
    private AmountSkills amountSkills;
    public int nextMaxLevel=3;
    public Slider slider;
    public Text levelNumber;
    public PlayerHealth playerHealth;
    [SerializeField]
    GameObject levelUp;

    private int level = 1;
    private int experience;
    private int experienceToNextLevel = 15;

    private void Start()
    {
        levelUp.SetActive(false);
        experience = 0;
        slider.value = slider.minValue;
    }
    public void SetBarLevel(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void addExp(int exp)
    {
        experience += exp;
        while (experience >= slider.maxValue)
        {
            mineAmount.GetComponent<CapacityMine>().AddMine();

            amountSkills.AddSkills();
           // skill.AddSkill();
            playerHealth.SetHealt();
            setMaxValue();
            LevelUp();
            slider.value = 0;
            level++;
            setlevelNumber(level);
            experience -= experienceToNextLevel;
        }
        slider.value = experience;
    }
    public void setlevelNumber(int level)
    {
        levelNumber.text = level.ToString();
    }
    public void setMaxValue()
    {
        slider.maxValue += nextMaxLevel;
    }
    private void LevelUp()
    {
        levelUp.SetActive(true);
        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(1.5f);
        levelUp.SetActive(false);
    }
}
