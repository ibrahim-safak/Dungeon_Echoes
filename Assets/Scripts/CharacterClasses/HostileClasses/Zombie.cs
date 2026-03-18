using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : HostileCharacter
{
    [Header("Zombie Specific")]
    
    [SerializeField] private float Damage = 10f;
    [SerializeField] private float attackRate = 2.7f;
    [SerializeField] private float nextAttackTime = 1f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private LayerMask LayerMask;
    public bool isDead = false;


    [SerializeField] private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    [SerializeField] private GameObject bloodEffectPrefab;
    [SerializeField] private Transform bloodEffectSpawnPoint;
    public override void TakeDamage(float damage)
    {
        if (isDead) return;

        base.TakeDamage(damage);
        if (Health <= 0)
        {
            
            die();
        }
        // Kan efekti oluþtur
        if (bloodEffectPrefab != null)
        {
           GameObject effectInstance = Instantiate(bloodEffectPrefab, transform.position + 1.5f*Vector3.up, Quaternion.identity);
            Destroy(effectInstance, 1f); 
        }
       
    }

    public override float Health => base.Health;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (isDead || player == null) return;

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

        animator.SetFloat("MovementSpeed", agent.velocity.magnitude);
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
            StartCoroutine(DelayedDamage(0.8f)); 
            nextAttackTime = Time.time + attackRate;
        }


     }

    IEnumerator DelayedDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Saldýrýnýn isabet edip etmediðini kontrol et
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackDistance, 1f, LayerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerCharacter playerCharacter = hitCollider.GetComponent<PlayerCharacter>();
                if (playerCharacter != null)
                {
                    playerCharacter.TakeDamage(Damage);
                }
            }
        }
    }


    public override void die()
    {
        Debug.Log("Zombie öldü!");
        isDead = true; 
        animator.SetBool("IsDead", true);
        
        

        agent.enabled = false; 

        GetComponent<Collider>().enabled = false; 

        Destroy(this.gameObject, 4f);
    }

}
