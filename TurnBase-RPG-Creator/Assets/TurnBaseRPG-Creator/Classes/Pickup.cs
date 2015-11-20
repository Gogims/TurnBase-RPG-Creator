using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Representa un item en el mapa.
/// </summary>
public class Pickup : RPGElement {
    /// <summary>
    /// Imagen que representa el item.
    /// </summary>
    public Sprite Image;
    /// <summary>
    /// items que da el objeto.
    /// </summary>
    public AbstractUsable ItemUsable;
}
