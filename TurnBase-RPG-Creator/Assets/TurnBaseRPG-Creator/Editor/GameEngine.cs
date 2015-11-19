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
        window.maxSize = new Vector2(900, 770);
        window.minSize = new Vector2(900, 770);
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta el mantenimiento de enemigos
    /// </summary>
    [MenuItem("RPG-Creator/Enemy")]
    public static void ShowEnemy()
    {
        var window = EditorWindow.GetWindow<EnemyUI>();
        window.maxSize = new Vector2(900, 930);
        window.minSize = new Vector2(900, 930);
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta el mantenimiento de enemigos
    /// </summary>
    [MenuItem("RPG-Creator/State")]
    public static void ShowState()
    {
        var window = EditorWindow.GetWindow<StateUI>();
        window.maxSize = new Vector2(900, 530);
        window.minSize = new Vector2(900, 530);
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta el mantenimiento de jugadores
    /// </summary>
    [MenuItem("RPG-Creator/Ability")]
    public static void ShowAbility()
    {
        var window = EditorWindow.GetWindow<AbilityUI>();
        window.maxSize = new Vector2(900, 400);
        window.minSize = new Vector2(900, 400);
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
    /// Presenta la ventana del inspector de RPG-CREATOR
    /// </summary>
    [MenuItem("RPG-Tools/RPG-Inspector")]
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
        var window = EditorWindow.GetWindow<JobUI>();
        window.maxSize = new Vector2(900, 600);
        window.minSize = new Vector2(900, 600);
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta la ventana de los items
    /// </summary>
    [MenuItem("RPG-Creator/Item")]
    public static void ShowItem()
    {
        var window = EditorWindow.GetWindow<ItemUI>();
        window.maxSize = new Vector2(900, 500);
        window.minSize = new Vector2(900, 500);
        window.Init();
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
    /// <summary>
    /// Presenta La ventana de importar imagenes
    /// </summary>
    [MenuItem("RPG-File/Object Browser")]
    public static void ShowObjectBrowser()
    {
        var window = EditorWindow.GetWindow<ObjectBrowserUI>();
        window.Init();
        window.Show();
    }

    /// <summary>
    /// Presenta la ventana de las clases
    /// </summary>
    [MenuItem("RPG-Creator/Map Objects")]
    public static void ShowMapObjects()
    {
        var window = EditorWindow.GetWindow<MapObjectsUI>();
        window.Init();
        window.Show();
    }
    
}
