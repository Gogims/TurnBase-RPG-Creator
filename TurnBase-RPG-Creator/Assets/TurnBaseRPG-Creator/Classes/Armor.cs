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
    public enum ArmorType
    {
        Body,
        Feet,
        Necklace,
        Ring,
        Helmet
    };
}