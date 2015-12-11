using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RPGElement : MonoBehaviour
{
    /// <summary>
    /// Identificador que no se repite
    /// </summary>
    public string Id;
    /// <summary>
    /// Nombre del elemento
    /// </summary>
    public string Name = string.Empty;    
    /// <summary>
    /// Icono del elemento
    /// </summary>
    public Sprite Icon;

    [NonSerialized]
    public Texture Logo = Resources.Load<Texture>("LogoPUCMM");
}