using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ImportImageUI : EditorWindow
{	
	FileBrowser image = new FileBrowser ("Assets/Sprites/96x96.jpg");
    int Type;
    Texture2D Texture;
    string Name;
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
        Name = string.Empty;
        err = new ErrorHandler();
        err.InsertPropertyError("Name",Name.Length, "The length of the name has to be greater than 1",new Rect(290,30,200,20));
        err.InsertPropertyError("Image", path.Length, "You have to select a image.",new Rect(410,0,100,20));
        err.InsertCondition("Name", 1, ErrorCondition.Greater, LogicalOperators.None);
        err.InsertCondition("Image", 0, ErrorCondition.Greater, LogicalOperators.None);
        obtype = ObjectTypes.GetTypes();
	}
    /// <summary>
    /// Funcion que se llama cuando la ventana esta en focus
    /// </summary>
	void OnGUI() 
	{
        if ( err == null || obtype == null )
            Init();
        GUI.enabled = false;
        path = GUI.TextField(new Rect(0,0, 300, 20),path);
        if (path.Length != 0)
            image.path = path;
        GUI.enabled = true;
        upload = GUI.Button (new Rect(300, 0, 100, 20), "Upload Photo");
        Texture= new Texture2D(width, height);
       Texture.LoadImage(image.GetImage);
		EditorGUI.LabelField(new Rect(0,30,100,20),new GUIContent("Name:"));
		Name = EditorGUI.TextField(new Rect(90,30,200,20),Name);
		EditorGUI.LabelField(new Rect(0,70,100,20),new GUIContent("Sprite Type:"));
		Type = EditorGUI.Popup(new Rect(90,70,200,10),Type,obtype);
		EditorGUI.LabelField(new Rect(0,90,100,20),new GUIContent("Description:"));
		EditorGUI.TextField(new Rect(0,110,290,120),ObjectTypes.GetDescription(obtype[Type]));
		EditorGUI.LabelField(new Rect(300,70, 80,20),new GUIContent("Sprite Sheet:"));
		spritesheet = EditorGUI.Toggle(new Rect(300,90,20,20),spritesheet);
        EditorGUI.DrawPreviewTexture(new Rect(300, 110, width, height), Texture);
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
        err.UpdateValue("Name", Name.Length);
        err.UpdateValue("Image", path.Length);
    }
    /// <summary>
    /// Funcion que se llama en cada frame
    /// </summary>
	void Update()
	{
        if (obtype[Type] == "Background")
        {
            spritesheet = false;
        }
		if (upload) {
			path = EditorUtility.OpenFilePanel ("test", "test2", "jpg;*.png");
		}
		if (import) {
			oldname = Name;
			Name = obtype[Type]+"_"+Name;
            
			CreateRpgObject();
			Name = oldname;
		}
	}
    /// <summary>
    /// Guarda el sprite en la carpeta de sprites
    /// </summary>
	void SaveSprites(){
        if (Texture != null)
        {
            if (!spritesheet && obtype[Type] != "Background")
                TextureScale.Bilinear(Texture, 32, 32);
            var bytes = Texture.EncodeToPNG();
			int startindex = image.path.LastIndexOf ('.');
			FileStream file = File.Open (Application.dataPath + "/Sprites/"+Name+image.path.Substring (startindex), FileMode.Create);
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
		return "Assets/Sprites/"+ Name+image.path.Substring (startindex);
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