using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCharacter : BaseCharacter
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
    public override void Attack()
    {
        // Temel saldýrý mekanizmasý burada tanýmlanabilir.
        // Her karakter sýnýfý bunu kendi özel saldýrýsýyla override edebilir.
    }

}
