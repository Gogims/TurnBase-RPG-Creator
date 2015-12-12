using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Inspector que presenta cual objeto esta seleccionado.
/// </summary>
public class RPGInspectorUI : EditorWindow {    
    static Sprite texture;
    private Sprite defaultText;
	const int width = Constant.INSPECTOR_IMAGE_WIDTH;
	const int height = Constant.INSPECTOR_IMAGE_HEIGTH;
    public static string scenePath = string.Empty;    

    /// <summary>
    /// Si al elegir un enemigo se debe seleccionar el área donde se moverá
    /// </summary>
    public static bool SelectionMode;

    public void Init()
	{
		defaultText = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TurnBaseRPG-Creator/RPG-Sprites/96x96.jpg");
	}


	void OnGUI() 
	{
        GUILayout.BeginArea(new Rect(20, 20, Constant.INSPECTOR_IMAGE_WIDTH + 50, Constant.INSPECTOR_IMAGE_HEIGTH + 75));

        string tag = "";
        string name = "";

        if (texture == null || defaultText == null)
            this.Init();
		
		if (MapEditor.selectedObject != null && MapEditor.selectedObject.tag != "RPG-CREATOR") {
            texture = MapEditor.selectedObject.GetComponent<RPGElement>().Icon;
			tag = " (" + MapEditor.selectedObject.tag + ")";
            name = MapEditor.selectedObject.GetComponent<RPGElement>().Name;
		}
        else {
            texture = defaultText;
		}

        EditorGUILayout.LabelField(name);
        EditorGUILayout.LabelField(tag);
        GUI.DrawTextureWithTexCoords(new Rect(0, 55, width, height), texture.texture, Constant.GetTextureCoordinate(texture));

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(20, Constant.INSPECTOR_IMAGE_HEIGTH + 95, Constant.INSPECTOR_IMAGE_WIDTH + 50, 140));

        // Area para dibujar si el gameobject no vino vacío
        if (MapEditor.selectedObject != null)
        {
            var troop = MapEditor.selectedObject.GetComponent<Troop>();

            if (troop != null)
            {
                SelectionMode = GUILayout.Toggle(SelectionMode, "Selection Mode");

                GUILayout.Label("Area:", EditorStyles.boldLabel);
                troop.AreaWidth = EditorGUILayout.IntField("Width:", troop.AreaWidth);
                troop.AreaHeight = EditorGUILayout.IntField("Height:", troop.AreaHeight);

                if (troop.Changed)
                {
                    troop.Changed = false;
                    MapEditor.selectedObject.name = MapEditor.selectedObject.name.Replace("(Clone)", "");
                    GameObject obj = Instantiate(MapEditor.selectedObject);
                    var father = MapEditor.selectedObject.transform.parent.gameObject;

                    obj.transform.parent = father.transform;
                    obj.transform.localPosition = new Vector2(0, 0);

                    var sprite = father.GetComponent<SpriteRenderer>().sprite;
                    var sprite2 = obj.GetComponent<SpriteRenderer>().sprite;
                    obj.transform.localScale = new Vector3(sprite.rect.width / sprite2.rect.width, sprite.rect.height / sprite2.rect.height);

                    DestroyImmediate(MapEditor.selectedObject);
                    MapEditor.selectedObject = obj;
                    Selection.activeGameObject = obj;
                }
            }
            var door = MapEditor.selectedObject.GetComponent<Door>();
            if (door != null)
            {
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
                    MapEditor.selectedObject.tag = "RPG-SELECTED";                    
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
        }        

        GUILayout.EndArea();
    }    
}
