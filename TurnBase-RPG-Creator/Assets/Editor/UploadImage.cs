using UnityEngine;
using System.Collections;
using UnityEditor;

public class UploadImage : EditorWindow
{	
	Object PhotoUpload;
	Texture2D texture = null;
	FileBrowser image = new FileBrowser ();
	bool upload;

	void OnGUI() 
	{
        // Text field to upload image
        GUI.enabled = false;
        image.path =  EditorGUILayout.TextField(image.path, GUILayout.Width(300) );

        // Button to upload image
        GUI.enabled = true;
        upload = GUILayout.Button ("Upload Photo", GUILayout.Width(100));
		texture = new Texture2D (320, 137);

		texture.LoadImage (image.GetImage);
		EditorGUILayout.PrefixLabel("Preview");
		EditorGUI.PrefixLabel(new Rect(150,45, 320,137),0, new GUIContent("Preview:"));
		EditorGUI.DrawPreviewTexture(new Rect(150,60,320,137),texture);
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