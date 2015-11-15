using UnityEngine;
using System.Collections;
/// <summary>
/// Representa una imagen para RPG
/// </summary>
public class RPGImage:RPGElement  {
    /// <summary>
    /// Imagen
    /// </summary>
    public Texture2D Texture { get; set; }
    /// <summary>
    /// Tipo de imagen ( Tiles,Wall,etc.)
    /// </summary>
	public int Type { set; get; } 	

}
