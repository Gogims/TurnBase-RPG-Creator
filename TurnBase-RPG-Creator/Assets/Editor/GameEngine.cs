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
        var window = EditorWindow.GetWindow<MapWindow>();

        window.maxSize = new Vector2(500, 200);
        window.minSize = new Vector2(500, 200);
        window.Init();
        window.Show();
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
    /// Presenta La ventana de importar imagenes
    /// </summary>
    [MenuItem("RPG-Creator/Import Image")]
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
<<<<<<< HEAD
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
	/// <summary>
	/// Presenta la ventana para crear un proyecto nuevo.
	/// </summary>
	[MenuItem("RPG-File/New Project")]
	public static void ShowNewProject()
	{
        var window = EditorWindow.GetWindow<NewProject>();
        window.Init();
        window.Show();
	}
	/// <summary>
	/// Presenta la ventana de crear un nuevo proyecto.
	/// </summary>
	[MenuItem("RPG-File/Open Project")]
	public static void ShowOpenProject()
	{
        Project.Open(EditorUtility.OpenFolderPanel("Select Project", "", ""));
	}
	/// <summary>
	/// Presenta la ventana para hacer deploy
	/// </summary>
	[MenuItem("RPG-File/Deploy")]
	public static void ShowDeployProject()
	{
	}

=======

    /// <summary>
    /// Presenta la ventana de la curva de experiencia
    /// </summary>
    [MenuItem("RPG-Creator/Stats")]
    public static void ShowStats()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<StatsCurve>();
        window.Init(1);
        window.Show();
    }

    /// <summary>
    /// Presenta la ventana del inspector de RPG-CREATOR
    /// </summary>
    [MenuItem("RPG-Creator/RPG-Inspector")]
    public static void ShowInspector()
    {
        if (inspectorRpg == null)
            inspectorRpg = EditorWindow.GetWindow<RPGInspector>();
        //Show existing window instance. If one doesn't exist, make one.]
        inspectorRpg.Init();
        inspectorRpg.Show();
    }

    /// <summary>
    /// Presenta la ventana de las clases
    /// </summary>
    [MenuItem("RPG-Creator/Class")]
    public static void ShowClass()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var window = EditorWindow.GetWindow<StatsCurve>();
        window.Init(1);
        window.Show();
    }
>>>>>>> 4c5b0929ccb661a1ad2732646c9b69522c14b624
}
