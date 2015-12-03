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
            if (MapEditor.selectedObject.GetComponent<Troop>() != null)
            {
                DrawTroopAreas();
            }
        }

        GUILayout.EndArea();
    }

    void DrawTroopAreas()
    {
        Troop enemy = MapEditor.selectedObject.GetComponent<Troop>();

        GUILayout.Label("Area:", EditorStyles.boldLabel);
        enemy.AreaWidth = EditorGUILayout.IntField("Width:", enemy.AreaWidth);
        enemy.AreaHeight = EditorGUILayout.IntField("Height:", enemy.AreaHeight);
    }
}
