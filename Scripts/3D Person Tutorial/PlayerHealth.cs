using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealt = 100;
    [SerializeField]
    private int currentHealt;

    public PlayerHealthBar playerHealthBar;

    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    private void Start()
    {
        currentHealt = maxHealt;
        playerHealthBar.SetBarHealth(maxHealt);
    }

    public void TakeDamage(int damage)
    {
        currentHealt -= damage;
        playerHealthBar.SetHealth(damage);
        if (currentHealt <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<Animator>().SetTrigger("isDeath");
        GetComponent<DeathHandler>().HandleDeath();
    }
    public void SetHealt()
    {
        currentHealt = maxHealt;
        playerHealthBar.SetMaxHealth();
    }

}
