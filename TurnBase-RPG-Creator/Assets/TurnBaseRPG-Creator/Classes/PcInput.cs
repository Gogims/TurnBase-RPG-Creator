using UnityEngine;
using System.Collections;

public class PcInput {

	//Retorna true si el boton A es presionado de lo contrario retorna false.
	public  static bool A(){
		return Input.GetKey ("z");
	}	
	//Retorna true si el boton B es presionado de lo contrario retorna false.
	public static bool B(){
		return Input.GetKey ("x");
	}
	//Retorna true si el boton de moverse hacia arriba es presionado de lo contrario retorna false.
	public static bool Up(){
		return Input.GetKey ("up");
	}
	//Retorna true si el boton de moverse hacia abajo es presionado de lo contrario retorna false.
	public static bool Down () {
		return Input.GetKey ("down");
	}
	//Retorna true si el boton de moverse hacia la derecha es presionado de lo contrario retorna false.
	public static bool Right (){
		return Input.GetKey ("right");
	}
	//Retorna true si el boton de moverse hacia la izquierda es presionado de lo contrario retorna false.
	public static bool Left() {
		return Input.GetKey ("left");
	}
	//Retorna true si el boton de pausa es presionado de lo contrario retorna false.
	public static bool Pause(){
		return Input.GetKey ("enter");
	}
	//Retorna true si el boton de select es presionado de lo contrario retorna false.
	public static bool Select(){
		return Input.GetKey ("space");
	}
}
