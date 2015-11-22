using System;
using System.Collections.Generic;
using UnityEngine;

public class Troop : RPGElement
{
    /// <summary>
    /// El fondo de arriba del combate
    /// </summary>
    public Texture BackgroundTop;

    /// <summary>
    /// El fondo de abajo del combate
    /// </summary>
    public Texture BackgroundBottom;

    /// <summary>
    /// Lista de enemigos de la batalla
    /// </summary>
    public List<EnemyBattle> Enemies;

    public Troop()
    {
        Enemies = new List<EnemyBattle>();        
    }
}

[Serializable]
public class EnemyBattle
{
    /// <summary>
    /// Enemigo del battle map
    /// </summary>
    public AbstractEnemy Enemy;

    /// <summary>
    /// Position donde se presenta el enemigo
    /// </summary>
    public Rect EnemyPosition;

    public EnemyBattle()
    {
        Enemy = new AbstractEnemy();
        EnemyPosition = new Rect();
    }
}