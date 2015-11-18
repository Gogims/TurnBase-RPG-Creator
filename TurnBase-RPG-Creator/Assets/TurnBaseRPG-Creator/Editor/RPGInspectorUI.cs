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
        if (texture == null || defaultText == null)
            this.Init();
		string tag = "";
		string name = "";
		if (MapEditor.selectedObject != null && MapEditor.selectedObject.tag != "RPG-CREATOR") {
            texture = MapEditor.selectedObject.GetComponent<RPGElement>().Icon;
			tag = MapEditor.selectedObject.tag;
			name = MapEditor.selectedObject.name;
		} else {
            texture = defaultText;
		}
		GUI.enabled = true;
		EditorGUILayout.PrefixLabel(tag);
		EditorGUI.PrefixLabel(new Rect(50,45, 500,137),0, new GUIContent(name));
        GUI.DrawTextureWithTexCoords(new Rect(50, 55, width, height), texture.texture, GetTextureCoordinate(texture));
	}
    private Rect GetTextureCoordinate(Sprite T)
    {
        return new Rect(T.textureRect.x / T.texture.width,
                        T.textureRect.y / T.texture.height,
                        T.textureRect.width / T.texture.width,
                        T.textureRect.height / T.texture.height);
    }
}
