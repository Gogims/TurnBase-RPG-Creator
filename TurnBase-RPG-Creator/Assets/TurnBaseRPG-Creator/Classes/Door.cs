using UnityEngine;
using System.Collections;
/// <summary>
/// Representa una puerta en el juego.
/// </summary>
public class Door: MapObject
{
    public Door() {
        InMap = new AbstractMap();
    }
    /// <summary>
    /// Define cual es el mapa que se va mostrar al entrar por la puerta
    /// </summary>
    public AbstractMap InMap;
    public float X;
    public float Y;
}
