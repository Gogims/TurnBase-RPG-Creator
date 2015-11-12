using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipppable
{
    /// <summary>
    /// Porcentaje que el arma tiene para realizar su ataque
    /// </summary>
    public float HitRate;

    /// <summary>
    /// Cantidad de golpes que hace el arma por ataque
    /// </summary>
    public int NumberHit;

    /// <summary>
    /// Tipo de arma: Espada, Lanza, Hacha, etc.
    /// </summary>
    public WeaponType Type;

    public enum WeaponType
    {
        Axe,
        Sword,
        Spear
    };
}