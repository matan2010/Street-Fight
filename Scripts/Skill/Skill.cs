using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonController playerController;
    [SerializeField]
    private GameObject nails;
    [SerializeField]
    private GameObject fire;
    [SerializeField]
    private GameObject mine;
    private int secondLevelSkill = 2;
    private int thirdLeveSkills = 3;
    private int extraRadius = 5;

    //damage
    private int damageNail = 20;
    private int damageFire = 30;
    private int extraDamageMine = 40;
    private bool secondLevelBaseballBat = false;
    private bool secondLevelMine = false;

    private bool buySkillNail = false;
    private bool buySkillFire = false;
    private bool buySkillRange = false;
    private bool buySkillPowerful = false;

    [SerializeField]
    private AmountSkills amountSkills;
    private void Start()
    {
        nails.SetActive(false);
        fire.SetActive(false);
    }
    public void BaseballBatWhitNail() 
    {
       
        if (!buySkillNail && (amountSkills.GetAmountSkills() >= secondLevelSkill))
        {
            buySkillNail = true;
            buySkillFire = true;
            secondLevelBaseballBat = true;
            SetAmounSkill(secondLevelSkill);
            nails.SetActive(true);
            playerController.AttackDamage = damageNail;
        }
    }
    public void BaseballBatWhitFire() 
    {
        if (buySkillFire && (amountSkills.GetAmountSkills() >= thirdLeveSkills && secondLevelBaseballBat))
        {
            buySkillFire = false;
            SetAmounSkill(thirdLeveSkills);
            fire.SetActive(true);
            playerController.AttackDamage = damageFire;
        }
    }

    public void MineRange()
    {
        if (!buySkillRange && (amountSkills.GetAmountSkills() >= secondLevelSkill))
        {
            mine.GetComponentInChildren<FlashMine>().Radius = extraRadius;
            buySkillPowerful = true;
            buySkillRange = true;
            secondLevelMine = true;
            SetAmounSkill(secondLevelSkill);
        }
    }

    public void PowerfulMine()
    {
        if (buySkillPowerful && (amountSkills.GetAmountSkills() >= thirdLeveSkills && secondLevelMine))
        {
            mine.GetComponentInChildren<FlashMine>().Damage = extraDamageMine;
            buySkillPowerful =false;
            SetAmounSkill(thirdLeveSkills);
        }
    }

    private void SetAmounSkill(int amountSkill)
    {
        amountSkills.SetAmount(amountSkill);
    }
}
