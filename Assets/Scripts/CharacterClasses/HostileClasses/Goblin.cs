using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinArcher : HostileCharacter
{
    [Header("Goblin Specific")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRate = 3f;
    private float nextAttackTime = 0f;

    [Tooltip("Hangi mesafeden ok atmaya baþlasýn?")]
    [SerializeField] private float attackDistance = 10f;

    [Tooltip("Hangi oranda tam isabet etsin? (0.4 = %40)")]
    [SerializeField] private float accuracy = 0.4f;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform bowStringPoint; // Okun çýkacaðý el/yay noktasý
    [SerializeField] private float arrowSpeed = 15f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    public bool isDead = false;

    public override float Health => base.Health;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Karakterin dibine girmemesi için NavMesh durma mesafesini setle
        agent.stoppingDistance = attackDistance - 1f;
    }

    void Update()
    {
        if (isDead || player == null) return;

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

    private void FollowPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public override void Attack()
    {
        // Ok atarken hareket etmesin ama oyuncuya dönsün
        agent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack"); // Ok atma animasyonunu tetikle
            nextAttackTime = Time.time + attackRate;
            // Not: Ok yaratma iþlemini animasyonun tam o anýnda yapmak için 
            // istersen Invoke kullanabilirsin ya da aþaðýdakini direkt çaðýrabilirsin.
            StartCoroutine(ShootWithDelay(1f));
        }
    }

    IEnumerator ShootWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isDead) yield break;

        // 1. Hedef Belirleme (%40 Doðruluk Mantýðý)
        Vector3 targetPosition = player.position + Vector3.up * 1.2f; // Karakterin gövdesini hedefle

        if (Random.value > accuracy)
        {
            // Iskala: Rastgele bir sapma ekle
            Vector3 deviation = Random.insideUnitSphere * 2.5f;
            targetPosition += deviation;
        }

        // 2. Oku Oluþtur ve Fýrlat
        GameObject arrow = Instantiate(arrowPrefab, bowStringPoint.position, Quaternion.identity);

        // Oku hedefe yönelt
        Vector3 shootDirection = (targetPosition - bowStringPoint.position).normalized;
        arrow.transform.forward = shootDirection;

        // Oku ileri it (Okun üzerinde Rigidbody olmalý)
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * arrowSpeed;
        }
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