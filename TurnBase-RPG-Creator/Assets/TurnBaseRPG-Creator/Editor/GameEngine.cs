using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Presenta todos los menu del Game Engine
/// </summary>
public class GameEngine : EditorWindow {	

	public static RPGInspectorUI inspectorRpg = EditorWindow.GetWindow<RPGInspectorUI> ();
    static GameEngine() {
    }
    /// <summary>
    /// Presenta la ventana de crear un nuevo mapa
    /// </summary>
    [MenuItem("RPG-Creator/New Map")]
    public static void ShowMap()
    {
        var window = EditorWindow.GetWindow<MapUI>();
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
        var window = EditorWindow.GetWindow<ArmorUI>();

        window.maxSize = new Vector2(900, 400);
        window.minSize = new Vector2(900, 400);
        window.Init();
        window.Show();
    }
    /// <summary>
    /// Presenta la ventana de la curva de experiencia
    /// </summary>
    [MenuItem("RPG-Creator/Formula")]
    public static void ShowFormula()
    {
        var window = EditorWindow.GetWindow<ExpCurve>();
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta la ventana de la curva de experiencia
    /// </summary>
    [MenuItem("RPG-Creator/Stats")]
    public static void ShowStats()
    {
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
            inspectorRpg = EditorWindow.GetWindow<RPGInspectorUI>();
        inspectorRpg.Init();
        inspectorRpg.Show();
    }

    /// <summary>
    /// Presenta la ventana de las clases
    /// </summary>
    [MenuItem("RPG-Creator/Class")]
    public static void ShowClass()
    {
        var window = EditorWindow.GetWindow<StatsCurve>();
        window.Init(1);
        window.Show();
    }

    /// <summary>
    /// Presenta la ventana para crear un proyecto nuevo.
    /// </summary>
    [MenuItem("RPG-File/New Project")]
    public static void ShowNewProject()
    {
        var window = EditorWindow.GetWindow<ProjectUI>();
        window.titleContent.text = "New";
        
        window.minSize = new Vector2(450, 200);
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
    /// <summary>
    /// Presenta la ventana para ajustar los settings del proyecto
    /// </summary>
    [MenuItem("RPG-Tools/Settings...")]
    public static void ShowSettings()
    {
        var window = GetWindow<SettingsUI>();
        window.Init();
        window.Show();
    }
    /// <summary>
    /// Presenta La ventana de importar imagenes
    /// </summary>
    [MenuItem("RPG-Tools/Import Image")]
    public static void ShowImportImage()
    {
        var window = EditorWindow.GetWindow<ImportImageUI>();
        window.minSize = new Vector2(500, 300);
        window.Init();
        window.Show();
    }
    /// <summary>
    /// Presenta La ventana de importar imagenes
    /// </summary>
    [MenuItem("RPG-Tools/Import Sound")]
    public static void ShowImportSound()
    {
    }
    
}
