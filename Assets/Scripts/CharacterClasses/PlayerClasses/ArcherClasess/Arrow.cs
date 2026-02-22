using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    public float speed = 25f;
    private float damage = 10f;
    public float lifeTime = 5f;
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Destroy(gameObject, lifeTime);
    }

    private void Start()
    {
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }
    }


    public void SetDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return; 

        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
