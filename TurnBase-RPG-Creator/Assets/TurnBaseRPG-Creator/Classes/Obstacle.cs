using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Clase que represetan un obstaculo
/// </summary>
public class Obstacle:RPGElement  {
    /// <summary>
    /// Propiedad para especificar de que es un obejto fisico
    /// </summary>
    public Rigidbody2D Body;
    /// <summary>
    /// Hp del obstaculo
    /// </summary>
    public int hp;
    /// <summary>
    /// Imagenes del obstaculo
    /// </summary>
    public Sprite Image;

}

