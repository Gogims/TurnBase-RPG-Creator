using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

public class MapUI : EditorWindow {
    
    /// <summary>
    /// 
    /// </summary>
	Map map;
    /// <summary>
    /// 
    /// </summary>
    ErrorHandler err = new ErrorHandler();
    /// <summary>
    /// 
    /// </summary>
    bool create = false;
    /// <summary>
    /// Estilo de la letra.
    /// </summary>
    GUIStyle fontStyle = new GUIStyle();
    /// <summary>
    /// 
    /// </summary>
    private Vector2 scrollPosition;
    Dictionary<string,string> Maps = new Dictionary<string,string>();
    GameObject temp;
    private string SelectedPath = "";
    private string SelectedKey = "";
    private bool errores;
	public void Init () {
       
        temp = new GameObject();
        LoadList();
        map = temp.AddComponent<Map>();
        err.InsertPropertyError("Heigth", map.Heigth, "The Heigth has to be greater than 5 and less than 10");
        err.InsertPropertyError("Width", map.Width, "The Width has to be greater than 5 and less than 17");
        err.InsertPropertyError("Name", map.Name.Length, "The length of the name has to be grater than 5");
        err.InsertCondition("Heigth", Constant.MIN_MAP_HEIGTH, ErrorCondition.GreaterOrEqual, LogicalOperators.AND);
        err.InsertCondition("Heigth", Constant.MAX_MAP_HEIGTH, ErrorCondition.LessOrEqual, LogicalOperators.None);
        err.InsertCondition("Width", Constant.MIN_MAP_WIDTH, ErrorCondition.GreaterOrEqual, LogicalOperators.AND);
        err.InsertCondition("Width", Constant.MAX_MAP_WIDTH, ErrorCondition.LessOrEqual, LogicalOperators.None);
        err.InsertCondition("Name", 5, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        fontStyle.fontStyle = FontStyle.BoldAndItalic;
       
	}
	void OnGUI () {

        RenderLeftSide();
        GUILayout.BeginArea(new Rect((float)(this.position.width * 0.5), 0, (float)(this.position.width * 0.5), this.position.height));
        err.ShowErrorsLayout();      
		GUILayout.Label ("Settings", EditorStyles.boldLabel);
		map.Name = EditorGUILayout.TextField ("Map Name",map.Name);
		map.Width = EditorGUILayout.IntField ("Width", map.Width);
		map.Heigth = EditorGUILayout.IntField ("Heigth", map.Heigth);
        UpdateValidationVal();
        GUILayout.EndArea();
        if (GUI.Button(new Rect((float)(this.position.width * 0.5), this.position.height - 20, 100, 20), "New"))
        {
            ClearFields();
        }
        errores = err.CheckErrors();
        if (GUI.Button(new Rect((float)(this.position.width * 0.5) + 105, this.position.height - 20, 100, 20), "Save") && !errores)
        {
            if (SelectedPath != "")
                SaveSelected();
            else
               CreateNew();
            ClearFields();
        }
        if (SelectedPath != "")
            GUI.enabled = true;
        else
            GUI.enabled = false;
        if (GUI.Button(new Rect((float)(this.position.width * 0.5) + 210, this.position.height - 20, 100, 20), "Delete"))
        {
            DeleteSelected();
            ClearFields();
        }

        GUI.enabled = true;
	}

    private void DeleteSelected()
    {
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Directory.GetCurrentDirectory() + @"\Assets\Maps");
        foreach (FileInfo file in directory.GetFiles())
            if (file.FullName == SelectedPath)
            {
                file.Delete();
                Maps.Remove(SelectedKey);
                break;
            }

        LoadList();
    }

    private void CreateNew()
    {
        map.CreateMap();
        DestroyImmediate(temp);
        temp = new GameObject();
        LoadList();
    }

    private void SaveSelected()
    {
        map.updateMap(SelectedPath);
    }

    private void ClearFields()
    {
        map.Name = "";
        map.Width = 0;
        map.Heigth = 0;
        SelectedPath = "";
        SelectedKey = "";
        err.CheckErrors();
        UpdateValidationVal();
    }
    void RenderLeftSide() {
        GUILayout.BeginArea(new Rect(0,0,(float)(this.position.width*0.5),this.position.height));
        GUILayout.Label("Maps", fontStyle);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);
        DrawObjectList();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void DrawObjectList()
    {
        foreach (var key in Maps.Keys)
        {
            if (GUILayout.Button(key))
            {
                string temp = EditorApplication.currentScene;
                EditorApplication.OpenScene(Maps[key]);
                GameObject obj = GameObject.Find("Settings");
                Map aux = obj.GetComponent<Map>();
                map.Name = aux.Name;
                map.Width = aux.Width;
                map.Heigth = aux.Heigth;
                SelectedPath = Maps[key];
                SelectedKey = key;
                EditorApplication.OpenScene(temp);
            }
        }
    }
    public Rect GetTextureCoordinate(Sprite T)
    {
        return new Rect(T.textureRect.x / T.texture.width,
                        T.textureRect.y / T.texture.height,
                        T.textureRect.width / T.texture.width,
                        T.textureRect.height / T.texture.height);
    }
    void LoadList() {

        string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory()+@"\Assets\Maps");
        foreach (string path in fileEntries)
        {
            if (path.Substring(path.LastIndexOf('.') + 1) == "meta")
                continue;
                string key = path.Substring(path.LastIndexOf('\\') + 1);
                key = key.Substring(0, key.LastIndexOf('.') );

                if (Maps.ContainsKey(key))
                {
                    Maps[key] = path;
                }
                else
                {
                    Maps.Add(key, path);
                }
        }
    }
    void OnDestroy()
    {
        if (temp != null)
        {
            DestroyImmediate(temp);
        }
        
		this.Close ();
	}
    void UpdateValidationVal()
    {
        err.UpdateValue("Heigth", map.Heigth);
        err.UpdateValue("Width", map.Width);
        err.UpdateValue("Name", map.Name.Length);
    }
}
