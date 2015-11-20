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
    /// 0 = Izquierdo, 1 = Centro, 2 = Derecho
    /// </summary>
    List<AbstractEnemy> Enemies;

    public Troop()
    {
        Enemies = new List<AbstractEnemy>();
    }
}