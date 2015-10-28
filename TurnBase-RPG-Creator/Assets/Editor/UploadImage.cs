using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class UploadImage : EditorWindow
{	
    Texture2D texture;
	FileBrowser image = new FileBrowser ("Assets/Sprites/96x96.jpg");
	RpgObject rpgobject = new RpgObject();
	bool upload;
	bool import=false;
    const int width = 120;
    const int height = 120;
	string [] obtype = ObjectTypes.GetTypes ();
	string oldname;
	bool spritesheet = false;
	public void Init(){
		rpgobject.name = "New_object";
	}
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
		EditorGUI.LabelField(new Rect(0,30,100,20),new GUIContent("Name:"));
		rpgobject.name = EditorGUI.TextField(new Rect(90,30,200,20),rpgobject.name);
		EditorGUI.LabelField(new Rect(0,70,100,20),new GUIContent("Sprite Type:"));
		rpgobject.type = EditorGUI.Popup(new Rect(90,70,200,10),rpgobject.type,obtype);
		EditorGUI.LabelField(new Rect(0,90,100,20),new GUIContent("Description:"));
		EditorGUI.TextField(new Rect(0,110,290,120),ObjectTypes.GetDescription(obtype[rpgobject.type]));
		EditorGUI.LabelField(new Rect(300,70, 80,20),new GUIContent("Sprite Sheet:"));
		spritesheet = EditorGUI.Toggle(new Rect(300,90,20,20),spritesheet);
		EditorGUI.DrawPreviewTexture(new Rect(300, 110, width, height), texture);
		import = GUI.Button (new Rect(60, 240,300, 20), "Import Object");
	}

	void Update()
	{
		if (upload) {
			image.path = EditorUtility.OpenFilePanel ("test", "test2", "jpg;*.png");
		}
		if (import) {
			oldname = rpgobject.name;
			rpgobject.name = obtype[rpgobject.type]+"_"+rpgobject.name;
			Debug.Log(rpgobject.name);
			CreateRpgObject();
			rpgobject.name = oldname;
			Debug.Log(rpgobject.name);
		}
	}
	void SaveSprites(){
		if (texture != null) {
			if (!spritesheet) 
				TextureScale.Bilinear(texture,32,32);
			var bytes = texture.EncodeToPNG ();
			int startindex = image.path.LastIndexOf ('.');
			FileStream file = File.Open (Application.dataPath + "/Sprites/"+rpgobject.name+image.path.Substring (startindex), FileMode.Create);
			var binary = new BinaryWriter (file);
			binary.Write (bytes);
			file.Close ();
			AssetDatabase.Refresh();
		}
	}
	string GetPath(){
		int startindex = image.path.LastIndexOf ('.');
		return "Assets/Sprites/"+ rpgobject.name+image.path.Substring (startindex);
	}
	void CreateRpgObject(){
		SaveSprites ();
		TextureImporter x = TextureImporter.GetAtPath(GetPath()) as TextureImporter;
        x.spritePixelsPerUnit = 32;
		x.textureType = TextureImporterType.Advanced;
		x.isReadable = true;
		x.textureFormat = TextureImporterFormat.ARGB32;
		x.spriteImportMode = spritesheet? SpriteImportMode.Multiple:SpriteImportMode.Single;
		AssetDatabase.ImportAsset(GetPath(),ImportAssetOptions.ForceUpdate); 

	}
}