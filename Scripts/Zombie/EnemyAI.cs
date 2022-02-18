using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    GetPoint point;


    public bool canRun = true;

    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] string ZombieChase;
    private Rigidbody rb;

    int currenttHealt;
    Vector3 randomPoint;
    bool isWalking = true;
    Transform target;
    EnemyHealth health;
    NavMeshAgent navMeshAgent;
    //AudioManager audioManager;
    private float maxSpeed;
    Animator animator;
    [SerializeField] int runSpeed = 5;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
       // audioManager = FindObjectOfType<AudioManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
        currenttHealt = GetComponent<EnemyHealth>().currenttHealt;
    }

    void Update()
    {
        //Debug.Log("Distance to other: " + transform.position);
         if (health.IsDead)
          {
             enabled = false;
             navMeshAgent.enabled = false;
         }
        // else
        // {
        // animator.SetFloat("speed", 1);//rb.velocity.magnitude/maxSpeed);

        if (isWalking)
        {
            RandomWalking();
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isWalking = false;
            isProvoked = true;
            navMeshAgent.SetDestination(target.position);
        }
        // }

    }

    private void RandomWalking()
    {
        if (!navMeshAgent.hasPath)
        {
            randomPoint = point.GetRandomPoint();// GetPoint.Instance.GetRandomPoint();
            navMeshAgent.SetDestination(randomPoint);
        }
        float dist = Vector3.Distance(randomPoint, transform.position);
        if (dist < 2)
        {
            randomPoint = point.GetRandomPoint();// GetPoint.Instance.GetRandomPoint();
            navMeshAgent.SetDestination(randomPoint); ;
        }
    }

    private void EngageTarget()
    {
        FaceTarget();
        if ( (distanceToTarget >= navMeshAgent.stoppingDistance))
        {
            ChaseTarget();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {

        GetComponent<Animator>().SetTrigger("isRun");
        GetComponent<Animator>().SetBool("attack", false);
        if (GetComponent<EnemyHealth>().currenttHealt > 0)
        {
            navMeshAgent.SetDestination(target.position);
        }

        
    }
    void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true);
    }
    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void StartAttack()
    {

        navMeshAgent.speed = 0;
    }

    public void StartRun()
    {
        navMeshAgent.speed = runSpeed;
    }
}
