using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Clase que se encarga de verificar los inputs del juego. 
public class ProxyInput
{
    /// <summary>
    /// Instancia de la clase input.
    /// </summary>
    public static ProxyInput instance;
    /// <summary>
    /// Delegado que se encarga de verificar si el botón B es presionado.
    /// </summary>
    /// <returns>True si el boton B es presionado</returns>
    public delegate bool ActionB();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón A es presionado.
    /// </summary>
    /// <returns>True si el boton A es presionado</returns>
    public delegate bool ActionA();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia arriba es presionado.
    /// </summary>
    /// <returns>True si el boton hacia arriba es presionado</returns>
    public delegate bool ActionUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia abajo es presionado.
    /// </summary>
    /// <returns>True si el boton hacia abajo es presionado</returns>
    public delegate bool ActionDown();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia derecha es presionado.
    /// </summary>
    /// <returns>True si el boton hacia derecha es presionado</returns>
    public delegate bool ActionRight();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia izquierda es presionado.
    /// </summary>
    /// <returns>True si el boton hacia izquierda es presionado</returns>
    public delegate bool ActionLeft();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de pausa es presionado.
    /// </summary>
    /// <returns>True si el boton de pausa es presionado</returns>
    public delegate bool ActionPause ();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de select es presionado.
    /// </summary>
    /// <returns>True si el boton de select es presionado</returns>
    public delegate bool ActionSelect ();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón B es presionado.
    /// </summary>
    /// <returns>True si el boton B es presionado</returns>
    public delegate bool ActionBUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón A es presionado.
    /// </summary>
    /// <returns>True si el boton A es presionado</returns>
    public delegate bool ActionAUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia arriba es presionado.
    /// </summary>
    /// <returns>True si el boton hacia arriba es presionado</returns>
    public delegate bool ActionUpUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia abajo es presionado.
    /// </summary>
    /// <returns>True si el boton hacia abajo es presionado</returns>
    public delegate bool ActionDownUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia derecha es presionado.
    /// </summary>
    /// <returns>True si el boton hacia derecha es presionado</returns>
    public delegate bool ActionRightUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de moverse hacia izquierda es presionado.
    /// </summary>
    /// <returns>True si el boton hacia izquierda es presionado</returns>
    public delegate bool ActionLeftUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de pausa es presionado.
    /// </summary>
    /// <returns>True si el boton de pausa es presionado</returns>
    public delegate bool ActionPauseUp();
    /// <summary>
    /// Delegado que se encarga de verificar si el botón de select es presionado.
    /// </summary>
    /// <returns>True si el boton de select es presionado</returns>
    public delegate bool ActionSelectUp();
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón B es presionado
    /// </summary>
    public event ActionB OnB;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón A es presionado
    /// </summary>
    public event ActionA OnA;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia arriba es presionado
    /// </summary>
    public event ActionUp OnUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia abajo es presionado
    /// </summary>
    public event ActionDown OnDown;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia derecha es presionado
    /// </summary>
    public event ActionRight OnRight;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia izquierda es presionado
    /// </summary>
    public event ActionLeft OnLeft;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón pausa es presionado
    /// </summary>
    public event ActionPause OnPause;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón select es presionado
    /// </summary>
    public event ActionSelect OnSelect;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón B es presionado
    /// </summary>
    public event ActionB OnBUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón A es presionado
    /// </summary>
    public event ActionA OnAUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia arriba es presionado
    /// </summary>
    public event ActionUp OnUpUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia abajo es presionado
    /// </summary>
    public event ActionDown OnDownUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia derecha es presionado
    /// </summary>
    public event ActionRight OnRightUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón hacia izquierda es presionado
    /// </summary>
    public event ActionLeft OnLeftUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón pausa es presionado
    /// </summary>
    public event ActionPause OnPauseUp;
    /// <summary>
    /// Variable que se encarga de llamar la función de cuando el botón select es presionado
    /// </summary>
    public event ActionSelect OnSelectUp;
    /// <summary>
    /// Retorna la instancia de la clase.
    /// </summary>
    private ProxyInput (){}    
    /// <summary>
    /// Busca la instancia del proxy, en caso de que no exista, la construye (una sola vez)
    /// </summary>
    /// <returns>Retorna la instancia de la clase ProxyInput. Dependiendo del ambiente de ejecución</returns>
    public static ProxyInput GetInstance()
    {
		if (instance == null)
        {
			instance = new ProxyInput();

            if (!Application.isMobilePlatform)
            {
                instance.OnA = PcInput.A;
                instance.OnB = PcInput.B;
                instance.OnSelect = PcInput.Select;
                instance.OnPause = PcInput.Pause;
                instance.OnDown = PcInput.Down;
                instance.OnLeft = PcInput.Left;
                instance.OnRight = PcInput.Right;
                instance.OnUp = PcInput.Up;
                instance.OnAUp = PcInput.AUp;
                instance.OnBUp = PcInput.BUp;
                instance.OnSelectUp = PcInput.SelectUp;
                instance.OnPauseUp = PcInput.PauseUp;
                instance.OnDownUp = PcInput.DownUp;
                instance.OnLeftUp = PcInput.LeftUp;
                instance.OnRightUp = PcInput.RightUp;
                instance.OnUpUp = PcInput.UpUp;
                
            }
            else
            {
                instance.OnA = PhoneInput.A;
                instance.OnB = PhoneInput.B;
                instance.OnSelect = PhoneInput.Select;
                instance.OnPause = PhoneInput.Pause;
                instance.OnDown = PhoneInput.Down;
                instance.OnLeft = PhoneInput.Left;
                instance.OnRight = PhoneInput.Right;
                instance.OnUp = PhoneInput.Up;
                instance.OnAUp = PhoneInput.AUp;
                instance.OnBUp = PhoneInput.BUp;
                instance.OnSelectUp = PhoneInput.SelectUp;
                instance.OnPauseUp = PhoneInput.PauseUp;
                instance.OnDownUp = PhoneInput.DownUp;
                instance.OnLeftUp = PhoneInput.LeftUp;
                instance.OnRightUp = PhoneInput.RightUp;
                instance.OnUpUp = PhoneInput.UpUp;
            }
            instance.OnA = PhoneInput.A;
            instance.OnB = PhoneInput.B;
            instance.OnSelect = PhoneInput.Select;
            instance.OnPause = PhoneInput.Pause;
            instance.OnDown = PhoneInput.Down;
            instance.OnLeft = PhoneInput.Left;
            instance.OnRight = PhoneInput.Right;
            instance.OnUp = PhoneInput.Up;
            instance.OnAUp = PhoneInput.AUp;
            instance.OnBUp = PhoneInput.BUp;
            instance.OnSelectUp = PhoneInput.SelectUp;
            instance.OnPauseUp = PhoneInput.PauseUp;
            instance.OnDownUp = PhoneInput.DownUp;
            instance.OnLeftUp = PhoneInput.LeftUp;
            instance.OnRightUp = PhoneInput.RightUp;
            instance.OnUpUp = PhoneInput.UpUp;

            


        }
		return instance;
	}

    /// <summary>
    /// Función para detectar botón A presionado
    /// </summary>
    /// <returns>True si el boton A es presionado</returns>
    public bool A()
    {
		return OnA();
	}

    /// <summary>
    /// Función para detectar botón B presionado
    /// </summary>
    /// <returns>True si el boton B es presionado</returns>
    public bool B()
    {
		return OnB();
	}

    /// <summary>
    /// Función para detectar botón hacia arriba presionado
    /// </summary>
    /// <returns>True si el boton hacia arriba es presionado</returns>
    public bool Up()
    {
		return OnUp();
	}

    /// <summary>
    /// Función para detectar botón hacia abajo presionado
    /// </summary>
    /// <returns>True si el boton hacia abajo es presionado</returns>
    public bool Down()
    {
		return OnDown();
	}

    /// <summary>
    /// Función para detectar botón hacia derecha presionado
    /// </summary>
    /// <returns>True si el boton hacia derecha es presionado</returns>
    public bool Right()
    {
		return OnRight();
	}

    /// <summary>
    /// Función para detectar botón hacia izquierda presionado
    /// </summary>
    /// <returns>True si el boton hacia izquierda es presionado</returns>
    public bool Left()
    {
		return OnLeft();
	}

    /// <summary>
    /// Función para detectar botón pausa presionado
    /// </summary>
    /// <returns>True si el boton pausa es presionado</returns>
    public bool Pause()
    {
		return OnPause();
	}

    /// <summary>
    /// Función para detectar botón select presionado
    /// </summary>
    /// <returns>True si el boton select es presionado</returns>
    public bool Select()
    {
		return OnSelect();
	}

    /// <summary>
    /// Función para detectar si el personaje está en movimiento
    /// </summary>
    /// <returns>True si está en movimiento</returns>
    public bool Movement()
    {
        return OnDown() || OnRight() || OnLeft() || OnUp();
    }

    /// <summary>
    /// Función para detectar cuanto se movió horizontalmente
    /// </summary>
    /// <returns>Cuanto se movió horizontalmente</returns>
    public float GetHorizontal()
    {
        float horizontal = 0;

        if (Application.isMobilePlatform)
        {
            horizontal = PhoneInput.GetHorizontal();
        }
        else
        {
            horizontal = PcInput.GetHorizontal();
        }
        horizontal = PhoneInput.GetHorizontal();

        return horizontal;
    }
    /// <summary>
    /// Función para detectar cuanto se movió verticalmente
    /// </summary>
    /// <returns>Cuanto se movió verticalmente</returns>
    public float GetVertical()
    {
        float vertical = 0;

        if (Application.isMobilePlatform)
        {
            vertical = PhoneInput.GetVertical();
        }
        else
        {
            vertical = PcInput.GetVertical();
        }
        vertical = PhoneInput.GetVertical();

        return vertical;
    }
    /// <summary>
    /// Función para detectar botón A presionado
    /// </summary>
    /// <returns>True si el boton A es presionado</returns>
    public bool AUp()
    {
        return OnAUp();
    }

    /// <summary>
    /// Función para detectar botón B presionado
    /// </summary>
    /// <returns>True si el boton B es presionado</returns>
    public bool BUp()
    {
        return OnBUp();
    }

    /// <summary>
    /// Función para detectar botón hacia arriba presionado
    /// </summary>
    /// <returns>True si el boton hacia arriba es presionado</returns>
    public bool UpUp()
    {
        return OnUpUp();
    }

    /// <summary>
    /// Función para detectar botón hacia abajo presionado
    /// </summary>
    /// <returns>True si el boton hacia abajo es presionado</returns>
    public bool DownUp()
    {
        return OnDownUp();
    }

    /// <summary>
    /// Función para detectar botón hacia derecha presionado
    /// </summary>
    /// <returns>True si el boton hacia derecha es presionado</returns>
    public bool RightUp()
    {
        return OnRightUp();
    }

    /// <summary>
    /// Función para detectar botón hacia izquierda presionado
    /// </summary>
    /// <returns>True si el boton hacia izquierda es presionado</returns>
    public bool LeftUp()
    {
        return OnLeftUp();
    }

    /// <summary>
    /// Función para detectar botón pausa presionado
    /// </summary>
    /// <returns>True si el boton pausa es presionado</returns>
    public bool PauseUp()
    {
        return OnPauseUp();
    }

    /// <summary>
    /// Función para detectar botón select presionado
    /// </summary>
    /// <returns>True si el boton select es presionado</returns>
    public bool SelectUp()
    {
        return OnSelectUp();
    }

}