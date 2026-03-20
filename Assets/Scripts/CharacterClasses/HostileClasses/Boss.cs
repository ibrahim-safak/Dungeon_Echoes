using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : HostileCharacter
{
    [Header("Menzil Ayarlarý")]
    [SerializeField] private float attackDistance = 3f;      // Yakýn vuruþ mesafesi
    [SerializeField] private float jumpAttackRange = 12f;   // Zýplama saldýrýsý tetiklenme mesafesi

    [Header("Saldýrý Deðerleri")]
    [SerializeField] private float meleeDamage = 15f;
    [SerializeField] private float jumpDamage = 30f;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private float jumpCooldown = 5f;

    private float nextAttackTime = 0f;
    private float nextJumpTime = 0f;
    private bool speedBoosted = false;
    private bool isJumping = false;
    public bool isDead = false;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb.isKinematic = true; 
    }

    void Update()
    {
        if (isDead || player == null) return;

        CheckEnrage();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isJumping)
        {
            if (distanceToPlayer <= attackDistance)
            {
                Attack();
            }
            else if (distanceToPlayer >= 7f && Time.time >= nextJumpTime)
            {
                StartCoroutine(JumpAttackAction());
            }
            else
            {
                FollowPlayer();
            }
        }

        animator.SetFloat("MovementSpeed", agent.velocity.magnitude);
    }

    private void CheckEnrage()
    {
        if (Health <= base.Health * 0.5f && !speedBoosted)
        {
            agent.speed *= 1.8f; // Hýzý artýr
            speedBoosted = true;
            Debug.Log("Boss Öfkelendi! Hýz Artýþý!");
        }
    }

    private void FollowPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public override void Attack()
    {
        agent.isStopped = true;
        LookAtPlayer();

        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(DelayedMeleeHit(0.5f));
            nextAttackTime = Time.time + attackRate;
        }
    }

    IEnumerator JumpAttackAction()
    {
        isJumping = true;
        nextJumpTime = Time.time + jumpCooldown;

        agent.enabled = false;
        rb.isKinematic = false;

        animator.SetTrigger("JumpAttack");

        // Ýleri ve yukarý doðru kuvvet uygula
        Vector3 jumpDirection = (player.position - transform.position).normalized;
        rb.AddForce(jumpDirection * 10f + Vector3.up * 8f, ForceMode.Impulse);

        yield return new WaitForSeconds(1.2f); // Havada kalma ve yere iniþ süresi tahmini

        // Yere indiðinde alan hasarý (AoE)
        ApplyAreaDamage(4f, jumpDamage);

        rb.isKinematic = true;
        agent.enabled = true;
        isJumping = false;
    }

    private void ApplyAreaDamage(float radius, float damageValue)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radius);
        foreach (var t in targets)
        {
            if (t.CompareTag("Player"))
            {
                t.GetComponent<PlayerCharacter>()?.TakeDamage(damageValue);
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
    }

    IEnumerator DelayedMeleeHit(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Yakýn mesafe kontrolü
        if (Vector3.Distance(transform.position, player.position) <= attackDistance + 1f)
        {
            player.GetComponent<PlayerCharacter>()?.TakeDamage(meleeDamage);
        }
    }
}