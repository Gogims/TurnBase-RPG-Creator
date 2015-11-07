using UnityEngine;
using System.Collections;
/// <summary>
/// Representa una imagen para RPG
/// </summary>
public class RPGImage  {
    /// <summary>
    /// Imagen
    /// </summary>
    public Texture2D texture { get; set; }
    /// <summary>
    /// Nombre de la imagen
    /// </summary>
	public string name { set; get; }
    /// <summary>
    /// Tipo de imagen ( Tiles,Wall,etc.)
    /// </summary>
	public int type { set; get; } 	

}
