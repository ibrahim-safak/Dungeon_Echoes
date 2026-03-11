using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : HostileCharacter
{
    [Header("Zombie Specific")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float Damage = 10f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float nextAttackTime = 0f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private LayerMask LayerMask;

    [SerializeField] private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance)
        {
            Attack();
        }
        else
        {
            FollowPlayer();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    private void FollowPlayer() { 
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }


     public override void Attack()
     {
            agent.isStopped = true;
        // Oyuncuya bakma
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");

            nextAttackTime = Time.time + attackRate;

            
          //  player?.GetComponent<PlayerHealth>().TakeDamage(10);
        }

    }


}
