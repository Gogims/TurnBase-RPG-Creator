using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Item : RPGElement
{
    /// <summary>
    /// Descripción del equipamiento
    /// </summary>
    public string Description = string.Empty;

    /// <summary>
    /// El valor que tiene el equipamiento
    /// </summary>
    public int Price;
}