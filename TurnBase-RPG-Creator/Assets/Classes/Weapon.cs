using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipppable
{
    /// <summary>
    /// Tipo de arma: Espada, Lanza, Hacha, etc.
    /// </summary>
    public int HitType;

    /// <summary>
    /// Porcentaje que el arma tiene para realizar su ataque
    /// </summary>
    public float HitRate;

    /// <summary>
    /// Cantidad de golpes que hace el arma por ataque
    /// </summary>
    public int NumberHit;

    /// <summary>
    /// Método encargado de devolver todos los tipos de armas del engine
    /// </summary>
    /// <returns>Listado de todos los tipos de armas</returns>
    public static string[] WeaponTypes()
    {
        string[] types = { "Axe", "Sword", "Spear"};        
       
        return types;
    }
}