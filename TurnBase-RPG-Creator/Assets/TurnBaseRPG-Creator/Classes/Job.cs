using System;
using System.Collections.Generic;
using UnityEngine;

public class Job : RPGElement
{
    public Formula MaxHP;

    public Formula MaxMP;

    public Formula Attack;

    public Formula Defense;

    public Formula MagicAttack;

    public Formula MagicDefense;

    public Formula Agility;

    public Formula Luck;

    public Formula XP;

    public List<AbstractAbility> Abilities;

    public Job()
    {
        Abilities = new List<AbstractAbility>();
    }
}
