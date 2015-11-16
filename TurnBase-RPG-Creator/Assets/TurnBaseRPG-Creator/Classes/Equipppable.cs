using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Equipppable : RPGElement
{
    /// <summary>
    /// Descripción del equipamiento
    /// </summary>
    public string Description;

    /// <summary>
    /// Nivel mínimo que se necesita para equipar
    /// </summary>
    public int MinLevel;

    /// <summary>
    /// El valor que tiene el equipamiento
    /// </summary>
    public int Price;    

    /// <summary>
    /// Str, DEX, Int, etc.
    /// </summary>
    public Attribute Stats;

    /// <summary>
    /// Estado que aplica el equipamiento
    /// </summary>
    public AbstractState State;

    /// <summary>
    /// Probabilidad de aplicar el estado
    /// </summary>
    public float PercentageState;

    /// <summary>
    /// Icono del equipamiento
    /// </summary>
    public Sprite Image;

    public Equipppable()
    {
        Stats = new Attribute();
        State = new AbstractState();
    }    

    
}