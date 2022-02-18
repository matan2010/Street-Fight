using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMine : MonoBehaviour
{

    private float radius = 2f;
    public float Radius { set { radius = value; } }
    public float force = 250f;
    private int damage = 15;
    public int Damage { set { damage = value; } }
    public AudioSource normalMode;
    public GameObject explosionEffect;
    [SerializeField] float maxIntensity = 100f;
    [SerializeField] float minIntensity = 1f;
    [SerializeField] float explosionTime = 0.5f;
    [SerializeField] float delayTime = 1f;
    float currentIntensity;
    [SerializeField] public float intensityDecay = 0.1f;
    [SerializeField] public float triggerMine = 1f;

    bool maxcapacity = true;
    public Light myLight;

    public Collider mineCollider;
    private void Start()
    {
        myLight = GetComponent<Light>();
        myLight.intensity = maxIntensity;
        currentIntensity = maxIntensity;
        StartCoroutine(ActivationDelay());
        mineCollider.enabled = false;
    }
    void Update()
    {
        if (maxcapacity)
        {
            myLight.intensity = currentIntensity;
            currentIntensity -= intensityDecay;
            if(minIntensity >= currentIntensity)
            {
                maxcapacity = false;
            }
        }
        else
        {
            currentIntensity += intensityDecay;
            myLight.intensity = currentIntensity;
            if (maxIntensity <= currentIntensity)
            {
                maxcapacity = true;
            }
        }
    }


    IEnumerator ActivationDelay()
    {

        yield return new WaitForSeconds(delayTime);
        mineCollider.enabled = true;

    }



        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Zombie"))
        {
            normalMode.Stop();
            StartCoroutine(ActivatingMine());
        }
    }

    IEnumerator ActivatingMine()
    {
        intensityDecay = triggerMine;

        yield return new WaitForSeconds(explosionTime);
        Instantiate(explosionEffect,transform.position,transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {

            if (nearbyObject.gameObject.tag == "Player")
            {
               
                nearbyObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            if (nearbyObject.gameObject.tag == "Zombie")
            {
                nearbyObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
        Destroy(transform.parent.gameObject);
    }
}
