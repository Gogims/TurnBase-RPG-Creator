using System;
using System.Collections.Generic;

public class Enemy : Actor
{
    /// <summary>
    /// Información del enemigo
    /// </summary>
    public AbstractEnemy Data;

    public Enemy()
    {
        Data = new AbstractEnemy();
    }
}

[Serializable]
public class AbstractEnemy : AbstractActor
{
    public int RewardExperience;
    public int RewardCurrency;
    public List<Rate> Items;    

    public AbstractEnemy()
    {
        Items = new List<Rate>();
    }
}

[Serializable]
public class Rate
{
    /// <summary>
    /// Elemento que tiene un porcentage para ser aplicado
    /// </summary>
    public AbstractUsable Element;

    /// <summary>
    /// Porcentage para aplicar el elemento
    /// </summary>
    public float ApplyRate;

    public Rate()
    {
        Element = new AbstractUsable();
    }
}