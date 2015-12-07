using System;
using System.Collections.Generic;
using UnityEngine;

public class Job : RPGElement
{
    public AbstractJob Data;

    public Job()
    {
        Data = new AbstractJob();
    }
}

[Serializable]
public class AbstractJob
{
    public string JobName = string.Empty;

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

    public AbstractJob()
    {
        Abilities = new List<AbstractAbility>();
        AbstractAbility aux = new AbstractAbility();
        aux.ApplyStatePorcentage = 2;
        aux.Ability = "Fire";
        Abilities.Add(aux);
        Abilities.Add(aux);
        Abilities.Add(aux);
        Abilities.Add(aux);
        Abilities.Add(aux);
        Abilities.Add(aux);
        Abilities.Add(aux);
        Abilities.Add(aux);
    }
}
