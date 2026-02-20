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
    [SerializeField] private float arrowForce = 30f;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
       
    }

  
    public override void Attack()
    {
        animator.SetTrigger("Attack"); 
        SpawnArrow(attackDamage, arrowForce);
    }

    public void SpecialAbility()
    {
        animator.SetTrigger("Special");
        for (int i = -1; i <= 1; i++)
        {
            Quaternion spreadRotation = arrowSpawnPoint.rotation * Quaternion.Euler(0, i * 15f, 0);
            SpawnArrow(attackDamage * 1.5f, arrowForce, spreadRotation);
        }
    }

    public void UltimateAbility()
    {
        animator.SetTrigger("Ultimate");
        SpawnArrow(ultimateAbilityDamage, arrowForce * 2f);
    }

    private void SpawnArrow(float damage, float force, Quaternion? customRotation = null)
    {
        Quaternion rotation = customRotation ?? arrowSpawnPoint.rotation;
        GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnPoint.position, rotation);

        Arrow arrowScript = arrowObj.GetComponent<Arrow>();
        if (arrowScript != null) arrowScript.damage = damage;

        Rigidbody arb = arrowObj.GetComponent<Rigidbody>();
        arb.AddForce(arrowObj.transform.forward * force, ForceMode.Impulse);
    }

    public override void die()
    {
        animator.SetTrigger("Die");
    }

    public override float Health { get;  }
}
