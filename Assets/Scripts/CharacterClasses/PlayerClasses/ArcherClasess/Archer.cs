using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerCharacter, ISpecialSkill, IUltimateSkill
{
    private Animator animator;
    private Rigidbody rb;


    [Header("Archer Özellikleri")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float ultimateAbilityDamage = 50f;


    [Header("Ok ve Atýþ Ayarlarý")]
    [SerializeField] private GameObject arrowPrefab; 
    [SerializeField] private Transform arrowSpawnPoint;

    [Header("special ability ayarlarý")]
    [SerializeField] private float specialAbilityCooldown = 5f;
    [SerializeField] private float specialAbilityRadius = 5f;
    [SerializeField] private float specialAbilityPushForce = 10f;
    private float lastSpecialAbilityTime = 0f;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
       
    }

  
    public override void Attack()
    {
        SpawnArrow(attackDamage);
        

    }

    public void SpecialAbility()
    {
        if(Time.time - lastSpecialAbilityTime < specialAbilityCooldown) return;
        animator.SetTrigger("Special");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, specialAbilityRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player")) continue;

            // Herhangi bir Rigidbody'ye sahip olan objeleri ittir
            Rigidbody targetRb = hitCollider.attachedRigidbody;
            if (targetRb != null)
            {
                Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                targetRb.AddForce(pushDirection * specialAbilityPushForce, ForceMode.Impulse);
            }

        }
    }

    public void UltimateAbility()
    {
        animator.SetTrigger("Ultimate");
        SpawnArrow(ultimateAbilityDamage );
    }

    private void SpawnArrow(float damage, Quaternion? customRotation = null)
    {
         
        Quaternion rotation = customRotation ?? arrowSpawnPoint.rotation;
        GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnPoint.position, rotation);
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.SetDamage(damage);
        }
    }

    public override void die()
    {
        animator.SetTrigger("Die");
    }

    public override float Health { get;  }
}
