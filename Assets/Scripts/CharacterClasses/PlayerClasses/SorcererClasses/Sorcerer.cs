using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : PlayerCharacter, ISpecialSkill, IUltimateSkill
{
    private Animator animator;
    private Rigidbody rb;

    [Header("Sorcerer Özellikleri")]
    [SerializeField] private float attackDamage = 15f;


    [Header("Büyü ve Atış Ayarları")]
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform magicSpawnPoint;

    [Header("Özel Yetenek Ayarları")]
    [SerializeField] private float specialAbilityCooldown = 8f;
    [SerializeField] private float specialAbilityRadius = 7f;
    [SerializeField] private float specialAbilityDuration = 3f;
    [SerializeField] private float boundaryPadding = 0.05f;
    [SerializeField] private LayerMask affectedMask;
    private float lastSpecialAbilityTime = 0f;

    [Header("Ultimate Yetenek Ayarları")]
    [SerializeField] private float ultimateCooldown = 20f;
    [SerializeField] private float ultimateDuration = 5f;
    [SerializeField] private float ultimateRadius = 30f;
    [SerializeField] private float ultimateDamagePerSecond = 20f;
    [SerializeField] private float ultimateForcePerSecond = 15f;
    private float lastUltimateTime = 0f;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }
    public override void Attack()
    {
        SpawnMagicProjectile(attackDamage);
    }

    private void SpawnMagicProjectile(float damage)
    {
        GameObject projectile = Instantiate(magicProjectilePrefab, magicSpawnPoint.position, magicSpawnPoint.rotation);
        Redcore magicScript = projectile.GetComponent<Redcore>();
        if (magicScript != null)
        {
            magicScript.SetDamage(damage);
        }
    }
    public void SpecialAbility()
    {
        if (Time.time - lastSpecialAbilityTime < specialAbilityCooldown) return;
        lastSpecialAbilityTime = Time.time;

        animator.SetTrigger("Special");
        StartCoroutine(SpecialPulseRoutine());
    }

    public void UltimateAbility()
    {
        if (Time.time - lastUltimateTime < ultimateCooldown) return;
        lastUltimateTime = Time.time;

        animator.SetTrigger("Ultimate");
        StartCoroutine(UltiPulseRoutine());
    }

    private IEnumerator SpecialPulseRoutine()
    {
        {
            float t = 0f;

            while (t < specialAbilityDuration)
            {
                Collider[] hits = (affectedMask.value == 0)
                    ? Physics.OverlapSphere(transform.position, specialAbilityRadius)
                    : Physics.OverlapSphere(transform.position, specialAbilityRadius, affectedMask);

                foreach (var hit in hits)
                {
                    if (!hit) continue;

                    // Kendini etkilemesin
                    if (hit.transform.IsChildOf(transform)) continue;

                    // Player'ları etkilemesin
                    if (hit.transform.root.CompareTag("Player")) continue;

                    Rigidbody targetRb = hit.attachedRigidbody;
                    if (targetRb == null) continue;

                    // Ek güvenlik: rigidbody benimkisi ise geç
                    if (targetRb.transform.IsChildOf(transform)) continue;

                    Vector3 toTarget = targetRb.position - transform.position;
                    float dist = toTarget.magnitude;
                    if (dist < 0.0001f) continue;

                    Vector3 outward = toTarget / dist; // dışarı yön

                    // 1) İçeri doğru hız bileşenini kaldır (yaklaşmayı engeller, itmez)
                    Vector3 v = targetRb.velocity;
                    float inwardSpeed = Vector3.Dot(v, -outward); // merkeze doğru hız
                    if (inwardSpeed > 0f)
                    {
                        targetRb.velocity = v + outward * inwardSpeed;
                    }

                    // 2) Çok az pozisyon düzeltmesi (uçurma yok)
                    float minDist = specialAbilityRadius + boundaryPadding;
                    if (dist < minDist)
                    {
                        Vector3 targetPos = transform.position + outward * minDist;
                        targetRb.MovePosition(targetPos);
                    }
                }

                t += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, specialAbilityRadius);
    }

private IEnumerator UltiPulseRoutine()
    {
        float t = 0f;

        while (t < ultimateDuration)
        {
            Collider[] hits = (affectedMask.value == 0)
                ? Physics.OverlapSphere(transform.position, ultimateRadius)
                : Physics.OverlapSphere(transform.position, ultimateRadius, affectedMask);

            foreach (var hit in hits)
            {
                if (!hit) continue;

                if (hit.transform.IsChildOf(transform)) continue;
                if (hit.transform.root.CompareTag("Player")) continue;

                
                 
                if (enemy != null)
                {
                    enemy.TakeDamage(ultimateDamagePerSecond * Time.deltaTime);
                }

                // FORCE (yatay, süreye yay)
                Rigidbody targetRb = hit.attachedRigidbody;
                if (targetRb != null)
                {
                    if (targetRb.transform.IsChildOf(transform)) continue;

                    Vector3 dir = (targetRb.position - transform.position);
                    dir.y = 0f;
                    dir = dir.sqrMagnitude < 0.0001f ? transform.forward : dir.normalized;

                    // Süreye yaymak için ForceMode.Force daha mantıklı
                    targetRb.AddForce(dir * ultimateForcePerSecond, ForceMode.Force);
                }
            }

            t += Time.deltaTime;     // ✅ foreach dışına alındı
            yield return null;       // ✅ foreach dışına alındı
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, specialAbilityRadius);
        Gizmos.DrawWireSphere(transform.position, ultimateRadius);
    }
}

