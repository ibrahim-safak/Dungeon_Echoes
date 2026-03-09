using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redcore : MonoBehaviour
{
    public float speed = 20f;
    private float damage = 15f;
    public float lifeTime = 5f;
    private float damageRadius = 3f;

    [Header("Vortex Settings")]
    [SerializeField] public float pullForce = 5f; 
    [SerializeField] public float launchDelay = 5f;
    [SerializeField] private GameObject explosionPrefab;
    private Rigidbody rb;

    public bool hasExploded = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime + launchDelay);
    }

    private void Start()
    {
        StartCoroutine(LaunchAfterDelay());
    }

    private IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(launchDelay);

        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    public void SetDamage(float d) => damage = d;
    public void SetDamageRadius(float r) => damageRadius = r;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        hasExploded = true;
                
        ApplyVortexAndDamage();

        Destroy(gameObject);
        SpawnExplosionEffect(collision);
    }

    private void SpawnExplosionEffect(Collision collision)
    {
        if (explosionPrefab != null)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 spawnPos = contact.point; 

            Quaternion spawnRot = Quaternion.LookRotation(contact.normal);

            GameObject effectInstance = Instantiate(explosionPrefab, spawnPos, spawnRot);

            
            Destroy(effectInstance, 2f);
        }
    }
    private void ApplyVortexAndDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (var hitCollider in hitColliders)
        {
            Rigidbody targetRb = hitCollider.GetComponent<Rigidbody>();
            if (targetRb != null && hitCollider.gameObject != gameObject)
            {
                Vector3 pullDirection = transform.position - hitCollider.transform.position;

                targetRb.AddForce(pullDirection.normalized * pullForce, ForceMode.Impulse);
            }

            var dmg = hitCollider.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
