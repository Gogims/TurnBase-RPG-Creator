using System;
using UnityEngine;
using System.Collections.Generic;

public class Actor : RPGElement
{
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

[Serializable]
public class AbstractActor
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
    /// Imagen del actor
    /// </summary>
    public Sprite Image;
    /// <summary>
    /// Arma del actor
    /// </summary>
    public AbstractWeapon MainHand;
    /// <summary>
    /// Cabeza del actor
    /// </summary>
    public AbstractArmor Helmet;
    /// <summary>
    /// Pecho y piernas del actor
    /// </summary>
    public AbstractArmor Body;
    /// <summary>
    /// Zapatos del actor
    /// </summary>
    public AbstractArmor Feet;
    /// <summary>
    /// Anillo del actor
    /// </summary>
    public AbstractArmor Ring;
    /// <summary>
    /// Collar del actor
    /// </summary>
    public AbstractArmor Necklace;
    /// <summary>
    /// Clase del actor
    /// </summary>
    public AbstractJob Job;
    /// <summary>
    /// Nombre del actor
    /// </summary>
    public string ActorName = string.Empty;

    public AbstractActor()
    {
        Stats = new Attribute();
        MainHand = new AbstractWeapon();
        Helmet = new AbstractArmor();
        Body = new AbstractArmor();
        Feet = new AbstractArmor();
        Ring = new AbstractArmor();
        Necklace = new AbstractArmor();
        Job = new AbstractJob();
    }
}
