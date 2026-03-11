using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileCharacter : BaseCharacter
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void die()
    {
        Debug.Log($"{gameObject.name} (düþman) öldü.");
        animator?.SetTrigger("Die");

        // Collider ve davranýþlarý kapat
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // AI component varsa kapatýlmalý (ör: EnemyAI)
        var ai = GetComponent<MonoBehaviour>(); // uygun AI script'i burada kapatýlmalý
        if (ai != null) ai.enabled = false;

        Destroy(gameObject, 3f);
    }

    public virtual void Attack()
    {
        // Boþ - alt sýnýflar implement eder
    }
}
