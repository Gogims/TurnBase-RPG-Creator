using System;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipppable
{
    /// <summary>
    /// Tipo de armadura: pecho, pantalones, aretes, etc.
    /// </summary>
    public ArmorType Type;

    public enum ArmorType
    {
        Chest,
        Leg,
        Feet,
        Necklace,
        Ring,
        Helmet
    };

    public Armor()
    {
        Stats = new Attribute();
        Image = new Sprite();
    }
}