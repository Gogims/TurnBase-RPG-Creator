using System;
using UnityEngine;

public class Ability : RPGElement
{
    public AbstractAbility Data;

    public Ability()
    {
        Data = new AbstractAbility();
    }
}

[Serializable]
public class AbstractAbility
{
    /// <summary>
    /// Descripción de la habilidad
    /// </summary>
    public string Description;
    /// <summary>
    /// El poder de ataque es influencia por la cantidad de Inteligencia del personaje
    /// </summary>
    public int AttackPower;
    /// <summary>
    /// Nivel mínimo para utilizar la habilidad
    /// </summary>
    public int MinLevel;
    /// <summary>
    /// Cuanta mana cuesta la habilidad
    /// </summary>
    public int MPCost;
    /// <summary>
    /// Probabilidad de aplicar un estado al utilizar la habilidad
    /// </summary>
    public float ApplyStatePorcentage;
    /// <summary>
    /// Ofensivo = Aplica el estado al utilizar habilidad. Defensivo = remueve el estado al utilizar la habilidad (si la tiene)
    /// </summary>
    public AbstractState State;
    /// <summary>
    /// Nombre de la habilidad
    /// </summary>
    public string Ability;
    /// <summary>
    /// Cuantos aliados/enemigos afectará la habilidad
    /// </summary>
    public Constant.AOE AreaOfEffect;
    /// <summary>
    /// Si la habilidad es ofensiva o defensiva.
    /// </summary>
    public Constant.OffenseDefense Type;

    public Constant.AttackType AttackType;

    public AbstractAbility()
    {
        State = new AbstractState();
    }

    
}