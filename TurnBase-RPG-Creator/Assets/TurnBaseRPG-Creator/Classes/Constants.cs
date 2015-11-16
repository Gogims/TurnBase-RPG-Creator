using UnityEngine;
using System.Collections;
/// <summary>
/// Constantes de TurnBase-RPG-Creator.
/// </summary>
public class Constant{
	/// <summary>
	/// La altura maxima del mapa
	/// </summary>
	public const int MAX_MAP_HEIGTH = 10;
	/// <summary>
	/// Ancho maximo del mapa
	/// </summary>
	public const int MAX_MAP_WIDTH = 17;
	/// <summary>
	/// Altura Minima del mapa
	/// </summary>
	public const int MIN_MAP_HEIGTH = 5;
	/// <summary>
	/// Ancho Minimo del mapa.
	/// </summary>
	public const int MIN_MAP_WIDTH = 5;
	/// <summary>
	/// Altura de la imagen del RPGInspector
	/// </summary>
	public const int INSPECTOR_IMAGE_HEIGTH = 150;
	/// <summary>
	/// Ancho de la imagen del RPGInspector
	/// </summary>
	public const int INSPECTOR_IMAGE_WIDTH = 150;

    /// <summary>
    /// Crea el marco de la imagen del equipamiento
    /// </summary>
    /// <returns>Rectángulo con sus coordenadas definidas</returns>
    public static Rect GetTextureCoordinate(Sprite Image)
    {
        return new Rect(Image.textureRect.x / Image.texture.width,
                        Image.textureRect.y / Image.texture.height,
                        Image.textureRect.width / Image.texture.width,
                        Image.textureRect.height / Image.texture.height);
    }
}
