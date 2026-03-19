using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour
{
    public float speed = 25f;
    private float damage = 10f;
    public float lifeTime = 5f;
    private Rigidbody rb;
    private bool hasHit = false; 
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        rb.velocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
    }

    
    public void SetDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return; 

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            hasHit = true;

            Stick(other.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Stick(Transform targetParent)
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true; 
        transform.SetParent(targetParent); 
        Destroy(gameObject, 2f); 
    }
}