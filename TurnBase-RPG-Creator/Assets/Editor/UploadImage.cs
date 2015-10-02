using UnityEngine;
using System.Collections;
using UnityEditor;

public class UploadImage : EditorWindow
{	
    Texture2D texture;
	FileBrowser image = new FileBrowser ("Assets/Sprites/96x96.jpg");
	bool upload;
    const int width = 96;
    const int height = 96;

	void OnGUI() 
	{
        // Text field to upload image
        GUI.enabled = false;
        image.path = GUI.TextField(new Rect(0,0, 300, 20), image.path);

        // Button to upload image
        GUI.enabled = true;
        upload = GUI.Button (new Rect(300, 0, 100, 20), "Upload Photo");
		texture = new Texture2D (width, height);

		texture.LoadImage (image.GetImage);
		EditorGUILayout.PrefixLabel("Preview");
		EditorGUI.PrefixLabel(new Rect(150,45, 320,137),0, new GUIContent("Preview:"));
		EditorGUI.DrawPreviewTexture(new Rect(150, 60, width, height), texture);
	}

	void Update()
	{
		if (upload) {
			image.path = EditorUtility.OpenFilePanel ("test", "test2", "jpg;*.png");

			if (!string.IsNullOrEmpty(image.path)) {
				texture.LoadImage(image.GetImage);
				AssetDatabase.CreateAsset(texture, "Assets/Sprites/test.asset");
			}
		}

	}
}