using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Equipppable : RPGElement
{
    /// <summary>
    /// Descripción del equipamiento
    /// </summary>
    public string Description;

    /// <summary>
    /// Nivel mínimo que se necesita para equipar
    /// </summary>
    public int MinLevel;

    /// <summary>
    /// El valor que tiene el equipamiento
    /// </summary>
    public int Price;

    /// <summary>
    /// Imagen del equipamiento
    /// </summary>
    public Sprite Image;

    /// <summary>
    /// Str, DEX, Int, etc.
    /// </summary>
    public Attribute Stats;

    /// <summary>
    /// Estado que aplica el equipamiento
    /// </summary>
    public AbstractState State;

    /// <summary>
    /// Probabilidad de aplicar el estado
    /// </summary>
    public float PercentageState;

    public Equipppable()
    {
        Stats = new Attribute();
        State = new AbstractState();
    }    

    /// <summary>
    /// Crea el marco de la imagen del equipamiento
    /// </summary>
    /// <returns>Rectángulo con sus coordenadas definidas</returns>
    public Rect GetTextureCoordinate()
    {
        return new Rect(Image.textureRect.x / Image.texture.width, 
                        Image.textureRect.y / Image.texture.height, 
                        Image.textureRect.width / Image.texture.width, 
                        Image.textureRect.height / Image.texture.height);
    }
}