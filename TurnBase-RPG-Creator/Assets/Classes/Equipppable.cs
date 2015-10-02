using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipppable : MonoBehaviour
{
    public int Id;

    public string Name;

    public string Description;

    public int MinLevel;

    public int Price;

    public Sprite Image;

    public Attribute Stats;

    public Equipppable()
    {
        Stats = new Attribute();
    }    

    public Rect GetTextureCoordinate()
    {
        return new Rect(Image.textureRect.x / Image.texture.width, 
                        Image.textureRect.y / Image.texture.height, 
                        Image.textureRect.width / Image.texture.width, 
                        Image.textureRect.height / Image.texture.height);
    }
}