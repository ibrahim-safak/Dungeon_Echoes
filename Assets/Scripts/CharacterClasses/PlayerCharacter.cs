using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BaseCharacter
{
    public override float Health => throw new System.NotImplementedException();

    protected override void Start()
    {
        base.Start(); // BaseCharacter'daki can doldurmayý çalýþtýr
    }

    public override void die()
    {
        Debug.Log("OYUNCU ÖLDÜ! Game Over.");
        // Buraya oyun bitiþ ekraný kodu gelecek.
    }

    // Alt sýnýflar (Warrior, Mage) bunu dolduracak
    public virtual void Attack()
    {
        // Boþ
    }
    
}
