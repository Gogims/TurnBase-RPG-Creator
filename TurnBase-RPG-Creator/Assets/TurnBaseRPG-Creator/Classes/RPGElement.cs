using System;
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
    public string Name = string.Empty;
<<<<<<< HEAD
    /// <summary>
    /// Icono del elemento
    /// </summary>
    public Sprite Icon; 

=======

    /// <summary>
    /// Imagen del elemento
    /// </summary>
    public Sprite Image;
>>>>>>> ae91eb4481e0d5c3116edc2df2ba120c491946dd
}