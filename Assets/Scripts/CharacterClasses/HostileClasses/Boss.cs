using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : HostileCharacter
{
    [SerializeField] private float Damage = 10f;
    [SerializeField] private float attackRate = 2.7f;
    [SerializeField] private float nextAttackTime = 1f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private LayerMask LayerMask;
    public bool isDead = false;
}
