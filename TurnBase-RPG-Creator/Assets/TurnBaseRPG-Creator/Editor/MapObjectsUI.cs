using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapObjectsUI : EditorWindow {
    /// <summary>
    /// Tag de los objetos creado en esta ventana
    /// </summary>
    string tag = "RPG-MAPOBJECT";
    /// <summary>
    /// Objetos que se van a mostrar en la ventana.
    /// </summary>
    Object[] Objects;
    /// <summary>
    /// Estilo de la letra.
    /// </summary>
    //GUIStyle fontStyle = new GUIStyle();
    /// <summary>
    /// Almacena el tab que se esta renderizando.
    /// </summary>
    int tab = 0;
    /// <summary>
    /// almacena el tab anterior al que se esta renderizando.
    /// </summary>
    int oldtab = 0;
    /// <summary>
    /// Contiene la posicion actual de la barra de scroll.
    /// </summary>
    Vector2 scrollPosition = Vector2.zero;
    /// <summary>
    /// Objeto seleccionado
    /// </summary>
    GameObject Selected;
    /// <summary>
    /// Imagen del objeto que se va crear
    /// </summary>
    Sprite texture;
    /// <summary>
    /// Nombre de la imagen.
    /// </summary>
    string textureName = string.Empty;
    /// <summary>
    /// Representa la pared que se va crear.
    /// </summary>
    Wall wall;
    /// <summary>
    /// Representa el pickup que se va crear
    /// </summary>
    Pickup pickup;
    /// <summary>
    /// Representa la loseta que se va crear.
    /// </summary>
    Tile tile;
    /// <summary>
    /// Representa el obstaculo que se va crear.
    /// </summary>
    Obstacle obstacle;
    /// <summary>
    /// Representa la puerta que se va crear.
    /// </summary>
    Door door;
    /// <summary>
    /// Representa la casa que se va crear.
    /// </summary>
    House house;
    /// <summary>
    /// Tamaño de los labels
    /// </summary>
    int labelWidth = 150;
    /// <summary>
    /// tipo de dato del tab seleccionado
    /// </summary>
    private string type;
    /// <summary>
    /// Inicializa los valores de la ventana
    /// </summary>
    /// 
    public void Init() {
        GameObject tempobj = new GameObject();
        tile = tempobj.AddComponent<Tile>();
        DestroyImmediate(tempobj);
        GameObject tempobj1 = new GameObject();
        obstacle = tempobj1.AddComponent<Obstacle>();
        DestroyImmediate(tempobj1);
        GameObject tempobj2 = new GameObject();
        door = tempobj2.AddComponent<Door>();
        DestroyImmediate(tempobj2);
        GameObject tempobj3 = new GameObject();
        house = tempobj3.AddComponent<House>();
        DestroyImmediate(tempobj3);
        GameObject tempobj4 = new GameObject();
        pickup = tempobj4.AddComponent<Pickup>();
        DestroyImmediate(tempobj4);
        GameObject tempobj5 = new GameObject();
        wall = tempobj5.AddComponent<Wall>();
        DestroyImmediate(tempobj5);
        if (name == null)
            name = string.Empty;
    }
    /// <summary>
    /// Metodo que se llama cuando la ventana esta abierta.
    /// </summary>
    void OnGUI()
    {
        if (tile == null || wall == null || pickup == null | house == null || obstacle == null || door == null)
            Init();
        RenderLeftSide();
        GUILayout.BeginArea(new Rect((float)(this.position.width * 0.5), 0, 96, 96), string.Empty, EditorStyles.helpBox);
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect((float)(this.position.width*0.5), 96, (float)(this.position.width * 0.5), this.position.height), string.Empty, EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:",GUILayout.Width(labelWidth));
        name = GUILayout.TextField(name);
        GUILayout.EndHorizontal();
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Select Image", GUILayout.Width(labelWidth))) 
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false,type, 1);

        }
        GUI.enabled = false;
        GUILayout.TextField(textureName);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
        AddObject();
        GUILayout.Space(15);
        if ( tab == 0 ){
            tile.Name = name;
            tile.Icon = texture;
        }
        else if (tab == 1) { 
            wall.Name = name;
            wall.Icon = texture;
            
        }
        else if (tab == 2 ){
            pickup.Name = name;
            pickup.Icon = texture;
            if (GUILayout.Button("Select Item", GUILayout.Width(labelWidth))) 
            {
            
                //TODO falta la ventana de item

            }
        }
        else if (tab == 3){
            GUILayout.BeginHorizontal();
            GUILayout.Label("Obstacle HP:",GUILayout.Width(labelWidth));
            obstacle.hp = EditorGUILayout.IntField(obstacle.hp);
            GUILayout.EndHorizontal();
            obstacle.Name = name;
            obstacle.Icon = texture;
        }
        else if (tab == 4 ){
            door.Name = name;
            door.Icon = texture;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Inside Map", GUILayout.Width(labelWidth)))
            {

                //TODO falta la ventana de item

            }
            GUI.enabled = false;
            GUILayout.TextField(door.InMap);
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Outside Map", GUILayout.Width(labelWidth)))
            {

                //TODO falta la ventana de item

            }
            GUI.enabled = false;
            GUILayout.TextField(door.OutMap);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

        }
        else if (tab == 5) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("House Width:", GUILayout.Width(labelWidth));
            house.Width = EditorGUILayout.IntField(house.Width);
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            GUILayout.Label("House Heigth:", GUILayout.Width(labelWidth));
            house.Heigth = EditorGUILayout.IntField(house.Heigth);
            GUILayout.EndHorizontal();
            house.Name = name;
            house.Icon = texture;
        }

        GUILayout.EndArea();
        if (GUI.Button(new Rect((float)(this.position.width * 0.5), this.position.height - 20, 100, 20), "New"))
        {
            ClearFields();    
        }
        if (GUI.Button(new Rect((float)(this.position.width * 0.5) + 105, this.position.height - 20, 100, 20), "Save"))
        {
            if (Selected != null)
                SaveSelected();
            else
                CreateNew();
            ClearFields();
        }
        if (Selected != null)
            GUI.enabled = true;
        else
            GUI.enabled = false;
        if (GUI.Button(new Rect((float)(this.position.width * 0.5) + 210, this.position.height - 20, 100, 20), "Delete"))
        {
            DeleteSelected();
            ClearFields();
        }
        GUI.enabled = true;
            
        if (texture != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect((float)(this.position.width * 0.5), 0, 96, 96), texture.texture, GetTextureCoordinate(texture));
        }
    }

    private void DeleteSelected()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/" + type + "/" + Selected.name + ".prefab");
    }

    private void CreateNew()
    {

        switch (tab)
        {
            case 1:
                wall.gameObject.tag = tag;
                SpriteRenderer image1 =  wall.gameObject.AddComponent<SpriteRenderer>();
                image1.sprite = wall.Icon;
                 PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",wall.gameObject);
                 DestroyImmediate(wall.gameObject);
                break;
            case 2:
                pickup.gameObject.tag = tag;
                SpriteRenderer image2 = pickup.gameObject.AddComponent<SpriteRenderer>();
                image2.sprite = pickup.Icon;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",pickup.gameObject);
                 DestroyImmediate(pickup.gameObject);
                break;
            case 3:
                SpriteRenderer image3 = obstacle.gameObject.AddComponent<SpriteRenderer>();
                image3.sprite = obstacle.Icon;
                obstacle.gameObject.tag = tag;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",obstacle.gameObject);
                 DestroyImmediate(obstacle.gameObject);
                break;
            case 4:
                SpriteRenderer image4 = door.gameObject.AddComponent<SpriteRenderer>();
                image4.sprite = door.Icon;
                door.gameObject.tag = tag;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",door.gameObject);
                 DestroyImmediate(door.gameObject);
                break;
            case 5:
                SpriteRenderer image5 = house.gameObject.AddComponent<SpriteRenderer>();
                image5.sprite = house.Icon;
                house.gameObject.tag = tag;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",house.gameObject);
                 DestroyImmediate(house.gameObject);
                break;
            default:
                SpriteRenderer image = tile.gameObject.AddComponent<SpriteRenderer>();
                 image.sprite = tile.Icon;
                 tile.gameObject.tag = tag;
                 PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",tile.gameObject);
                 DestroyImmediate(tile.gameObject);
                 break;
        }
       
       
    }

    private void SaveSelected()
    {
        var obj = Selected.GetComponent<RPGElement>();
        obj.Name = name;
        obj.Icon = texture; 
        switch (tab)
        {
            case 2:
                //pickup.Item= temp.Item; Falta el crud de items
                break;
            case 3:
                var temp1 = obj as Obstacle;
                temp1.hp = obstacle.hp ;
                break;
            case 4:
                var temp2 = obj as Door;
                temp2.OutMap = door.OutMap;
                temp2.InMap = door.InMap;
                break;
            case 5:
                var temp3 = obj as House;
                temp3.Width = house.Width;
                temp3.Heigth = house.Heigth;
                break;
        }
    }
    private void AddObject()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated") 
        {
            texture = (Sprite)EditorGUIUtility.GetObjectPickerObject();

            if (texture != null)
            {
                textureName = texture.name;
              

            }
            Repaint();
        }
    }
    public Rect GetTextureCoordinate(Sprite T )
    {
        return new Rect(T.textureRect.x / T.texture.width,
                        T.textureRect.y / T.texture.height,
                        T.textureRect.width / T.texture.width,
                        T.textureRect.height / T.texture.height);
    }
    /// <summary>
    /// Renderisa la parte izquierda de la ventana.
    /// </summary>
    void RenderLeftSide()
    {

        GUILayout.BeginArea(new Rect(0, 0, (float)(this.position.width * 0.5), this.position.height), string.Empty, EditorStyles.helpBox);
        tab = GUILayout.Toolbar(tab, new string[] { "Tiles", "Walls", "PickUps", "Obstacles", "Doors", "Houses" });
        GUILayout.BeginVertical();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);
        if (tab != oldtab)
        {
            oldtab = tab;
            ClearFields();
        }
        switch (tab)
        {
            case 1:
                Objects = Resources.LoadAll("Wall", typeof(GameObject));
                type = "Wall";
                break;
            case 2:
                Objects = Resources.LoadAll("PickUp", typeof(GameObject));
                type = "PickUp";
                break;
            case 3:
                Objects = Resources.LoadAll("Obstacle", typeof(GameObject));
                type = "Obstacle";
                break;
            case 4:
                Objects = Resources.LoadAll("Door", typeof(GameObject));
                type = "Door";
                break;
            case 5:
                Objects = Resources.LoadAll("House", typeof(GameObject));
                type = "House";
                break;

            default:
                Objects = Resources.LoadAll("Tile", typeof(GameObject));
                type = "Tile";
                break;
        }
        DrawObjectList();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void ClearFields()
    {
        textureName = string.Empty;
        texture = new Sprite();
        name = string.Empty;
        obstacle.hp = 0;
        door.InMap = string.Empty;
        door.OutMap = string.Empty;
        house.Width = 0;
        house.Heigth = 0;
        Selected = null;
    }

    private void RenderRightSide(string p)
    {
        
    }
    /// <summary>
    /// Dibuja los objetos que estan en el arreglo
    /// </summary>
    void DrawObjectList() 
    {
        int x = 10;
        int y = 26;
        foreach (var obj in Objects)
        {
            GameObject temp = (GameObject)obj;
            if (temp.tag != "RPG-MAPOBJECT")
                continue;
            RPGElement comp = temp.GetComponent<RPGElement>();
            Sprite sprite = comp.Icon;
            Rect position = new Rect(x, y, 64, 64);
            GUI.DrawTextureWithTexCoords(position, sprite.texture,GetTextureCoordinate(sprite));
            if (comp.name.Length > 8)
                GUI.Label(new Rect(x, y + 74, 64, 20), comp.Name.Substring(0, 6) + "...");
            else
                GUI.Label(new Rect(x, y + 74, 64, 20), comp.Name);
            if (GUI.Button(position, "", new GUIStyle()))
            {
                Selected = temp;
                updateFields();
            }
            if (x + 84 +112 < this.position.width)
                x += 84;
            else
            {
                GUILayout.Label("", GUILayout.Height(64 + 25), GUILayout.Width(x+60));
                y += 84;
                x = 10;
            }
        }
    }
    private void updateFields(){
        var obj = Selected.GetComponent<RPGElement>();
        name = obj.Name;
        Debug.Log(name);
        texture = obj.Icon;
        textureName = obj.Icon.name;
        switch (tab)
        { 
            case 2:
               // var temp = obj as Pickup;
                //pickup.Item= temp.Item; Falta el crud de items
                break;
            case 3:
                var temp1 = obj as Obstacle;
                obstacle.hp = temp1.hp;
                break;
            case 4:
                var temp2 = obj as Door;
                door.OutMap = temp2.OutMap;
                door.InMap = temp2.InMap;
                break;
            case 5:
                var temp3 = obj as House;
                house.Width = temp3.Width;
                house.Heigth = temp3.Heigth;
                break;
        }

    }    
}

