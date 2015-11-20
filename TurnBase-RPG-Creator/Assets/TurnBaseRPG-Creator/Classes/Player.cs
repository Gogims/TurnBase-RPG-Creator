using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    /// <summary>
    /// Información del jugador
    /// </summary>
    public AbstractPlayer Data;

    public Player()
    {
        Data = new AbstractPlayer();
    }

    void Update()
    {
        MoveDirection(ProxyInput.GetInstance().Down(), "down", 0);
        MoveDirection(ProxyInput.GetInstance().Left(), "left", 0.34f);
        MoveDirection(ProxyInput.GetInstance().Up(), "up", 0.67f);
        MoveDirection(ProxyInput.GetInstance().Right(), "right", 1);
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
    /// <summary>
    /// Listado de estados que posee el actor actualmente
    /// </summary>
    public List<AbstractState> States;
}