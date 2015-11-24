using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// Clase que se encarga de mostrar la ventana de objectBrowser.
/// </summary>
public class ObjectBrowserUI : EditorWindow
{
    /// <summary>
    /// Objetos que se van a mostrar en la ventana.
    /// </summary>
    Object[] Objects;
    /// <summary>
    /// Estilo de la letra.
    /// </summary>
    GUIStyle fontStyle = new GUIStyle();
    /// <summary>
    /// Indica cual tab de mapObjects se va mostrar
    /// </summary>
    int tab = 0;
    /// <summary>
    /// Indica si el boton de mapobject fue presionado.
    /// </summary>
    bool mapObjects = true;
    /// <summary>
    /// Contiene la posicion actual de la barra de scroll.
    /// </summary>
    Vector2 scrollPosition = Vector2.zero;
    /// <summary>
    /// Contiene el nombre de la opcion que se selecciono
    /// </summary>
    string optionSelected = "";
    /// <summary>
    /// Funcion para inicializar los valores de la ventana.
    /// </summary>
    public void Init() {
        mapObjects = true;
        optionSelected = "Map Objects";
        Objects = Resources.LoadAll("Tile",typeof(GameObject));
        fontStyle.fontStyle = FontStyle.BoldAndItalic;
    }
    /// <summary>
    /// Function que se llama cuando la ventana esta abierta
    /// </summary>
    void OnGUI(){
        if (Objects == null)
            Init();
        RenderLeftSide();
        int diff = 0;
        if (mapObjects)
        {
            diff = 25;
            GUILayout.BeginArea(new Rect(300, 0, this.position.width - 300, 25), string.Empty, EditorStyles.helpBox);
            tab = GUILayout.Toolbar(tab, new string[] { "Tiles", "Walls", "PickUps", "Obstacles", "Doors", "Houses" });
            switch (tab)
            {
                case 1:
                    Objects = Resources.LoadAll("Wall", typeof(GameObject));
                    break;
                case 2:
                    Objects = Resources.LoadAll("PickUp", typeof(GameObject));
                    break;
                case 3:
                    Objects = Resources.LoadAll("Obstacle", typeof(GameObject));
                    break;
                case 4:
                    Objects = Resources.LoadAll("Door", typeof(GameObject));
                    break;
                case 5:
                    Objects = Resources.LoadAll("House", typeof(GameObject));
                    break;
                   
                default:
                    Objects = Resources.LoadAll("Tile", typeof(GameObject));
                    break;
            }
            GUILayout.EndArea();
        }
        GUILayout.BeginArea(new Rect(300, diff, this.position.width - 300, this.position.height-diff), string.Empty, EditorStyles.helpBox);
        GUILayout.Label(optionSelected, fontStyle);
        GUILayout.BeginVertical();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DrawObjectList();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    /// <summary>
    /// Dibuja los objetos que estan en el arreglo
    /// </summary>
    void DrawObjectList()
    {
        int x = 10;
        int y = 10;
        foreach (var obj in Objects)
        {
           
            GameObject temp = (GameObject)obj;
            if (temp.tag == "RPG-CORE") continue;
            RPGElement ob = temp.GetComponent<RPGElement>();
            Rect position = new Rect(x, y, 44, 44);
            if (ob.Icon != null)
            {
                GUI.DrawTextureWithTexCoords(position, ob.Icon.texture, GetTextureCoordinate(ob.Icon)); 
            }
            if (ob.Name.Length > 6)
                GUI.Label(new Rect(x, y + 54, 44, 20), ob.Name.Substring(0, 4) + "...");
            else
                GUI.Label(new Rect(x, y + 54, 44, 20), ob.Name);
            if (GUI.Button(position, "", new GUIStyle()))
            {
                MapEditor.selectedObject = temp;
                Selection.activeObject = temp;
                GameEngine.inspectorRpg.Focus();
                if (optionSelected == "Maps")
                {
                    string OpenScene = temp.GetComponent<Map>().Data.MapPath;
                    if (OpenScene != Directory.GetCurrentDirectory() + "\\" + EditorApplication.currentScene.Replace('/', '\\'))
                    {
                        EditorApplication.SaveScene();
                        EditorApplication.OpenScene(OpenScene);
                    }
                }

            }
            if (x + 84 + 64 < this.position.width)
                x += 84;
            else
            {
                GUILayout.Label("", GUILayout.Height(44 + 25), GUILayout.Width(x+25));
                y += 64;
                x = 10;
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
    /// <summary>
    /// Renderisa la parte izquierda de la ventana.
    /// </summary>
    void RenderLeftSide() {
        GUILayout.BeginArea(new Rect(0, 0, 300,this.position.height),string.Empty,EditorStyles.helpBox);
        GUILayout.Label("Objects", fontStyle);
        GUILayout.Space(10);
        if (GUILayout.Button("Map Objects"))
        {
            mapObjects = true;
            optionSelected = "Map Objects";
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Player")) {
            mapObjects = false;
            Objects = Resources.LoadAll("Player", typeof(GameObject));
            optionSelected = "Player";
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Armor")) {
            mapObjects = false;
            optionSelected = "Armor";
            Objects = Resources.LoadAll("Armor", typeof(GameObject));
        }
        GUILayout.Space(10);
        if ( GUILayout.Button("Weapon")){
            mapObjects = false;
            optionSelected = "Weapon";
            Objects = Resources.LoadAll("Weapon", typeof(GameObject));
        }
        GUILayout.Space(10);
        if ( (GUILayout.Button("Item"))){
            mapObjects = false;
            optionSelected = "Item";
            Objects = Resources.LoadAll("Item", typeof(GameObject));
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Maps")){
            optionSelected = "Maps";
            mapObjects = false;
            Objects = Resources.LoadAll("Maps", typeof(GameObject));
        }
        GUILayout.EndArea();
    }
}
