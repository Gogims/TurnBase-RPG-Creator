using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// Inspector que presenta cual objeto esta seleccionado.
/// </summary>
public class RPGInspector : EditorWindow {

	static Texture2D texture;
	static public byte[] image;
	const int width = 150;
	const int height = 150;

	public void Init()
	{
		image = new FileBrowser ("Assets/Sprites/96x96.jpg").GetImage;
	}
	void OnGUI() 
	{
		string tag = "";
		string name = "";
		if (MapEditor.floorObject != null) {
			image = MapEditor.floorObject.GetComponent<SpriteRenderer> ().sprite.texture.EncodeToPNG ();
			tag = MapEditor.floorObject.tag;
			name = MapEditor.floorObject.name;
		} else {
			image = new FileBrowser ("Assets/Sprites/96x96.jpg").GetImage;
		}
		GUI.enabled = true;
		texture = new Texture2D (width, height);
		texture.LoadImage (image);
		EditorGUILayout.PrefixLabel(tag);
		EditorGUI.PrefixLabel(new Rect(50,45, 500,137),0, new GUIContent(name));
		EditorGUI.DrawPreviewTexture(new Rect(50, 55, width, height), texture);
	}
}
