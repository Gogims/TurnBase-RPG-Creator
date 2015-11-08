using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ImportImageUI : EditorWindow
{	
	FileBrowser image = new FileBrowser ("Assets/Sprites/96x96.jpg");
	RPGImage rpgobject = new RPGImage();
    string path = "";
    bool upload;
	bool import=false;
    const int width = 120;
    const int height = 120;
	string [] obtype = ObjectTypes.GetTypes ();
	string oldname;
	bool spritesheet = false;
    ErrorHandler err;
    /// <summary>
    /// Incializa la ventana.
    /// </summary>
	public void Init(){
		rpgobject.name = "New_object";
        err = new ErrorHandler();
        err.InsertPropertyError("Name", rpgobject.name.Length, "The length of the name has to be greater than 5",new Rect(290,30,200,20));
        err.InsertPropertyError("Image", path.Length, "You have to select a image.",new Rect(410,0,100,20));
        err.InsertCondition("Name", 5, ErrorCondition.Greater, LogicalCondition.None);
        err.InsertCondition("Image", 0, ErrorCondition.Greater, LogicalCondition.None);
	}
    /// <summary>
    /// Funciona que se llama cuando la ventana esta en focus
    /// </summary>
	void OnGUI() 
	{
        if (rpgobject == null || err == null)
            Init();
        GUI.enabled = false;
        path = GUI.TextField(new Rect(0,0, 300, 20),path);
        if (path.Length != 0)
            image.path = path;
        GUI.enabled = true;
        upload = GUI.Button (new Rect(300, 0, 100, 20), "Upload Photo");
        rpgobject.texture= new Texture2D(width, height);
        rpgobject.texture.LoadImage(image.GetImage);
		EditorGUI.LabelField(new Rect(0,30,100,20),new GUIContent("Name:"));
		rpgobject.name = EditorGUI.TextField(new Rect(90,30,200,20),rpgobject.name);
		EditorGUI.LabelField(new Rect(0,70,100,20),new GUIContent("Sprite Type:"));
		rpgobject.type = EditorGUI.Popup(new Rect(90,70,200,10),rpgobject.type,obtype);
		EditorGUI.LabelField(new Rect(0,90,100,20),new GUIContent("Description:"));
		EditorGUI.TextField(new Rect(0,110,290,120),ObjectTypes.GetDescription(obtype[rpgobject.type]));
		EditorGUI.LabelField(new Rect(300,70, 80,20),new GUIContent("Sprite Sheet:"));
		spritesheet = EditorGUI.Toggle(new Rect(300,90,20,20),spritesheet);
        EditorGUI.DrawPreviewTexture(new Rect(300, 110, width, height), rpgobject.texture);
        UpdateValidationVal();
		import = GUI.Button (new Rect(60, 240,300, 20), "Import Object");
        err.ShowErrors();
        if (!import && err.CheckErrors())
        {
            import = false;
        }
        
	}
    /// <summary>
    /// Actualiza los valores del manejador de errores
    /// </summary>
    void UpdateValidationVal()
    {
        err.UpdateValue("Name", rpgobject.name.Length);
        err.UpdateValue("Image", path.Length);
    }
    /// <summary>
    /// Funcion que se llama en cada frame
    /// </summary>
	void Update()
	{
        if (obtype[rpgobject.type] == "Background")
        {
            spritesheet = false;
        }
		if (upload) {
			path = EditorUtility.OpenFilePanel ("test", "test2", "jpg;*.png");
		}
		if (import) {
			oldname = rpgobject.name;
			rpgobject.name = obtype[rpgobject.type]+"_"+rpgobject.name;
            
			CreateRpgObject();
			rpgobject.name = oldname;
		}
	}
    /// <summary>
    /// Guarda el sprite en la carpeta de sprites
    /// </summary>
	void SaveSprites(){
        if (rpgobject.texture != null)
        {
            if (!spritesheet && obtype[rpgobject.type] != "Background")
                TextureScale.Bilinear(rpgobject.texture, 32, 32);
            var bytes = rpgobject.texture.EncodeToPNG();
			int startindex = image.path.LastIndexOf ('.');
			FileStream file = File.Open (Application.dataPath + "/Sprites/"+rpgobject.name+image.path.Substring (startindex), FileMode.Create);
			var binary = new BinaryWriter (file);
			binary.Write (bytes);
			file.Close ();
			AssetDatabase.Refresh();
		}
	}
    /// <summary>
    /// Retorna el path donde se va guardar la imagen
    /// </summary>
    /// <returns></returns>
	string GetPath(){
		int startindex = image.path.LastIndexOf ('.');
		return "Assets/Sprites/"+ rpgobject.name+image.path.Substring (startindex);
	}
    /// <summary>
    /// Crea el objeto RPG.
    /// </summary>
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