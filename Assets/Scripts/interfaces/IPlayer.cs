using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer : IDamageable
{
    void Attack(); 
}

public interface ISpecialSkill
{
    void SpecialAbility(); 
}

public interface IUltimateSkill
{
    void UltimateAbility();
}
