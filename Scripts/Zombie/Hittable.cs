using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private int hitId = 0;
    private EnemyHealth enemyHealth;
    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }
    public void Hit(HitData data ,int damage)
    {
        if (hitId != data.id)
        {
            hitId = data.id;
            enemyHealth.TakeDamage(damage);
        }  
    }
}
