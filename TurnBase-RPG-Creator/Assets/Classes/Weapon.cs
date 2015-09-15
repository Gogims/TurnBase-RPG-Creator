using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Id;

    public string Name;

    public string Description;

    public int HitType;

    public float HitRate;

    public int NumberHit;

    public Attribute Stats;

    public Weapon ()        
	{
        Stats = new Attribute();
	}

    public static string[] WeaponTypes()
    {
        string[] types = { "Axe", "Sword", "Spear"};        
       
        return types;
    }
}