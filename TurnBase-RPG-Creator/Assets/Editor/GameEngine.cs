using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Presenta todos los menu del Game Engine
/// </summary>
public class GameEngine : EditorWindow {	

	/// <summary>
	/// Presenta el mantenimiento de jugadores
	/// </summary>
	[MenuItem("RPG-Creator/Player")]
	public static void ShowPlayer()
	{		
        //Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(PlayerUI));
	}
	
	/// <summary>
	/// Presenta el Mantenimiento de las armas
	/// </summary>
	[MenuItem("RPG-Creator/Weapon")]
	public static void ShowWeapon()
	{
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<WeaponUI>();
        window.Init();
        window.Show();
	}

    /// <summary>
	/// Presenta el Mantenimiento de las armas
	/// </summary>
	[MenuItem("RPG-Creator/Get Weapon")]
    public static void GetWeapon()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<GetWeapon>();
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta el logo
    /// </summary>
    [MenuItem("RPG-Creator/Image")]
	public static void ShowImage()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(UploadImage));
	}
}
