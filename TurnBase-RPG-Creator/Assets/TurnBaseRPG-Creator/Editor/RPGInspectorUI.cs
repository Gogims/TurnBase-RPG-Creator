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
        GUILayout.BeginArea(new Rect(0, 0, Constant.INSPECTOR_IMAGE_WIDTH + 50, Constant.INSPECTOR_IMAGE_HEIGTH + 75));

        string tag = "";
        string name = "";

        if (texture == null || defaultText == null)
            this.Init();
		
		if (MapEditor.selectedObject != null && MapEditor.selectedObject.tag != "RPG-CREATOR") {
            texture = MapEditor.selectedObject.GetComponent<RPGElement>().Icon;
			tag = MapEditor.selectedObject.tag;
            name = MapEditor.selectedObject.GetComponent<RPGElement>().Name;
		}
        else {
            texture = defaultText;
		}        

		EditorGUILayout.PrefixLabel(tag);
		EditorGUI.PrefixLabel(new Rect(50,45, 500,137),0, new GUIContent(name));
        GUI.DrawTextureWithTexCoords(new Rect(50, 55, width, height), texture.texture, Constant.GetTextureCoordinate(texture));

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(0, Constant.INSPECTOR_IMAGE_WIDTH + 75, Constant.INSPECTOR_IMAGE_WIDTH + 50, 100));

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
        }        

        GUILayout.EndArea();
    }    
}
