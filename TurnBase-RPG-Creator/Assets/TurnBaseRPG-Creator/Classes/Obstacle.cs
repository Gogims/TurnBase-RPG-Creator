using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Clase que represetan un obstaculo
/// </summary>
public class Obstacle: MapObject
{
    /// <summary>
    /// Hp del obstaculo
    /// </summary>
    public int hp;

    public AudioClip Sound;

    public Constant.ObstacleType Type;

    public GameObject Removable;

    public bool Switched=true;

    //DamageWall is called when the player attacks a wall.
    public void DamageWall(int loss)
    {
        //Subtract loss from hit point total.
        hp -= loss;

        //If hit points are less than or equal to zero:
        if (hp <= 0)
            //Disable the gameObject.
            gameObject.SetActive(false);
    }    

    void Update()
    {
        if (Removable != null)
        {
            Removable.SetActive(Switched);
        }
    }
}

