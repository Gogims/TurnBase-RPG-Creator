using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
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
                    Objects = Resources.LoadAll("Obstacles", typeof(GameObject));
                    break;
                case 4:
                    Objects = Resources.LoadAll("Door", typeof(GameObject));
                    break;
                case 5:
                    Objects = Resources.LoadAll("Houses", typeof(GameObject));
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
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);
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
            SpriteRenderer texture = temp.GetComponent<SpriteRenderer>();
            Rect position = new Rect(x, y, 44, 44);
            GUI.DrawTexture(position, texture.sprite.texture);
            if (GUI.Button(position, "", new GUIStyle()))
            {
                Debug.Log(temp.tag);
                MapEditor.selectedObject = temp;
                GameEngine.inspectorRpg.Focus();
            }
            if (x + 84 + 64 < this.position.width)
                x += 84;
            else
            {
                y += 64;
                x = 10;
                GUILayout.Label("", GUILayout.Height(44 + 25));
            }
        }
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
        if (GUILayout.Button("Background")){
            optionSelected = "Background";
            mapObjects = false;
            Objects = Resources.LoadAll("Backround", typeof(GameObject));
        }
        GUILayout.EndArea();
    }
}
