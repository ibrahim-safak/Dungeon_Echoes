using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GoblinArcher : HostileCharacter
{
    [Header("Goblin Specific")]
    [SerializeField] private float attackRate = 3f;
    private float nextAttackTime = 0f;

    [Tooltip("Hangi mesafeden ok atmaya baþlasýn?")]
    [SerializeField] private float attackDistance = 10f;
    [SerializeField] private float fleeDistance = 5f;

    [Tooltip("Hangi oranda tam isabet etsin? (0.4 = %40)")]
    [SerializeField] private float accuracy = 0.4f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform bowStringPoint; 
    [SerializeField] private float arrowSpeed = 15f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    public bool isDead = false;
    public bool isFlee = false;

    public override float Health => base.Health;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.stoppingDistance = attackDistance - 1f;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isFlee && distanceToPlayer > attackDistance)
        {
            isFlee = false;
        }

        if (Health <= 50 && distanceToPlayer < fleeDistance)
        {
            FleeFromPlayer();
        }
        else if (distanceToPlayer <= attackDistance)
        {
            Attack();
        }
        else
        {
            animator.SetBool("Attack", false);
            FollowPlayer();
        }

        animator.SetFloat("MovementSpeed", agent.velocity.magnitude);
    }

    private void FleeFromPlayer()
    {
        agent.stoppingDistance = 0f;
        agent.isStopped = false;
        isFlee = true; 
        animator.SetBool("Attack", false);

        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 targetPoint = transform.position + fleeDirection * 10f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        agent.speed = 2f;
    }

    public override void Attack()
    {
        agent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Time.time >= nextAttackTime)
        {                                                            
            animator.SetBool("Attack", true);

            nextAttackTime = Time.time + attackRate;
            StartCoroutine(ShootWithDelay(1.4f));
        }
    }
    private void FollowPlayer()
    {
        agent.speed = 3.5f; 
        agent.stoppingDistance = attackDistance - 1f; 
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
    IEnumerator ShootWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isDead) yield break;
        
        Vector3 targetPosition = player.position + Vector3.up * 1.2f; 

        if (Random.value > accuracy)
        {
            Vector3 deviation = Random.insideUnitSphere * 2.5f;
            targetPosition += deviation;
        }

        GameObject arrow = Instantiate(arrowPrefab, bowStringPoint.position, Quaternion.identity);

        Vector3 shootDirection = (targetPosition - bowStringPoint.position).normalized;
        arrow.transform.forward = shootDirection;

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * arrowSpeed;
        }
        animator.SetBool("Attack", false);
    }

    public override void TakeDamage(float damage)
    {
        if (isDead) return;
        base.TakeDamage(damage);
        if (Health <= 0) die();
    }

    public override void die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);

        agent.enabled = false;

        GetComponent<Collider>().enabled = false;

        Destroy(this.gameObject, 4f);
    }
}