using System;
using UnityEngine;
using System.Collections.Generic;

public class Actor : RPGElement
{
    //Protected, virtual functions can be overridden by inheriting classes.
    protected virtual void Start()
    {

    }      

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    protected bool Move(int xDir, int yDir)
    {
        var rb2D = GetComponent<Rigidbody2D>();
        
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
    /// <summary>
    /// Listado de estados que posee el actor actualmente
    /// </summary>
    public List<AbstractState> States;
    /// <summary>
    /// Si su turno en el modo combate terminó
    /// </summary>
    public bool TurnEnded;

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
        States = new List<AbstractState>();        
    }

    public int TotalDamage()
    {
        int AttackDamage = Stats.Attack;

        AttackDamage += MainHand.Stats.Attack;
        AttackDamage += CheckArmor(Helmet).Attack;
        AttackDamage += CheckArmor(Body).Attack;
        AttackDamage += CheckArmor(Feet).Attack;
        AttackDamage += CheckArmor(Ring).Attack;
        AttackDamage += CheckArmor(Necklace).Attack;

        return AttackDamage;
    }

    public int TotalDefense()
    {
        int Defense = Stats.Defense;

        Defense += MainHand.Stats.Defense;
        Defense += CheckArmor(Helmet).Defense;
        Defense += CheckArmor(Body).Defense;
        Defense += CheckArmor(Feet).Defense;
        Defense += CheckArmor(Ring).Defense;
        Defense += CheckArmor(Necklace).Defense;

        return Defense;
    }

    public int TotalMagicDefense()
    {
        int MagicDefense = Stats.MagicDefense;

        MagicDefense += MainHand.Stats.MagicDefense;
        MagicDefense += CheckArmor(Helmet).MagicDefense;
        MagicDefense += CheckArmor(Body).MagicDefense;
        MagicDefense += CheckArmor(Feet).MagicDefense;
        MagicDefense += CheckArmor(Ring).MagicDefense;
        MagicDefense += CheckArmor(Necklace).MagicDefense;

        return MagicDefense;
    }

    public int TotalMagicDamage()
    {
        int MagicDamage = Stats.Magic;

        MagicDamage += MainHand.Stats.Magic;
        MagicDamage += CheckArmor(Helmet).Magic;
        MagicDamage += CheckArmor(Body).Magic;
        MagicDamage += CheckArmor(Feet).Magic;
        MagicDamage += CheckArmor(Ring).Magic;
        MagicDamage += CheckArmor(Necklace).Magic;

        return MagicDamage;
    }

    private Attribute CheckArmor(AbstractArmor armorPiece)
    {
        if (armorPiece == null) return new Attribute();

        return armorPiece.Stats;
    }
}