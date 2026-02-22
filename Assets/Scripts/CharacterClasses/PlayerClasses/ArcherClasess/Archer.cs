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
   

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
       
    }

  
    public override void Attack()
    {
        animator.SetTrigger("Attack"); 
        SpawnArrow(attackDamage);
    }

    public void SpecialAbility()
    {
        animator.SetTrigger("Special");
        
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
