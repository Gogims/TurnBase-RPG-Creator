using UnityEngine;

//Clase que se encarga de verificar los inputs del juego. 
public class ProxyInput
{
	// Instancia de la clase input.
	public static ProxyInput instance;
	//Delegado que se encarga de verificar si el boton B es presionado.
	public delegate bool ActionB();
	//Delegado que se encarga de verificar si el boton A es presionado.
	public delegate bool ActionA();
	//Delegado que se encarga de verificar si el boton de moverse hacia 
	// arriba es presionado.
	public delegate bool ActionUp();
	//Delegado que se encarga de verificar si el boton de moverse hacia 
	// abajo es presionado.
	public delegate bool ActionDown();
	//Delegado que se encarga de verificar si el boton de moverse hacia 
	// derecha es presionado.
	public delegate bool ActionRight();
	//Delegado que se encarga de verificar si el boton de moverse hacia 
	// izquierda es presionado.
	public delegate bool ActionLeft();
	//Delegado que se encarga de verificar si el boton de pausa es presionado
	public delegate bool ActionPause ();
	//Delegado que se encarga de verificar si el boton de select es presionado
	public delegate bool ActionSelect ();
	// Variable que se encarga de llamar la de cuando el boton b es presionado
	public event ActionB OnB;
	// Variable que se encarga de llamar la de cuando el boton a es presionado
	public event ActionA OnA;
	// Variable que se encarga de llamar la de cuando el boton de 
	// moverse hacia arriba es presionado
	public event ActionUp OnUp;
	// Variable que se encarga de llamar la de cuando el boton de 
	// moverse hacia abajo es presionado
	public event ActionDown OnDown;
	// Variable que se encarga de llamar la de cuando el boton de 
	// moverse hacia la derecha es presionado
	public event ActionRight OnRight;
	// Variable que se encarga de llamar la de cuando el boton de 
	// moverse hacia la izquierda es presionado
	public event ActionLeft OnLeft;
	// Variable que se encarga de llamar la de cuando el boton de pausa es presionado
	public event ActionPause OnPause;
	// Variable que se encarga de llamar la de cuando el boton de select es presionado
	public event ActionSelect OnSelect;
	//Retorna la instancia de la clase.
	private ProxyInput (){}
	// Retorna la instancia de la clase ProxyInput. Dependiendo del ambiente de ejecicion
	// Del juego asigna los eventos.
	public static ProxyInput GetInstance(){
		if (instance == null) {
			instance = new ProxyInput();
			if ( !Application.isMobilePlatform){
				instance.OnA = PcInput.A;
				instance.OnB = PcInput.B;
				instance.OnSelect = PcInput.Select;
				instance.OnPause = PcInput.Pause;
				instance.OnDown = PcInput.Down;
				instance.OnLeft = PcInput.Left;
				instance.OnRight = PcInput.Right;
				instance.OnUp = PcInput.Up;
			}
			else {
                instance.OnA = PhoneInput.A;
                instance.OnB = PhoneInput.B;
                instance.OnSelect = PhoneInput.Select;
                instance.OnPause = PhoneInput.Pause;
                instance.OnDown = PhoneInput.Down;
                instance.OnLeft = PhoneInput.Left;
                instance.OnRight = PhoneInput.Right;
                instance.OnUp = PhoneInput.Up;
            }
		}
		return instance;
	}
	//Retorna true si el boton A es presionado de lo contrario retorna false.
	public  bool A(){
		return OnA();
	}	
	//Retorna true si el boton B es presionado de lo contrario retorna false.
	public  bool B(){
		return OnB();
	}
	//Retorna true si el boton de moverse hacia arriba es presionado de lo contrario retorna false.
	public  bool Up(){
		return OnUp();
	}
	//Retorna true si el boton de moverse hacia abajo es presionado de lo contrario retorna false.
	public bool Down () {
		return OnDown();
	}
	//Retorna true si el boton de moverse hacia la derecha es presionado de lo contrario retorna false.
	public bool Right (){
		return OnRight();
	}
	//Retorna true si el boton de moverse hacia la izquierda es presionado de lo contrario retorna false.
	public bool Left() {
		return OnLeft();
	}
	//Retorna true si el boton de pausa es presionado de lo contrario retorna false.
	public bool Pause(){
		return OnPause();
	}
	//Retorna true si el boton de select es presionado de lo contrario retorna false.
	public bool Select(){
		return OnSelect();
	}
    public bool Movement(){
        return OnDown() || OnRight() || OnLeft() || OnUp();
    }
}