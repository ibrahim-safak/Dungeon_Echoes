using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Arrow : MonoBehaviour
{
    public float speed = 25f;
    private float damage = 10f;
    public float lifeTime = 10f;
    private Rigidbody rb;
    private bool hasHit = false; 
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        rb.velocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (hasHit) return;
        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }
    public void SetDamage(float d)
    {
        damage = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        hasHit = true;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Stick(other);
    }

    private void Stick(Collider targetCollider)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; 

        GetComponent<Collider>().enabled = false;

        transform.SetParent(targetCollider.transform);

        Destroy(gameObject, 10f); 
    }
}