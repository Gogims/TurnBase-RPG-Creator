using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipppable
{
    public int HitType;

    public float HitRate;

    public int NumberHit;

    public static string[] WeaponTypes()
    {
        string[] types = { "Axe", "Sword", "Spear"};        
       
        return types;
    }
}