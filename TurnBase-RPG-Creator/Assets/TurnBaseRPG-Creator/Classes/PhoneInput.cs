using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PhoneInput
{
    //Retorna true si el boton A es presionado de lo contrario retorna false.
    public static bool A()
    {
        return CrossPlatformInputManager.GetButtonUp("A");
    }
    //Retorna true si el boton B es presionado de lo contrario retorna false.
    public static bool B()
    {
        return CrossPlatformInputManager.GetButtonUp("B");
    }
    //Retorna true si el boton de moverse hacia arriba es presionado de lo contrario retorna false.
    public static bool Up()
    {
        return CrossPlatformInputManager.GetAxis("Vertical") > 0;
    }
    //Retorna true si el boton de moverse hacia abajo es presionado de lo contrario retorna false.
    public static bool Down()
    {
        return CrossPlatformInputManager.GetAxis("Vertical") < 0;
    }
    //Retorna true si el boton de moverse hacia la derecha es presionado de lo contrario retorna false.
    public static bool Right()
    {
        return CrossPlatformInputManager.GetAxis("Horizontal") > 0;
    }
    //Retorna true si el boton de moverse hacia la izquierda es presionado de lo contrario retorna false.
    public static bool Left()
    {
        return CrossPlatformInputManager.GetAxis("Horizontal") < 0;
    }
    //Retorna true si el boton de pausa es presionado de lo contrario retorna false.
    public static bool Pause()
    {
        return Input.GetKeyUp("enter");
    }
    //Retorna true si el boton de select es presionado de lo contrario retorna false.
    public static bool Select()
    {
        return Input.GetKeyUp("space");
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
}
