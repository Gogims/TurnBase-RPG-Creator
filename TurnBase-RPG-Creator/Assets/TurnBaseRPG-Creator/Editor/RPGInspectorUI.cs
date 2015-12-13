using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Inspector que presenta cual objeto esta seleccionado.
/// </summary>
public class RPGInspectorUI : EditorWindow
{
    /// <summary>
    /// Game object seleccionado en el object browser
    /// </summary>
    public static GameObject ObjectBrowser;
    /// <summary>
    /// Game object seleccionado en la escena del mapa
    /// </summary>
    public static GameObject ObjectMap;
    /// <summary>
    /// Dirección de la escena del mapa anterior (si selecciona una puerta)
    /// </summary>
    public static string scenePath = string.Empty;
    /// <summary>
    /// True = Enemigo seleccionado aparece con un cuadro encima para representar área de movimiento
    /// </summary>
    public static bool EnemySelection;
    /// <summary>
    /// Estado en el que el inspector elige un game object del mapa
    /// </summary>
    public static bool SelectionMode;


    /// <summary>
    /// Mensaje de error en el inspector
    /// </summary>
    private string Error;
    /// <summary>
    /// Variable temporal para guardar la referencia de un gameobject
    /// </summary>
    private static GameObject temp;
    private GUIStyle style;

    public RPGInspectorUI()
    {
        Error = string.Empty;
        style = new GUIStyle();
        style.normal.textColor = Color.red;
    }

	void OnGUI() 
	{
        // Seleccionado en Object Browser
        GUILayout.BeginArea(new Rect(20, 20, Constant.INSPECTOR_IMAGE_WIDTH + 80, Constant.INSPECTOR_IMAGE_HEIGTH + 140), "Object In Browser", EditorStyles.boldLabel);
        GUILayout.Space(15);

        DrawObject(ObjectBrowser);
        GUILayout.Space(Constant.INSPECTOR_IMAGE_HEIGTH + 20);

        GUI.enabled = ObjectBrowser != null;
        if (GUILayout.Button("Clear Selection"))
        {
            ObjectBrowser = null;
        }
        GUI.enabled = true;
        GUILayout.EndArea();

        // Seleccionado en la escena del mapa 
        GUILayout.BeginArea(new Rect(20, Constant.INSPECTOR_IMAGE_HEIGTH + 150, Constant.INSPECTOR_IMAGE_WIDTH + 50, position.height - Constant.INSPECTOR_IMAGE_HEIGTH + 120), "Object In Map", EditorStyles.boldLabel);
        GUILayout.Space(15);

        DrawObject(ObjectMap);
        GUILayout.Space(Constant.INSPECTOR_IMAGE_HEIGTH + 55);

        // Area para dibujar si el gameobject no vino vacío
        if (ObjectMap != null)
        {
            // Tropa
            if (ObjectMap.GetComponent<Troop>() != null)
            {
                var troop = ObjectMap.GetComponent<Troop>();
                EnemySelection = GUILayout.Toggle(EnemySelection, "Selection Mode");

                GUILayout.Label("Area:", EditorStyles.boldLabel);
                troop.AreaWidth = EditorGUILayout.IntField("Width:", troop.AreaWidth);
                troop.AreaHeight = EditorGUILayout.IntField("Height:", troop.AreaHeight);

                if (troop.Changed)
                {
                    troop.Changed = false;
                    ObjectMap.name = ObjectMap.name.Replace("(Clone)", "");
                    GameObject obj = Instantiate(ObjectMap);
                    var father = ObjectMap.transform.parent.gameObject;

                    obj.transform.parent = father.transform;
                    obj.transform.localPosition = new Vector2(0, 0);

                    var sprite = father.GetComponent<SpriteRenderer>().sprite;
                    var sprite2 = obj.GetComponent<SpriteRenderer>().sprite;
                    obj.transform.localScale = new Vector3(sprite.rect.width / sprite2.rect.width, sprite.rect.height / sprite2.rect.height);

                    DestroyImmediate(ObjectMap);
                    ObjectMap = obj;
                    Selection.activeGameObject = obj;
                }
            }

            // Puerta
            else if (ObjectMap.GetComponent<Door>() != null)
            {
                var door = ObjectMap.GetComponent<Door>();
                GUILayout.Label("Map:", EditorStyles.boldLabel);
                GUI.enabled = false;
                GUILayout.TextField(door.InMap.Name);
                GUI.enabled = true;
                if (GUILayout.Button("Select Inside Map"))
                {
                    var window = EditorWindow.GetWindow<MapUI>();
                    window.Initialize(ref door.InMap);
                    window.Show();
                }

                GUI.enabled = door.InMap.Name != string.Empty;
                if (GUILayout.Button("Select Position"))
                {
                    ObjectMap.tag = "RPG-SELECTED";
                    scenePath = EditorApplication.currentScene;
                    EditorApplication.SaveScene();
                    EditorApplication.OpenScene(door.InMap.MapPath);
                    MapEditor.selection = true;
                    Map.ResetDoors();
                }

                GUI.enabled = false;
                EditorGUILayout.FloatField("X:", door.X);
                EditorGUILayout.FloatField("Y:", door.Y);
                GUI.enabled = true;
            }

            // Tile
            else if (ObjectMap.GetComponent<Tile>() != null)
            {
                var tile = ObjectMap.GetComponent<Tile>();

                if (tile.Type == Constant.TileType.Pressable)
                {
                    GUILayout.Label("Object to remove:");
                    string name = tile.Removable == null ? string.Empty : tile.Removable.tag;
                    GUI.enabled = false;
                    GUILayout.TextField(name);
                    GUI.enabled = true;

                    if (GUILayout.Button("Select Object From Scene"))
                    {
                        temp = ObjectMap;
                    }
                }
            }

            else if (ObjectMap.GetComponent<Obstacle>() != null)
            {
                var obstacle = ObjectMap.GetComponent<Obstacle>();

                if (obstacle.Type == Constant.ObstacleType.Destroyable)
                {
                    EditorGUILayout.LabelField("HP:");
                    obstacle.hp = EditorGUILayout.IntField(obstacle.hp);
                }
                else if (obstacle.Type == Constant.ObstacleType.Switchable)
                {
                    GUILayout.Label("Object to remove:");
                    string name = obstacle.Removable == null ? string.Empty : obstacle.Removable.tag;
                    GUI.enabled = false;
                    GUILayout.TextField(name);
                    GUI.enabled = true;

                    if (GUILayout.Button("Select Object From Scene"))
                    {
                        temp = ObjectMap;
                    }
                }
            }
        }

        GUILayout.Label(Error, style);
        GUILayout.EndArea();
    }    

    void Update()
    {
        if (Error == null)
        {
            Error = string.Empty;
        }        

        // Si se seleccionó otro gameobject en el modo de selección
        if (temp != null && temp != ObjectMap)
        {
            // Tile
            var tile = temp.GetComponent<Tile>();
            if (tile != null)
            {
                Selection.activeGameObject = temp;

                if (ObjectMap.transform.parent.name != "Map" && ObjectMap.GetComponent<Player>() == null)
                {
                    tile.Removable = ObjectMap;
                }
                else
                {
                    Error = "Can't select Tiles or the Player";
                }
            }

            // Obstacle            
            else if (temp.GetComponent<Obstacle>() != null)
            {
                Selection.activeGameObject = temp;

                var obstacle = temp.GetComponent<Obstacle>();

                if (ObjectMap.transform.parent.name != "Map" && ObjectMap.GetComponent<Player>() == null)
                {
                    obstacle.Removable = ObjectMap;
                }
                else
                {
                    Error = "Can't select Tiles or the Player";
                }
            }

            temp = null;
        }
    }

    public void DrawObject(GameObject gobj)
    {
        if (gobj == null) return;

        RPGElement element = gobj.GetComponent<RPGElement>();

        EditorGUILayout.LabelField(element.Name);
        EditorGUILayout.LabelField(" (" + gobj.tag + ")");

        GUI.DrawTextureWithTexCoords(new Rect(0, 55, Constant.INSPECTOR_IMAGE_WIDTH, Constant.INSPECTOR_IMAGE_HEIGTH), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
    }
}
