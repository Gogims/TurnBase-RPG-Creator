using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Presenta todos los menu del Game Engine
/// </summary>
public class GameEngine : EditorWindow {	

	public static RPGInspector inspectorRpg = EditorWindow.GetWindow<RPGInspector> ();
	/// <summary>
	/// Presenta la ventana de crear un nuevo mapa
	/// </summary>
	[MenuItem("RPG-Creator/New Map")]
	public static void ShowMap()
	{
		var window = EditorWindow.GetWindow<MapWindow> ();

		window.maxSize = new Vector2 (500,200);
		window.minSize= new Vector2(500,200);
		window.Init();
		window.Show ();
	}
	/// <summary>
	/// Presenta el mantenimiento de jugadores
	/// </summary>
	[MenuItem("RPG-Creator/Player")]
	public static void ShowPlayer()
	{
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<PlayerUI>();
        //window.maxSize = new Vector2(500, 200);
        //window.minSize = new Vector2(500, 200);
        window.Init();
        window.Show();
    }
	
	/// <summary>
	/// Presenta el Mantenimiento de las armas
	/// </summary>
	[MenuItem("RPG-Creator/Weapon")]
	public static void ShowWeapon()
	{
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<WeaponUI>();

        window.maxSize = new Vector2(900, 400);
        window.minSize = new Vector2(900, 400);
        window.Init();
        window.Show();
	}    

    /// <summary>
	/// Presenta el Mantenimiento de las armaduras
	/// </summary>
	[MenuItem("RPG-Creator/Armor")]
    public static void ShowArmor()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<ArmorUI>();

        window.maxSize = new Vector2(900, 400);
        window.minSize = new Vector2(900, 400);
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
		var window =EditorWindow.GetWindow<UploadImage>();
		window.Init ();
		window.Show ();
	}

    /// <summary>
    /// Presenta la ventana de la curva de experiencia
    /// </summary>
    [MenuItem("RPG-Creator/Formula")]
    public static void ShowFormula()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<ExpCurve>();
        window.Init();
        window.Show();
    }
	/// <summary>
	/// Presenta la ventana del inspector de RPG-CREATOR
	/// </summary>
	[MenuItem("RPG-Creator/RPG-Inspector")]
	public static void ShowInspector()
	{
		if (inspectorRpg == null) 
			inspectorRpg = EditorWindow.GetWindow<RPGInspector> ();
		//Show existing window instance. If one doesn't exist, make one.]
		inspectorRpg.Init();
		inspectorRpg.Show();
	}
}
