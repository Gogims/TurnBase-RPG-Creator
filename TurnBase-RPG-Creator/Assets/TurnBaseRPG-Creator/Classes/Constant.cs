using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Constantes de TurnBase-RPG-Creator.
/// </summary>
public class Constant
{
#if UNITY_EDITOR
    /// <summary>
    /// Layers que utiliza RPG-Creator para ordenar los objetos en la scene.
    /// </summary>
    #region Layers
    public const string LAYER_ITEM = "Items";
    public const string LAYER_TILE = "Tile";
    public const string LAYER_ACTOR = "Actors";
    #endregion
    /// <summary>
	/// La altura maxima del mapa
	/// </summary>
	public const int MAX_MAP_HEIGTH = 100;
	/// <summary>
	/// Ancho maximo del mapa
	/// </summary>
	public const int MAX_MAP_WIDTH = 100;
	/// <summary>
	/// Altura Minima del mapa
	/// </summary>
	public const int MIN_MAP_HEIGTH = 13;
	/// <summary>
	/// Ancho Minimo del mapa.
	/// </summary>
	public const int MIN_MAP_WIDTH = 17;
	/// <summary>
	/// Altura de la imagen del RPGInspector
	/// </summary>
	public const int INSPECTOR_IMAGE_HEIGTH = 150;
	/// <summary>
	/// Ancho de la imagen del RPGInspector
	/// </summary>
	public const int INSPECTOR_IMAGE_WIDTH = 150;
    /// <summary>
    /// Ancho de los sprites de los elementos del juego
    /// </summary>
    public const int SpriteWidth = 64;
    /// <summary>
    /// Alto de los sprites de los elementos del juego
    /// </summary>
    public const int SpriteHeight = 64;


    /// <summary>
    /// Crea el marco de la imagen del equipamiento
    /// </summary>
    /// <returns>Rectángulo con sus coordenadas definidas</returns>
    public static Rect GetTextureCoordinate(Sprite Image)
    {
        return new Rect(Image.textureRect.x / Image.texture.width,
                        Image.textureRect.y / Image.texture.height,
                        Image.textureRect.width / Image.texture.width,
                        Image.textureRect.height / Image.texture.height);
    }

    /// <summary>
    /// Agrega un scene a la configuracion de construccion ( Build Settings).
    /// </summary>
    /// <param name="path">Ruta del scene</param>
    public static void AddSceneToBuild(string path)
    {
        EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
        System.Array.Copy(original, newSettings, original.Length);
        EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(path, true);
        newSettings[newSettings.Length - 1] = sceneToAdd;
        EditorBuildSettings.scenes = newSettings;
    }

    /// <summary>
    /// remueve un scene de la configuracion de construccion ( Build Settings). 
    /// </summary>
    /// <param name="p">ruta de la scene</param>
    public static void RemoveScene(string path)
    {
        EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length - 1];

        for (int i = 0, j = 0; i < original.Length; i++)
        {
            if (original[i].path == path) continue;
            newSettings[j] = original[i];
            j++;
        }
        EditorBuildSettings.scenes = newSettings;
    }
#endif

    /// <summary>
    /// Ancho del fondo de pantalla del combate
    /// </summary>
    public const int BackgroundWidth = 1044;
    /// <summary>
    /// Alto del fondo de pantalla del combate
    /// </summary>
    public const int BackgroundHeight= 580;
    /// <summary>
    /// Permite saber al jugador si el juego esta en pausa o no
    /// </summary>
    public static bool start;
    public static string LastSceneLoaded;
    public static GameObject ActiveMap;

    public static bool checkDoor(AbstractMap door)
    {
        string name = door.MapPath.Substring(door.MapPath.LastIndexOf("/") + 1).Replace(".unity", "");
        Object[] obj = Resources.LoadAll("Maps", typeof(GameObject));
        foreach (var i in obj)
        {
            Map iobj = (i as GameObject).GetComponent<Map>();
            if (iobj.Id == name)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Se encarga de colocar los anchor point en el borde del objeto
    /// </summary>
    /// <param name="o"></param>
    public static void SetAnchorPoint(GameObject o)
    {
        if (o != null && o.GetComponent<RectTransform>() != null)
        {
            var r = o.GetComponent<RectTransform>();
            var p = o.transform.parent.GetComponent<RectTransform>();

            var offsetMin = r.offsetMin;
            var offsetMax = r.offsetMax;
            var _anchorMin = r.anchorMin;
            var _anchorMax = r.anchorMax;

            var parent_width = p.rect.width;
            var parent_height = p.rect.height;

            var anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
                                        _anchorMin.y + (offsetMin.y / parent_height));
            var anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
                                        _anchorMax.y + (offsetMax.y / parent_height));

            r.anchorMin = anchorMin;
            r.anchorMax = anchorMax;

            r.offsetMin = new Vector2(0, 0);
            r.offsetMax = new Vector2(0, 0);
            r.pivot = new Vector2(0.5f, 0.5f);

        }
    }

    /// <summary>
    /// Tipos de items.
    /// </summary>
    public enum ItemType
    { 
        Armor,
        Usable,
        Weapon
    };

    public enum AttackType
    {
        MagicAttack,
        NormalAttack
    }
    public enum AOE
    {
        OneEnemy,
        TwoEnemies,
        AllEnemies,
        SingleAlly,
        TwoAllies,
        AllAllies,
        Self
    };

    public enum OffenseDefense
    {
        Offensive,
        Defensive
    };

    public enum ActionType
    {
        None,
        TargetOnlyEnemy,
        TargetOnlyAlly,
        TargetMyself,
        DoNothing
    };

    public enum DamageHeal
    {
        Damage,
        Heal
    };

    public enum ItemAvailable
    {
        Always,
        OnlyBattle,
        OnlyMenu,
        Never
    };

    public enum Attribute
    {
        None,
        HP,
        MP,
        Attack,
        Defense,
        Magic,
        MagicDefense,
        Agility,
        Luck,
        MaxHP,
        MaxMP
    };

    public enum MapObjectType
    {
        Door=9,
        House,
        Pickup,
        Obstacle,
        Tile,
        Wall
    };

    public enum EnemyType
    {
        Follower,
        Random,
        Stationary
    };

    public enum AudioType
    {
        Background,
        BackgroundEffect,
        Sound
    };

    public enum TileType
    {
        Normal,
        Pressable,
        EnemySpawn
    };

    public enum ObstacleType
    {
        Destroyable,
        Movable,
        Switchable        
    };

    public enum FormulaType
    {
        XP,
        MaxHP,
        MaxMP,
        Attack,
        Defense,
        MagicAttack,
        MagicDefense,
        Agility,
        Luck
    };    
}
