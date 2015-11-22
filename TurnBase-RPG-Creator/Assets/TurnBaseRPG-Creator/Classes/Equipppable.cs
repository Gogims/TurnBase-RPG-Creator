using System;
using UnityEngine;

[Serializable]
public abstract class Equippable : Item
{
    /// <summary>
    /// Nivel mínimo que se necesita para equipar
    /// </summary>
    public int MinLevel;

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

    public Equippable()
    {
        Stats = new Attribute();
        State = new AbstractState();
    }
}