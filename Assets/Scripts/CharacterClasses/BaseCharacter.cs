using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    [Header("Temel özellikler")]
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;

    public InventoryManager Inventory { get; private set; }

    protected virtual void Awake()
    {
        // Karakter uyanýrken üzerindeki Manager'ý bulur
        Inventory = GetComponent<InventoryManager>();

        if (Inventory == null)
        {
            // Eðer yoksa, çalýþma anýnda otomatik ekleyelim (Hata almaný önler)
            Inventory = gameObject.AddComponent<InventoryManager>();
            Debug.LogWarning($"{gameObject.name} üzerinde InventoryManager yoktu, otomatik eklendi.");
        }
    }

    // Saðlýk deðiþikliðini dinlemek için event (UI vb. için)
    public event Action<float, float> OnHealthChanged; // (current, max)

    
    public virtual float Health => currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public virtual void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log($"{gameObject.name} hasar aldý: {amount}. Kalan Can: {currentHealth}/{maxHealth}");

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            die();
        }
    }



    public abstract void die();
}
