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
    GameObject Selected;
    /// <summary>
    /// Variable que dice si hay algun error en la entrada de datos
    /// </summary>
    bool errores;
    /// <summary>
    /// Indica si esta en el modo de seleccion.
    /// 1 es para la seleccion del in map en la ventana de mapobjects doors
    /// 2 es para la seleccion del out map en la ventana de mapobjects doors
    /// </summary>
    bool selectMode;
    UnityEngine.Object[] Objs;
    /// <summary>
    /// Objeto  que se selecciono en el modo de seleccion (mapin).
    /// </summary>
    private AbstractMap SelectMap1;
    /// <summary>
    /// Objeto que se selecciono en el modo de seleccion (mapout).
    /// </summary>
    public static string SelectMap2 =string.Empty;
    /// <summary>
    /// Nombre de la scene que esta abierta cuando se abre la ventana.
    /// </summary>
    private string scene;
    bool SelectButton;
	public void Init () {
            Selected = null; 
            //LoadList();
            Objs = Resources.LoadAll("Maps", typeof(GameObject));
            temp = new GameObject();
            map = temp.AddComponent<Map>();
            InitErr();
            fontStyle.fontStyle = FontStyle.BoldAndItalic;
            scene = EditorApplication.currentScene;
	}
    private void InitErr(){
        err.InsertPropertyError("Heigth", map.Data.Heigth, "The Heigth has to be greater than 13 and less than 100");
        err.InsertPropertyError("Width", map.Data.Width, "The Width has to be greater than 17 and less than 100");
            err.InsertPropertyError("Name", map.Data.Name.Length, "The length of the name has to be grater than 1");
            err.InsertCondition("Heigth", Constant.MIN_MAP_HEIGTH, ErrorCondition.GreaterOrEqual, LogicalOperators.AND);
            err.InsertCondition("Heigth", Constant.MAX_MAP_HEIGTH, ErrorCondition.LessOrEqual, LogicalOperators.None);
            err.InsertCondition("Width", Constant.MIN_MAP_WIDTH, ErrorCondition.GreaterOrEqual, LogicalOperators.AND);
            err.InsertCondition("Width", Constant.MAX_MAP_WIDTH, ErrorCondition.LessOrEqual, LogicalOperators.None);
            err.InsertCondition("Name", 1, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
    }

    /// <summary>
    /// Inicializa la ventana en modo de seleccion o en modo de creacion.
    /// </summary>
    public void Initialize(ref AbstractMap val)
    {
        SelectMap1 = val;
        //SelectMap1 = "Prueba";
        selectMode = true;
        Init();
    }
    /// <summary>
    /// Funcion que se llama cuando la ventana esta abierta. dibuja los objetos de la ventana.
    /// </summary>
	void OnGUI () {
        RenderLeftSide();
        if (selectMode )
            GUI.enabled = false;
        GUILayout.BeginArea(new Rect((float)(this.position.width * 0.3), 0, (float)(this.position.width * 0.7), this.position.height-20),EditorStyles.helpBox);
        err.ShowErrorsLayout();
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        map.Data.Name = EditorGUILayout.TextField("Map Name", map.Data.Name);
        map.Data.Width = EditorGUILayout.IntField("Width", map.Data.Width);
        map.Data.Heigth = EditorGUILayout.IntField("Heigth", map.Data.Heigth);
        
        if (GUILayout.Button("Select Audio"))
        {
            EditorGUIUtility.ShowObjectPicker<>(null, false, "Background_", 1);
        }

        UpdateValidationVal();
        GUILayout.EndArea();
        if (GUI.Button(new Rect(0, this.position.height - 20, 100, 20), "Create"))
        {
            ClearFields();
        }
        errores = err.CheckErrors();
        if (!selectMode)
        {
            if (GUI.Button(new Rect((float)(this.position.width * 0.3), this.position.height - 20, 100, 20), "Save") && !errores)
            {
                if (Selected != null )
                    SaveSelected();
                else
                    CreateNew();
                ClearFields();
            }
            if (Selected != null)
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
            SelectButton = GUI.Button(new Rect((float)(this.position.width * 0.3), this.position.height - 20, 100, 20), "Select");
        }
	}
    /// <summary>
    /// Elimina el mapa seleccionado en la ventana
    /// </summary>
    private void DeleteSelected()
    {
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Directory.GetCurrentDirectory() + @"\Assets\Resources\Maps");
        var aux = Selected.GetComponent<Map>();
        foreach (FileInfo file in directory.GetFiles())
        {
            if (ComparePath(file.FullName,aux.Data.MapPath) && Objs.Length > 1)
            {

                if (aux.Data.MapPath == EditorApplication.currentScene)
                {
                    Constant.RemoveScene(aux.Data.MapPath);
                    DestroyImmediate(temp);
                    var newscene= (Objs[0] as GameObject).GetComponent<Map>();
                    if (aux.Data.MapPath != newscene.Data.MapPath)
                        EditorApplication.OpenScene(newscene.Data.MapPath);
                    else
                        EditorApplication.OpenScene((Objs[1] as GameObject).GetComponent<Map>().Data.MapPath);
                }
                file.Delete();
                AssetDatabase.DeleteAsset("Assets/Resources/Maps/" + aux.Id + ".prefab");
                Objs = Resources.LoadAll("Maps", typeof(GameObject));
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
        map.CreateMap();
        Maps = null;
        DestroyImmediate(temp);
        temp = new GameObject();
        map = temp.AddComponent<Map>();
        Objs = Resources.LoadAll("Maps", typeof(GameObject));
        Repaint();
    }
    void Update() {
        if (SelectButton)
        {
             var selectData = Selected.GetComponent<Map>().Data;
             SelectMap1.Heigth = selectData.Heigth;
             SelectMap1.MapPath= selectData.MapPath;
             SelectMap1.Width = selectData.Width;
             SelectMap1.Name = selectData.Name;
             this.Close();
        }
    }

    /// <summary>
    /// Actualiza el mapa seleccionado.
    /// </summary>
    private void SaveSelected()
    {
        var aux = Selected.GetComponent<Map>();
        map.updateMap(aux);
        aux.Data.Heigth = map.Data.Heigth;
        aux.Data.Width= map.Data.Width;
        aux.Name = aux.Data.Name = map.Data.Name;
        
    }
    /// <summary>
    /// Limpia todos los campos de la ventana
    /// </summary>
    private void ClearFields()
    {
        map.Data.Name = "";
        map.Data.Width = 0;
        map.Data.Heigth = 0;
        Selected = null;
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
        foreach (var i  in Objs)
        {   
            GameObject aux = i as GameObject;
            Map imap = aux.GetComponent<Map>();
            if (GUILayout.Button(imap.Data.Name))
            {
                if (!ComparePath(imap.Data.MapPath, EditorApplication.currentScene) && !selectMode)
                {
                    DestroyImmediate(temp);
                    EditorApplication.SaveScene();
                    EditorApplication.OpenScene(imap.Data.MapPath);
                    temp = new GameObject();
                    map = temp.AddComponent<Map>();

                }
                map.Data.Name = imap.Data.Name;
                map.Data.Width = imap.Data.Width;
                map.Data.Heigth = imap.Data.Heigth;
                map.Data.MapPath = imap.Data.MapPath;
                Selected = imap.gameObject;
            }
        }
    }
    /// <summary>
    /// Carga la lista de mapas
    /// </summary>
    void LoadList() {

        string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Assets\Maps");
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

            Maps.Add(new KeyValuePair<string, string>(key, path));
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
        err.UpdateValue("Heigth", map.Data.Heigth);
        err.UpdateValue("Width", map.Data.Width);
        err.UpdateValue("Name", map.Data.Name.Length);
    }
}
