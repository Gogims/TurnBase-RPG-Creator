using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Enemy : Actor
{
    public int RewardExperience;
    public int RewardCurrency;
    public List<Rate> Items;

    public Enemy()
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