using System;
using System.Collections.Generic;
using UnityEngine;

public class Armor : RPGElement
{
    /// <summary>
    /// Informacion de la armadura
    /// </summary>
    public AbstractArmor Data;

    public Armor()
    {
        Data = new AbstractArmor();
    }
}

[Serializable]
public class AbstractArmor : Equippable
{
    /// <summary>
    /// Tipo de armadura: pecho, pantalones, aretes, etc.
    /// </summary>
    public ArmorType Type;

    /// <summary>
    /// Nombre de la armadura
    /// </summary>
    public string ArmorName = string.Empty;

    public enum ArmorType
    {
        Chest,
        Leg,
        Feet,
        Necklace,
        Ring,
        Helmet
    };
}