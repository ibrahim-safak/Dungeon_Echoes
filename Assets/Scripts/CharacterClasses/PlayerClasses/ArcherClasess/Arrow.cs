using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour
{
    public float speed = 25f;
    private float damage = 10f;
    public float lifeTime = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (rb == null) return;
        rb.velocity = transform.forward * speed;
    }

    public void SetDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kendine atanmýþ owner collision'ýný kontrol edin (isteðe baðlý)
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
