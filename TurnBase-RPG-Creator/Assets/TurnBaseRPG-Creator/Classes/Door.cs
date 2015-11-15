using UnityEngine;
using System.Collections;
/// <summary>
/// Representa una puerta en el juego.
/// </summary>
public class Door: RPGElement  {
    /// <summary>
    /// Nombre del mapa que carga cuando se ingresa a la puerta.
    /// </summary>
    public string InMap = string.Empty;
    /// <summary>
    /// Nombre del mapa que carga cuando se sale por la puerta.
    /// </summary>
    public string OutMap = string.Empty;
    /// <summary>
    /// Sprite que representa la puerta.
    /// </summary>
    public Sprite Image;

}
