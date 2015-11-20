using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// Ventana del CRUD de mapa
/// </summary>
public class MapUI : EditorWindow {
    /// <summary>
    /// Variable que se encarga de almacenar los valores de creacion y de crear, actualizar o eliminar un mapa.
    /// </summary>
	Map map;
    /// <summary>
    /// Variable que se encarga del manejo de errores
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
    /// Indica la posicion del scroll de la parte izquierda de la ventana
    /// </summary>
    Vector2 scrollPosition;
    /// <summary>
    /// La lista de mapas que se renderiza a la izquierda de la ventana
    /// </summary>
    List<KeyValuePair<string,string>> Maps = new List<KeyValuePair<string,string>>();
    /// <summary>
    /// Objeto que se utiliza al crear un mapa nuevo.
    /// </summary>
    GameObject temp;
    /// <summary>
    /// Mapa seleccionado, el key es el nombre del mapa y el value la ruta absoluta del mapa
    /// </summary>
    KeyValuePair<string, string> Selected;
    /// <summary>
    /// Variable que dice si hay algun error en la entrada de datos
    /// </summary>
    bool errores;
    /// <summary>
    /// Indica si esta en el modo de seleccion.
    /// 1 es para la seleccion del in map en la ventana de mapobjects doors
    /// 2 es para la seleccion del out map en la ventana de mapobjects doors
    /// </summary>
    int selectMode = 0 ;
    /// <summary>
    /// Objeto  que se selecciono en el modo de seleccion (mapin).
    /// </summary>
    public static string SelectMap1 = string.Empty;
    /// <summary>
    /// Objeto que se selecciono en el modo de seleccion (mapout).
    /// </summary>
    public static string SelectMap2 =string.Empty;
    /// <summary>
    /// Nombre de la scene que esta abierta cuando se abre la ventana.
    /// </summary>
    private string scene;
	public void Init () {
            Selected = new KeyValuePair<string, string>("", "");
            LoadList();
            temp = new GameObject();
            map = temp.AddComponent<Map>();
            InitErr();
            fontStyle.fontStyle = FontStyle.BoldAndItalic;
            scene = EditorApplication.currentScene;
	}
    private void InitErr(){
            err.InsertPropertyError("Heigth", map.Heigth, "The Heigth has to be greater than 5 and less than 10");
            err.InsertPropertyError("Width", map.Width, "The Width has to be greater than 5 and less than 17");
            err.InsertPropertyError("Name", map.Name.Length, "The length of the name has to be grater than 5");
            err.InsertCondition("Heigth", Constant.MIN_MAP_HEIGTH, ErrorCondition.GreaterOrEqual, LogicalOperators.AND);
            err.InsertCondition("Heigth", Constant.MAX_MAP_HEIGTH, ErrorCondition.LessOrEqual, LogicalOperators.None);
            err.InsertCondition("Width", Constant.MIN_MAP_WIDTH, ErrorCondition.GreaterOrEqual, LogicalOperators.AND);
            err.InsertCondition("Width", Constant.MAX_MAP_WIDTH, ErrorCondition.LessOrEqual, LogicalOperators.None);
            err.InsertCondition("Name", 5, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
    }

    /// <summary>
    /// Inicializa la ventana en modo de seleccion o en modo de creacion.
    /// </summary>
    /// <param name="Select">Modo en e que se va inicializar la ventana( true modo de seleccion, false modo de creacion)</param>
    public void Init(int mode)
    {
        scene = EditorApplication.currentScene;
        LoadList();
        temp = new GameObject();
        map = temp.AddComponent<Map>();
        InitErr();
        fontStyle.fontStyle = FontStyle.BoldAndItalic;
        Selected = new KeyValuePair<string, string>("", "");
        selectMode = mode;
    }
    /// <summary>
    /// Funcion que se llama cuando la ventana esta abierta. dibuja los objetos de la ventana.
    /// </summary>
	void OnGUI () {
        RenderLeftSide();
        if (selectMode != 0 )
            GUI.enabled = false;
        GUILayout.BeginArea(new Rect((float)(this.position.width * 0.3), 0, (float)(this.position.width * 0.7), this.position.height-20),EditorStyles.helpBox);
        err.ShowErrorsLayout();
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        map.Name = EditorGUILayout.TextField("Map Name", map.Name);
        map.Width = EditorGUILayout.IntField("Width", map.Width);
        map.Heigth = EditorGUILayout.IntField("Heigth", map.Heigth);
        UpdateValidationVal();
        GUILayout.EndArea();
        if (GUI.Button(new Rect(0, this.position.height - 20, 100, 20), "Create"))
        {
            ClearFields();
        }
        errores = err.CheckErrors();
        if (selectMode == 0)
        {
            if (GUI.Button(new Rect((float)(this.position.width * 0.3), this.position.height - 20, 100, 20), "Save") && !errores)
            {
                if (Selected.Value != "")
                    SaveSelected();
                else
                    CreateNew();
                ClearFields();
            }
            if (Selected.Value != "")
                GUI.enabled = true;
            else
                GUI.enabled = false;
            if (GUI.Button(new Rect((float)(this.position.width * 0.3) + 100, this.position.height - 20, 100, 20), "Delete"))
            {
                DeleteSelected();
                ClearFields();
            }

            GUI.enabled = true;
        }
        else
        {
            GUI.enabled = true;
            if (GUI.Button(new Rect((float)(this.position.width * 0.3) , this.position.height - 20, 100, 20), "Select"))
            {
                if (selectMode == 1)
                    SelectMap1 = Selected.Key;
                else if (selectMode == 2)
                    SelectMap2 = Selected.Key;
                DestroyImmediate(temp);
                this.Close();

            }
        }
	}
    /// <summary>
    /// Elimina el mapa seleccionado en la ventana
    /// </summary>
    private void DeleteSelected()
    {
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Directory.GetCurrentDirectory() + @"\Assets\Maps");
        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.FullName == Selected.Value && Maps.Count > 1 )
            {

                if (ComparePath(Selected.Value, EditorApplication.currentScene))
                {
                    DestroyImmediate(temp);
                    if (Selected.Value != Maps[0].Value)
                        EditorApplication.OpenScene(Maps[0].Value);
                    else
                        EditorApplication.OpenScene(Maps[1].Value);
                }
                file.Delete();
                Maps.Remove(Selected);

                temp = new GameObject();
                map = temp.AddComponent<Map>();
                
                break;
            }
        }
        Repaint();
    }
    /// <summary>
    /// Crea un mapa nuevo
    /// </summary>
    private void CreateNew()
    {
        string newpath = map.CreateMap();
        Maps = null;
        Maps = new List<KeyValuePair<string, string>>();
        LoadList();
        DestroyImmediate(temp);
        temp = new GameObject();
        map = temp.AddComponent<Map>();
        Repaint();
    }
    /// <summary>
    /// Actualiza el mapa seleccionado.
    /// </summary>
    private void SaveSelected()
    {
        map.updateMap(Selected.Value);
    }
    /// <summary>
    /// Limpia todos los campos de la ventana
    /// </summary>
    private void ClearFields()
    {
        map.Name = "";
        map.Width = 0;
        map.Heigth = 0;
        Selected = new KeyValuePair<string,string>("","");
        err.CheckErrors();
        UpdateValidationVal();
    }
    /// <summary>
    /// Renderiza la parte izquierda de la ventana
    /// </summary>
    void RenderLeftSide() {
        GUILayout.BeginArea(new Rect(0,0,(float)(this.position.width*0.3),this.position.height-20),EditorStyles.helpBox);
        GUILayout.Label("Maps", fontStyle);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DrawObjectList();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    /// <summary>
    /// Compara la ruta absoluta con una ruta relativa
    /// </summary>
    /// <param name="path1">ruta absoluta</param>
    /// <param name="path2">ruta relativa</param>
    private bool ComparePath(string absoulutePath, string relativePath)
    {
        return absoulutePath.Replace('\\', '/') == Directory.GetCurrentDirectory().Replace('\\', '/') + "/" + relativePath;
    }
    /// <summary>
    /// Dibuja la lista de mapas
    /// </summary>
    private void DrawObjectList()
    {
        foreach (var i in Maps)
        {
            if (GUILayout.Button(i.Key))
            {
                if (!ComparePath(i.Value,EditorApplication.currentScene))
                {
                    DestroyImmediate(temp);
                    EditorApplication.SaveScene();
                    EditorApplication.OpenScene(i.Value);
                    temp = new GameObject();
                    map = temp.AddComponent<Map>();

                }
                GameObject obj = GameObject.Find("Settings");
                Map aux = obj.GetComponent<Map>();
                map.Name = aux.Name;
                map.Width = aux.Width;
                map.Heigth = aux.Heigth;
                Selected = i;
            }
        }
    }
    /// <summary>
    /// Carga la lista de mapas
    /// </summary>
    void LoadList() {
        string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory()+@"\Assets\Maps");
        string aux = EditorApplication.currentScene;
        EditorApplication.SaveScene();
        foreach (string path in fileEntries)
        {
            if (path.Substring(path.LastIndexOf('.') + 1) == "meta")
                continue;
                if (!ComparePath(path, EditorApplication.currentScene))
                    EditorApplication.OpenScene(path);
                GameObject Obj = GameObject.Find("Settings");
                string key = Obj.GetComponent<Map>().Name;

                Maps.Add(new KeyValuePair<string, string>(key,path));
        }
        if (EditorApplication.currentScene != aux) 
            EditorApplication.OpenScene(aux);
        Repaint();
    }
    /// <summary>
    /// Funcion que se llama antes de que la ventana se cierre.
    /// </summary>
    void OnDestroy()
    {
        DestroyImmediate(temp);
        if (scene != EditorApplication.currentScene)
        {
            EditorApplication.SaveScene();
            EditorApplication.OpenScene(scene);
        }
        
    }
    /// <summary>
    /// Actualiza los campos en el manejador de errores.
    /// </summary>
    void UpdateValidationVal()
    {
        err.UpdateValue("Heigth", map.Heigth);
        err.UpdateValue("Width", map.Width);
        err.UpdateValue("Name", map.Name.Length);
    }
}
