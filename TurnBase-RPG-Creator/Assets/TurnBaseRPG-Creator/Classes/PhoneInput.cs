using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PhoneInput
{
    //Retorna true si el boton A es presionado de lo contrario retorna false.
    public static bool A()
    {
        return CrossPlatformInputManager.GetButton("A");
    }
    //Retorna true si el boton B es presionado de lo contrario retorna false.
    public static bool B()
    {
        return CrossPlatformInputManager.GetButton("B");
    }
    //Retorna true si el boton de moverse hacia arriba es presionado de lo contrario retorna false.
    public static bool Up()
    {
        return CrossPlatformInputManager.GetButton("Up");
    }
    //Retorna true si el boton de moverse hacia abajo es presionado de lo contrario retorna false.
    public static bool Down()
    {
        return CrossPlatformInputManager.GetButton("Down");
    }
    //Retorna true si el boton de moverse hacia la derecha es presionado de lo contrario retorna false.
    public static bool Right()
    {
        return CrossPlatformInputManager.GetButton("Right");
    }
    //Retorna true si el boton de moverse hacia la izquierda es presionado de lo contrario retorna false.
    public static bool Left()
    {
        return CrossPlatformInputManager.GetButton("Left");
    }
    //Retorna true si el boton de pausa es presionado de lo contrario retorna false.
    public static bool Pause()
    {
        return CrossPlatformInputManager.GetButton("Start");
    }
    //Retorna true si el boton de select es presionado de lo contrario retorna false.
    public static bool Select()
    {
        return CrossPlatformInputManager.GetButton("Select");
    }

    //Retorna true si el boton A es soltado.
    public static bool AUp()
    {
        return CrossPlatformInputManager.GetButtonUp("A");
    }
    //Retorna true si el boton B es soltado.
    public static bool BUp()
    {
        return CrossPlatformInputManager.GetButtonUp("B");
    }
    //Retorna true si el boton de moverse hacia arriba es soltado.
    public static bool UpUp()
    {
        return CrossPlatformInputManager.GetButtonUp("Up");
    }
    //Retorna true si el boton de moverse hacia abajo es soltado.
    public static bool DownUp()
    {
        return CrossPlatformInputManager.GetButtonUp("Down");
    }
    //Retorna true si el boton de moverse hacia la derecha es soltado.
    public static bool RightUp()
    {
        return CrossPlatformInputManager.GetButtonUp("Right");
    }
    //Retorna true si el boton de moverse hacia la izquierda es soltado.
    public static bool LeftUp()
    {
        return CrossPlatformInputManager.GetButtonUp("Left");
    }
    //Retorna true si el boton de pausa es soltado.
    public static bool PauseUp()
    {
        return CrossPlatformInputManager.GetButtonUp("A");
    }
    //Retorna true si el boton de select es soltado.
    public static bool SelectUp()
    {
        return Input.GetKeyUp("Start");
    }
    /// <summary>
    /// Busca cuanto se movió verticalmente
    /// </summary>
    /// <returns>El movimiento verticalmente</returns>
    public static float GetHorizontal()
    {
        float value = 0;

        if (Right())
        {
            value = 1;
        }
        else if (Left())
        {
            value = -1;
        }

        return value;
    }
    /// <summary>
    /// Busca cuanto se movió horizontalmente
    /// </summary>
    /// <returns>El movimiento horizontalmente</returns>
    public static float GetVertical()
    {
        float value = 0;

        if (Up())
        {
            value = 1;
        }
        else if (Down())
        {
            value = -1;
        }

        return value;
    }
}
