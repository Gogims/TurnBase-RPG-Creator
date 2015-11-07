using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Inspector que presenta cual objeto esta seleccionado.
/// </summary>
public class RPGInspectorUI : EditorWindow {

	static Texture2D texture;
	static public byte[] image;
	const int width = Constatnt.INSPECTOR_IMAGE_WIDTH;
	const int height = Constatnt.INSPECTOR_IMAGE_HEIGTH;

	public void Init()
	{
		image = new FileBrowser ("Assets/TurnBaseRPG-Creator/RPG-Sprites/96x96.jpg").GetImage;
	}
	void OnGUI() 
	{
		string tag = "";
		string name = "";
		if (MapEditor.selectedObject != null && MapEditor.selectedObject.tag == "Tile") {
			image = MapEditor.selectedObject.GetComponent<SpriteRenderer> ().sprite.texture.EncodeToPNG ();
			tag = MapEditor.selectedObject.tag;
			name = MapEditor.selectedObject.name;
		} else {
            image = new FileBrowser("Assets/TurnBaseRPG-Creator/RPG-Sprites/96x96.jpg").GetImage;
		}
		GUI.enabled = true;
		texture = new Texture2D (width, height);
		texture.LoadImage (image);
		EditorGUILayout.PrefixLabel(tag);
		EditorGUI.PrefixLabel(new Rect(50,45, 500,137),0, new GUIContent(name));
		EditorGUI.DrawPreviewTexture(new Rect(50, 55, width, height), texture);
	}
}
