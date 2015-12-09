using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class ImportAudioUI : EditorWindow
{
    FileBrowser sound = new FileBrowser();
    string Name = string.Empty;
    bool upload = false;
	bool import=false;
    ErrorHandler err;
    GameObject prefab;

    public string AudioName = string.Empty;
    public Constant.AudioType Type;

    /// <summary>
    /// Incializa la ventana.
    /// </summary>
	public void Init()
    {
        prefab = new GameObject();
        err = new ErrorHandler();
        //err.InsertPropertyError("Name", Name.Length, "The length of the name has to be greater than 1");
        //err.InsertPropertyError("Image", sound.path.Length, "You have to select a image.");
        //err.InsertCondition("Name", 1, ErrorCondition.Greater, LogicalOperators.None);
        //err.InsertCondition("Image", 0, ErrorCondition.Greater, LogicalOperators.None);
    }

    /// <summary>
    /// Funcion que se llama cuando la ventana esta en focus
    /// </summary>
	void OnGUI() 
	{
        GUILayout.BeginArea(new Rect(0, 0, 500, 300), "Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        //err.ShowErrorsLayout();

        GUILayout.BeginHorizontal();
        GUI.enabled = false;
        EditorGUILayout.TextField("", sound.path);
        GUI.enabled = true;
        upload = GUILayout.Button("Upload Sound");
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        Name = EditorGUILayout.TextField("Name:", Name);
        Type = (Constant.AudioType)EditorGUILayout.EnumPopup("Type:", Type);

        GUI.enabled = false;
        EditorGUILayout.TextArea(GetDescription());
        GUI.enabled = true;

        GUILayout.Space(15);
        import = GUILayout.Button("Import Audio");
        GUILayout.EndArea();
        
        if (!import && err.CheckErrors())
        {
            import = false;
        }

        //UpdateValidationVal();
    }

    /// <summary>
    /// Actualiza los valores del manejador de errores
    /// </summary>
    //void UpdateValidationVal()
    //{
    //    err.UpdateValue("Name", Name.Length);
    //    err.UpdateValue("Image", sound.path.Length);
    //}

    /// <summary>
    /// Funcion que se llama en cada frame
    /// </summary>
	void Update()
	{
        if (prefab == null) Init();        

        if (upload)
        {
			sound.path = EditorUtility.OpenFilePanel("test", "test2", "mp3;*.ogg;*.wav");            
		}

		if (import)
        {
			AudioName = Type.ToString() + "_" + Name;
            CreateAudioSource();
		}
	}

    /// <summary>
    /// Crea el objeto RPG.
    /// </summary>
	void CreateAudioSource()
    {
        int index = sound.path.LastIndexOf('.');
        string extension = sound.path.Substring(index);
        string tempFile = Application.dataPath + "/Resources/Audio/Files/" + AudioName + extension;
        System.IO.File.WriteAllBytes(tempFile, sound.File);
	}

    private string GetDescription()
    {
        string description = string.Empty;

        switch (Type)
        {
            case Constant.AudioType.Background:
                description = "Background sound that has the lowest volume and loops forever.";
                break;
            case Constant.AudioType.BackgroundEffect:
                description = "Sound above the background sound (volumne wise) and loops forever.";
                break;
            case Constant.AudioType.Sound:
                description = "Sound above all other sounds and plays once.";
                break;
        }

        return description;
    }
}