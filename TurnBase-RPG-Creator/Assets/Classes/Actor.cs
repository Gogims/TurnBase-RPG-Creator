using System;
using UnityEngine;
using System.Collections.Generic;

public class Actor : RPGElement
{
    public Attribute Stats;    

    public int Level;

    public string Description;

    public int HP;

    public int MP;

    public Sprite Image;

    public List<Sprite> downSprites;
    public List<Sprite> leftSprites;
    public List<Sprite> upSprites;
    public List<Sprite> rightSprites;

    public Weapon MainHand;
    public Armor Head;
    public Armor UpperBody;
    public Armor LowerBody;
    public List<Armor> Accesories;

    public Actor ()
	{
        Stats = new Attribute();
        downSprites = new List<Sprite>();
        leftSprites = new List<Sprite>();
        upSprites = new List<Sprite>();
        rightSprites = new List<Sprite>();
        MainHand = new Weapon();
        Head = new Armor();
        UpperBody = new Armor();
        LowerBody = new Armor();
        Accesories = new List<Armor>();
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
