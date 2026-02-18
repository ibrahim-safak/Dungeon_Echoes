using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerCharacter,  ISpecialSkill, IUltimateSkill
{

    [Header("Warrior Özellikleri")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float Damage = 20f;
    [SerializeField] private float attackCooldown = 0.35f; 
    [SerializeField] private float runSpeed = 5f;

    [Header("Atýlma ayarlarý")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 2f;

    [Header("referanslar")]
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private Transform CameraTransform;
    private Animator animator;
    [Header("Ulti Ayarlarý")]
    [SerializeField] private float ultiDamage = 100f;
    [SerializeField] private float ultiRadius = 10f;
    [SerializeField] private float ultiCooldown = 10f;
    private float lastUltiTime = -100f;

    //deðiþkenler 
    private float lastAttackTime=0f;
    private float lastDashTime=0f;
    private bool isDashing = false;
    private Rigidbody rb;

    

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        var movement = GetComponent<PhysicalMovement>();
        if (movement != null) movement.moveSpeed = runSpeed;
    }

   
    public void SpecialAbility()
    {
        StartCoroutine(Dash());

    }


    private IEnumerator Dash()
    {
        if (Time.time - lastDashTime < dashCooldown || isDashing) yield break;

        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dashDir = CameraTransform.forward;
        dashDir.y = 0;
        dashDir.Normalize();

        animator.SetTrigger("Dash");

        rb.AddForce(dashDir * dashForce, ForceMode.VelocityChange);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector3.zero;
        isDashing = false;
    }

   
    
    public override float Health => throw new System.NotImplementedException();

    public override void die()
    {
        throw new System.NotImplementedException();
    }

    public void UltimateAbility()
    {
        if (Time.time - lastUltiTime < ultiCooldown) return;

        lastUltiTime = Time.time;
        Debug.Log("Ulti Kullanýldý: Yere Vurma!");

        animator.SetTrigger("Ultimate"); 

        // Alan hasarý kontrolü
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ultiRadius, LayerMask);
        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(ultiDamage);
            }
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Debug.Log("hýzlý saldýrý");
        animator.SetTrigger("Attack");
        RaycastHit hit;
        if (Physics.Raycast(CameraTransform.position, CameraTransform.forward, out hit, attackRange, LayerMask))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(Damage);
                Debug.Log("Düþmana " + Damage + " hasar verildi.");
            }
        }
    }

    
}
