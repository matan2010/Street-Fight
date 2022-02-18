using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Canvas HealthBar;
    [SerializeField]
    private int maxHealt = 30;
    public int currenttHealt;
    private Collider body;

    public Transform positionThrow;
    public GameObject DropLootPrefab;
    public EnemyHealthBar enemyHealthBar;
    GameObject DropLootTarget;

    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    private void Start()
    {
        currenttHealt = maxHealt;
        enemyHealthBar.SetBarHealth(maxHealt);
        body = GetComponent<Collider>();
        DropLootTarget = GameObject.FindGameObjectWithTag("DropLootTracker");
    }
    public void TakeDamage(int damage)
    {
        currenttHealt -= damage;
        enemyHealthBar.SetHealth(damage);
        if (currenttHealt <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        body.enabled = false;
        HealthBar.enabled = false;
        if (isDead) return;
        isDead = true;
        GetComponent<Animator>().SetTrigger("isDeath");
        
    }
    public void ThrowExp()
    {
        for (int i = 0; i < 4; i++)
        {
            var go = Instantiate(DropLootPrefab, positionThrow.position + new Vector3(Random.Range(0.2f, 0.4f), Random.Range(0.5f, 1f), Random.Range(0.2f, 0.4f)), Quaternion.identity);
            go.GetComponent<Follow>().target = DropLootTarget.transform;
        }
    }
}
