﻿using UnityEngine;
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
    string type;
    /// <summary>
    /// El objeto que se va crear.
    /// </summary>
    GameObject CreateObj;
    /// <summary>
    /// Inicializa los valores de la ventana
    /// </summary>
    public void Init() {
        CreateObj = new GameObject();
        tile = CreateObj.AddComponent<Tile>();
        if (name == null)
            name = string.Empty;
    }
    /// <summary>
    /// Metodo que se llama cuando la ventana esta abierta.
    /// </summary>
    void OnGUI()
    {
        RenderLeftSide();
        GUILayout.BeginArea(new Rect((float)(this.position.width * 0.5), 0, 96, 96), string.Empty, EditorStyles.helpBox);
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect((float)(this.position.width*0.5), 96, (float)(this.position.width * 0.5), this.position.height-20), string.Empty, EditorStyles.helpBox);
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
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Item", GUILayout.Width(labelWidth))) 
            {

                var window = EditorWindow.GetWindow<ItemUI>();
                window.Initialize(ref pickup.ItemUsable);
                window.Show();

            }
            GUILayout.TextField(pickup.ItemUsable.ItemName);
            GUILayout.EndHorizontal();

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
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Select Inside Map", GUILayout.Width(labelWidth)))
            {
                var window = EditorWindow.GetWindow<MapUI>();
                window.Initialize(ref door.InMap);
                window.Show();
            }
            GUI.enabled = false;
            GUILayout.TextField(door.InMap.Name);
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Inside Map", GUILayout.Width(labelWidth)))
            {
                var window = EditorWindow.GetWindow<MapUI>();
                window.Initialize(ref door.OutMap);
                window.Show();
            }
            GUI.enabled = false;
            GUILayout.TextField(door.OutMap.Name);
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
        if (GUI.Button(new Rect(0, this.position.height-20, 100, 20), "Create"))
        {
            ClearFields();    
        }
        if (GUI.Button(new Rect((float)(this.position.width * 0.5), this.position.height - 20, 100, 20), "Save"))
        {
            if (Selected != null)
                SaveSelected();
            else
                CreateNew();
        }
        if (Selected != null)
            GUI.enabled = true;
        else
            GUI.enabled = false;
        if (GUI.Button(new Rect((float)(this.position.width * 0.5) +100, this.position.height - 20, 100, 20), "Delete"))
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
     void DeleteSelected()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/" + type + "/" + Selected.name + ".prefab");
    }

     void CreateNew()
    {

        switch (tab)
        {
            case 1:
                wall.gameObject.tag = tag;
                SpriteRenderer image1 =  wall.gameObject.AddComponent<SpriteRenderer>();
                image1.sortingLayerName = Constant.LAYER_ITEM;
                wall.gameObject.AddComponent<BoxCollider2D>();
                image1.sprite = wall.Icon;
                wall.Image = wall.Icon;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",wall.gameObject);
                DestroyImmediate(wall.gameObject);
                CreateObj = new GameObject();
                wall = CreateObj.AddComponent<Wall>();
                break;
            case 2:
                pickup.gameObject.tag = tag;
                SpriteRenderer image2 = pickup.gameObject.AddComponent<SpriteRenderer>();
                image2.sortingLayerName = Constant.LAYER_ITEM;
                pickup.gameObject.AddComponent<BoxCollider2D>();
                image2.sprite = pickup.Icon;
                pickup.Image = pickup.Icon;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",pickup.gameObject);
                DestroyImmediate(pickup.gameObject);
                CreateObj = new GameObject();
                pickup = CreateObj.AddComponent<Pickup>();
                
                break;
            case 3:
                SpriteRenderer image3 = obstacle.gameObject.AddComponent<SpriteRenderer>();
                obstacle.gameObject.AddComponent<BoxCollider2D>();
                image3.sortingLayerName = Constant.LAYER_ITEM;
                image3.sprite = obstacle.Icon;
                obstacle.Image = obstacle.Icon;
                obstacle.gameObject.tag = tag;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",obstacle.gameObject);
                 DestroyImmediate(obstacle.gameObject);
                 CreateObj = new GameObject();
                 obstacle = CreateObj.AddComponent<Obstacle>();
                break;
            case 4:
                Door temp = GameObject.Find("New Game Object").GetComponent<Door>();
                SpriteRenderer image4 = temp.gameObject.AddComponent<SpriteRenderer>();
                image4.sortingLayerName = Constant.LAYER_ITEM;
                image4.sprite = door.Icon;
                temp.InMap = door.InMap;
                temp.OutMap = door.OutMap;
                temp.Name = door.Name;
                temp.Icon = door.Icon;
                temp.Image = door.Icon;
                temp.gameObject.tag = tag;
                temp.gameObject.AddComponent<BoxCollider2D>();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",temp.gameObject);
                 DestroyImmediate(temp.gameObject);
                 CreateObj = new GameObject();
                 door = CreateObj.AddComponent<Door>();
                break;
            case 5:
                SpriteRenderer image5 = house.gameObject.AddComponent<SpriteRenderer>();
                image5.sortingLayerName = Constant.LAYER_ITEM;
                house.gameObject.AddComponent<BoxCollider2D>();
                image5.sprite = house.Icon;
                house.Image = house.Icon;
                house.gameObject.tag = tag;
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",house.gameObject);
                 DestroyImmediate(house.gameObject);
                 CreateObj = new GameObject();
                 house = CreateObj.AddComponent<House>();
                break;
            default:
                SpriteRenderer image = tile.gameObject.AddComponent<SpriteRenderer>();
                image.sortingLayerName = Constant.LAYER_TILE;
                 image.sprite = tile.Icon;
                 tile.Image = tile.Icon;
                 tile.gameObject.tag = tag;
                 PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+name+".prefab",tile.gameObject);
                 DestroyImmediate(tile.gameObject);
                 CreateObj = new GameObject();
                 tile = CreateObj.AddComponent<Tile>();
                 break;
        }
        ClearFields();
       
    }

     void SaveSelected()
    {
        var obj = Selected.GetComponent<RPGElement>();
        obj.Name = name;
        obj.Icon = texture;
        SpriteRenderer Image = Selected.gameObject.GetComponent<SpriteRenderer>();
        switch (tab)
        {
            case 1:
                var temp = obj as Wall;
                Image.sprite = temp.Image = wall.Icon;
                break;
            case 2:
                var temp6 = obj as Pickup;
                Image.sprite = temp6.Image = pickup.Icon;
                //pickup.Item= temp.Item; Falta el crud de items
                break;
            case 3:
                var temp1 = obj as Obstacle;
                temp1.hp = obstacle.hp ;
                break;
            case 4:
                Door aux = GameObject.Find("New Game Object").GetComponent<Door>();
                var temp2 = obj as Door;
                Image.sprite = temp2.Image = door.Icon;
                temp2.OutMap = door.OutMap;
                temp2.InMap = door.InMap;
                 DestroyImmediate(aux.gameObject);
                 CreateObj = new GameObject();
                 door = CreateObj.AddComponent<Door>();

                break;
            case 5:
                var temp3 = obj as House;
                Image.sprite = temp3.Image = house.Icon;
                temp3.Width = house.Width;
                temp3.Heigth = house.Heigth;
                break;
            default:
                var temp4 = obj as Tile;
                Image.sprite = temp4.Image = tile.Icon;
                break;
        }
        ClearFields();
    }
     void AddObject()
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
        GUILayout.BeginArea(new Rect(0, 0, (float)(this.position.width * 0.5), this.position.height-20), string.Empty, EditorStyles.helpBox);
        tab = GUILayout.Toolbar(tab, new string[] { "Tiles", "Walls", "PickUps", "Obstacles", "Doors", "Houses" });
        GUILayout.BeginVertical();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        switch (tab)
        {
            case 1:
                if (oldtab != tab)
                {
                    DestroyImmediate(CreateObj);
                    CreateObj = new GameObject();
                    wall = CreateObj.AddComponent<Wall>();
                }
                Objects = Resources.LoadAll("Wall", typeof(GameObject));
                type = "Wall";
                break;
            case 2:
                if (oldtab != tab)
                {
                    DestroyImmediate(CreateObj);
                    CreateObj = new GameObject();
                    pickup = CreateObj.AddComponent<Pickup>();
                    pickup.ItemUsable = new AbstractUsable();
                    
                }
                Objects = Resources.LoadAll("PickUp", typeof(GameObject));
                type = "PickUp";
                break;
            case 3:
                if (oldtab != tab)
                {
                    DestroyImmediate(CreateObj);
                    CreateObj = new GameObject();
                    obstacle = CreateObj.AddComponent<Obstacle>();
                }
                Objects = Resources.LoadAll("Obstacle", typeof(GameObject));
                type = "Obstacle";
                break;
            case 4:
                if (oldtab != tab)
                {
                    DestroyImmediate(CreateObj);
                    CreateObj = new GameObject();
                    door = CreateObj.AddComponent<Door>();
                }
                Objects = Resources.LoadAll("Door", typeof(GameObject));
                type = "Door";
                break;
            case 5:
                if (oldtab != tab)
                {
                    DestroyImmediate(CreateObj);
                    CreateObj = new GameObject();
                    house = CreateObj.AddComponent<House>();
                }
                Objects = Resources.LoadAll("House", typeof(GameObject));
                type = "House";
                break;

            default:
                if (oldtab != tab)
                {
                    DestroyImmediate(CreateObj);
                    CreateObj = new GameObject();
                    tile = CreateObj.AddComponent<Tile>();
                }
                Objects = Resources.LoadAll("Tile", typeof(GameObject));
                type = "Tile";
                break;
        }
        if (tab != oldtab)
        {
            oldtab = tab;
            ClearFields();
        }
        DrawObjectList();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

     void ClearFields()
    {
        textureName = string.Empty;
        texture = new Sprite();
        name = string.Empty;
        if ( tab == 3)
            obstacle.hp = 0;
        if (tab == 4)
        {
            door.InMap = new AbstractMap() ;
            door.OutMap = new AbstractMap() ;
        }
        if (tab == 5)
        {
            house.Width = 0;
            house.Heigth = 0;
        }
        Selected = null;
    }
     void OnDestroy()
     {
        DestroyImmediate(CreateObj);
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
     void updateFields(){
        var obj = Selected.GetComponent<RPGElement>();
        name = obj.Name;
        texture = obj.Icon;
        textureName = obj.Icon.name;
        switch (tab)
        { 
            case 1:
                var temp4 = obj as Wall;
                wall.Image = temp4.Image;
                break;
            case 2:
               var temp = obj as Pickup;
               pickup.Image = temp.Image;
                //pickup.Item= temp.Item; Falta el crud de items
                break;
            case 3:
                var temp1 = obj as Obstacle;
                obstacle.hp = temp1.hp;
                obstacle.Image = temp1.Image;
                break;
            case 4:
                var temp2 = obj as Door;
                door.OutMap = temp2.OutMap;
                door.InMap = temp2.InMap;
                door.Image = temp2.Image;
                break;
            case 5:

                var temp3 = obj as House;
                house.Width = temp3.Width;
                house.Heigth = temp3.Heigth;
                house.Image = temp3.Image;
                break;
            default:
                var temp5 = obj as Tile;
                tile.Image = temp5.Image;
                break;
        }

    }    
}
