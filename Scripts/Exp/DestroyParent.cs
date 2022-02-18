using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    private int exp;
    public int minExp = 2;
    public int maxExp = 5;

    private void Awake()
    {
        exp = Random.Range(minExp, maxExp);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropLootTracker"))
        {
            other.GetComponentInChildren<LevelBar>().addExp(exp);
            other.GetComponentInChildren<Exp>().GetExp(exp);

            Destroy(transform.parent.gameObject);
        }
    }
}
