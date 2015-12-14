﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PcInput
{
	//Retorna true si el boton A es presionado de lo contrario retorna false.
	public  static bool A(){
		return Input.GetKeyDown ("z");
	}	
	//Retorna true si el boton B es presionado de lo contrario retorna false.
	public static bool B(){
        return Input.GetKeyDown("x");
	}
	//Retorna true si el boton de moverse hacia arriba es presionado de lo contrario retorna false.
	public static bool Up(){
		return Input.GetKey("up");
	}
	//Retorna true si el boton de moverse hacia abajo es presionado de lo contrario retorna false.
	public static bool Down () {
		return Input.GetKey("down");
	}
	//Retorna true si el boton de moverse hacia la derecha es presionado de lo contrario retorna false.
	public static bool Right (){
		return Input.GetKey("right");
	}
	//Retorna true si el boton de moverse hacia la izquierda es presionado de lo contrario retorna false.
	public static bool Left() {
		return Input.GetKey("left");
	}
	//Retorna true si el boton de pausa es presionado de lo contrario retorna false.
	public static bool Pause(){
        return Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("enter");
    }
	//Retorna true si el boton de select es presionado de lo contrario retorna false.
	public static bool Select(){
		return Input.GetKeyUp ("space");
	}

    //Retorna true si el boton A es soltado.
    public static bool AUp()
    {
        return Input.GetKeyUp("z");
    }
    //Retorna true si el boton B es soltado.
    public static bool BUp()
    {
        return Input.GetKeyUp("x");
    }
    //Retorna true si el boton de moverse hacia arriba es soltado.
    public static bool UpUp()
    {
        return Input.GetKeyUp("up");
    }
    //Retorna true si el boton de moverse hacia abajo es soltado.
    public static bool DownUp()
    {
        return Input.GetKeyUp("down");
    }
    //Retorna true si el boton de moverse hacia la derecha es soltado.
    public static bool RightUp()
    {
        return Input.GetKeyUp("right");
    }
    //Retorna true si el boton de moverse hacia la izquierda es soltado.
    public static bool LeftUp()
    {
        return Input.GetKeyUp("left");
    }
    //Retorna true si el boton de pausa es soltado.
    public static bool PauseUp()
    {
        return Input.GetKeyUp(KeyCode.Return);
    }
    //Retorna true si el boton de select es soltado.
    public static bool SelectUp()
    {
        return Input.GetKeyUp("space");
    }
    /// <summary>
    /// Busca cuanto se movió verticalmente
    /// </summary>
    /// <returns>El movimiento verticalmente</returns>
    public static float GetVertical()
    {
        return Input.GetAxis("Vertical");
    }
    /// <summary>
    /// Busca cuanto se movió horizontalmente
    /// </summary>
    /// <returns>El movimiento horizontalmente</returns>
    public static float GetHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
}
