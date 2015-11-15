using System;
using UnityEngine;
using System.Collections.Generic;

public class Actor : RPGElement
{
    /// <summary>
    /// Atributos actuales del actor
    /// </summary>
    public Attribute Stats;
    /// <summary>
    /// Nivel actual del actor
    /// </summary>
    public int Level;
    /// <summary>
    /// Breve descripcion del actor
    /// </summary>
    public string Description;
    /// <summary>
    /// Cantidad de vida disponible por el actor
    /// </summary>
    public int HP;
    /// <summary>
    /// Cantidad de mana disponible por el actor
    /// </summary>
    public int MP;

    /// <summary>
    /// Listado de las animaciones para caminar hacia abajo
    /// </summary>
    public List<Sprite> downSprites;
    /// <summary>
    /// Listado de las animaciones para caminar hacia la izquierda
    /// </summary>
    public List<Sprite> leftSprites;
    /// <summary>
    /// Listado de las animaciones para caminar hacia arriba
    /// </summary>
    public List<Sprite> upSprites;
    /// <summary>
    /// Listado de las animaciones para caminar hacia la derecha
    /// </summary>
    public List<Sprite> rightSprites;

    /// <summary>
    /// El componente físico 
    /// </summary>
    private Rigidbody2D rb2D;

    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {
        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        if (rb2D == null)
        {
            rb2D = gameObject.AddComponent<Rigidbody2D>();
        }

        rb2D.isKinematic = true;
    }

    public Actor ()
	{
        Stats = new Attribute();        
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

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    protected bool Move(int xDir, int yDir)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = new Vector2(xDir, yDir);

        //If nothing was hit, start Movement
        rb2D.velocity = end;


        //Return true to say that Move was successful
        return true;
    }
}
