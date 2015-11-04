using System;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equipppable
{
    /// <summary>
    /// Tipo de armadura: pecho, pantalones, aretes, etc.
    /// </summary>
    public int ArmorType;

    public enum Type
    {
        Chest,
        Leg,
        Feet,
        Necklace,
        Ring
    };

    public Armor()
    {
        Stats = new Attribute();
        Image = new Sprite();
    }

    public static string[] ArmorTypes()
    {
        string[] types = { "Chest", "Leg", "Feet", "Necklace", "Ring" };

        return types;
    }
}