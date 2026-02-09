using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour , IDamageable
{
    [Header("Temel Özellikler")]
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;

    public abstract float Health { get; }

    protected virtual void Start()
    {
        
        currentHealth = maxHealth;
    }
    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " hasar aldý. Kalan Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            die();
        }
    }

   
   
    public abstract void die();
}
