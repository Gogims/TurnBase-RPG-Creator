using System;
using System.Collections.Generic;
using UnityEngine;

public class Usable : RPGElement
{
    public AbstractUsable Data;

    public Usable()
    {
        Data = new AbstractUsable();
    }
}

[Serializable]
public class AbstractUsable : Item
{
    /// <summary>
    /// Si se gasta el item luego de utilizar
    /// </summary>
    public bool Consumeable;
    /// <summary>
    /// A cuantas aliados/enemigos afecta el item
    /// </summary>
    public Constant.AOE AreaOfEffect;
    /// <summary>
    /// Cuando está disponible el item para usar
    /// </summary>
    public Constant.ItemAvailable Available;
    /// <summary>
    /// Es un item clave?
    /// </summary>
    public bool KeyItem;
    /// <summary>
    /// Porcentaje para que el item sea exitoso al utilizar
    /// </summary>
    public float HitRate;
    /// <summary>
    /// Defense= Remueve Efecto. Offense = Agrega Efecto
    /// </summary>
    public Constant.OffenseDefense StateType;
    /// <summary>
    /// Listado de efectos que remueve/agrega al objetivo
    /// </summary>
    public List<AbstractState> States;
    /// <summary>
    /// Cual atributo se mejora/empeora al utilizar el item
    /// </summary>
    public Constant.Attribute Attribute;
    /// <summary>
    /// Cuanto aumentará de un atributo específico
    /// </summary>
    public int Power;

    public AbstractUsable()
    {
        States = new List<AbstractState>();
    }
}