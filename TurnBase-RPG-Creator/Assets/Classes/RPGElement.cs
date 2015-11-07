﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RPGElement : MonoBehaviour
{
    /// <summary>
    /// Identificador que no se repite
    /// </summary>
    public int Id;

    /// <summary>
    /// Nombre del elemento
    /// </summary>
    public string Name;
}