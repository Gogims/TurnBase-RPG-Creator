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
		image = MapEditor.floorObject.GetComponent<SpriteRenderer>().sprite.texture.EncodeToJPG();
	}
	void OnGUI() 
	{
		image = MapEditor.floorObject.GetComponent<SpriteRenderer> ().sprite.texture.EncodeToPNG ();
		GUI.enabled = true;
		texture = new Texture2D (width, height);
		texture.LoadImage (image);
		EditorGUILayout.PrefixLabel(MapEditor.floorObject.tag);
		EditorGUI.PrefixLabel(new Rect(50,45, 500,137),0, new GUIContent(MapEditor.floorObject.name));
		EditorGUI.DrawPreviewTexture(new Rect(50, 55, width, height), texture);
	}
}
