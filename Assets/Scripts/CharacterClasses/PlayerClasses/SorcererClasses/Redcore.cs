using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redcore : MonoBehaviour
{
    public float speed = 20f;
    private float damage = 15f;
    public float lifeTime = 5f;
    private float damageRadius = 3f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
    public void SetDamageRadius(float r)
    {
        damageRadius = r;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return; 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            var dmg = hitCollider.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }


}
