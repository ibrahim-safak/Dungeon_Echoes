using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCharacter : BaseCharacter
{
    // BaseCharacter zaten Health saðlýyor, override gerekirse buradan yapabilirsiniz.
    // public override float Health => currentHealth; // opsiyonel

    protected override void Start()
    {
        base.Start(); // BaseCharacter'daki can doldurmayý çalýþtýr
    }

    public override void die()
    {
        Debug.Log("OYUNCU ÖLDÜ! Game Over.");
        // Buraya oyun bitiþ ekraný kodu gelecek.
    }

    // Alt sýnýflar (Warrior, Mage, Archer) bunu override edecek
    public virtual void Attack()
    {
        // Boþ - alt sýnýflar implement eder
    }
}
