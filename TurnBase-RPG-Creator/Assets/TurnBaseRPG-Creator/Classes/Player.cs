using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public Inventory Items = new Inventory();
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
    /// Información del jugador
    /// </summary>
    public AbstractPlayer Data;

    public Player()
    {
        Data = new AbstractPlayer();
    }

    protected override void Start()
    {
        Data.InstanceStates();
    }

    void Update()
    {
        MoveDirection(ProxyInput.GetInstance().Down(), "down", 0);
        MoveDirection(ProxyInput.GetInstance().Left(), "left", 0.34f);
        MoveDirection(ProxyInput.GetInstance().Up(), "up", 0.67f);
        MoveDirection(ProxyInput.GetInstance().Right(), "right", 1);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == (int) Constant.MapObjectType.Tile)
        {
            Debug.Log("Tile");
        }
            
    }

    private void MoveDirection(bool input, string direction, float idle)
    {
        var animations = GetComponent<Animator>();

        if (input)
        {
            animations.SetBool(direction, true);
            animations.SetFloat("Idle", idle);            
        }
        else
        {
            animations.SetBool(direction, false);
        }

        PlayerMove();
    }

    private bool PlayerMove()
    {
        int horizontal = 0;
        int vertical = 0;
        
        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (Move(horizontal, vertical))
        {
            return false;
        }

        return true;
    }
}

[Serializable]
public class AbstractPlayer : AbstractActor
{
    
}