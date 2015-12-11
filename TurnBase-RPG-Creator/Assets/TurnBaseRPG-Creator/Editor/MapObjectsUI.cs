using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class MapObjectsUI : EditorWindow {
    /// <summary>
    /// Tag de los objetos creado en esta ventana
    /// </summary>
    string tag = "RPG-MAPOBJECT";
    /// <summary>
    /// Objetos que se van a mostrar en la ventana.
    /// </summary>
    UnityEngine.Object[] Objects;
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
    /// Tipo del item
    /// </summary>
    Constant.ItemType itemType;
    /// <summary>
    /// Tipo de objeto
    /// </summary>
    Constant.MapObjectType MapObjectType;
    /// <summary>
    /// Nombre del pickup
    /// </summary>
    private string pickupName;
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
            MapObjectType = Constant.MapObjectType.Tile;
        }
        else if (tab == 1) { 
            wall.Name = name;
            wall.Icon = texture;
            MapObjectType = Constant.MapObjectType.Wall;
        }
        else if (tab == 2 ){
            pickup.Name = name;
            pickup.Icon = texture;
            MapObjectType = Constant.MapObjectType.Pickup;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Sound", GUILayout.Width(labelWidth)))
            {
                EditorGUIUtility.ShowObjectPicker<AudioClip>(null, false, "Sound_", 2);
            }
            string soundName = pickup.Sound == null ? string.Empty : pickup.Sound.name;
            GUI.enabled = false;
            EditorGUILayout.TextField(soundName);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            itemType = (Constant.ItemType)EditorGUILayout.EnumPopup("Select Item Type", itemType);
            GUILayout.EndHorizontal();
            if (itemType == Constant.ItemType.Armor)
            {
                pickup.ItemArmor.Type = (AbstractArmor.ArmorType)EditorGUILayout.EnumPopup("Armor Type: ", pickup.ItemArmor.Type);

            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Item", GUILayout.Width(labelWidth))) 
            {
                pickup.ItemArmor = new AbstractArmor();
                pickup.ItemUsable = new AbstractUsable();
                pickup.ItemWeapon = new AbstractWeapon();
                switch (itemType) 
                { 
                    case Constant.ItemType.Armor:
                        var window = EditorWindow.GetWindow<ArmorUI>();
                        window.Selected = true;
                        window.Initialize(ref pickup.ItemArmor, pickup.ItemArmor.Type);
                        window.Show();
                        break;
                    case Constant.ItemType.Usable:
                        var window2 = EditorWindow.GetWindow<ItemUI>();
                        window2.Selected = true;
                        window2.Initialize(ref pickup.ItemUsable);
                        window2.Show();
                        break;
                    case Constant.ItemType.Weapon:
                        var window3 = EditorWindow.GetWindow<WeaponUI>();
                        window3.Selected = true;
                        window3.Initialize(ref pickup.ItemWeapon);
                        window3.Show();
                        break;
                }
            }
            pickupName = pickup.ItemArmor.ItemName;
            GUI.enabled = false;
            GUILayout.TextField(pickupName);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

        }
        else if (tab == 3){
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Sound", GUILayout.Width(labelWidth)))
            {
                EditorGUIUtility.ShowObjectPicker<AudioClip>(null, false, "Sound_", 3);
            }
            string soundName = obstacle.Sound == null ? string.Empty : obstacle.Sound.name;
            GUI.enabled = false;
            EditorGUILayout.TextField(soundName);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Obstacle HP:",GUILayout.Width(labelWidth));
            obstacle.hp = EditorGUILayout.IntField(obstacle.hp);
            GUILayout.EndHorizontal();
            obstacle.Name = name;
            obstacle.Icon = texture;
            MapObjectType = Constant.MapObjectType.Obstacle;
        }
        else if (tab == 4 ){
            door.Name = name;
            door.Icon = texture;
            MapObjectType = Constant.MapObjectType.Door;
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
            MapObjectType = Constant.MapObjectType.House;
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
            GUI.DrawTextureWithTexCoords(new Rect((float)(this.position.width * 0.5), 0, 96, 96), texture.texture, Constant.GetTextureCoordinate(texture));
        }
    }
     void DeleteSelected()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/" + type + "/" + Selected.GetComponent<RPGElement>().Id + ".prefab");
    }

     void CreateNew()
    {

        switch (tab)
        {
            case 1:
                wall.gameObject.tag = tag;
                SpriteRenderer image1 =  wall.gameObject.AddComponent<SpriteRenderer>();
                image1.sortingLayerName = Constant.LAYER_ITEM;
                image1.sprite = wall.Icon;
                BoxCollider2D collider = wall.gameObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(image1.sprite.rect.width, image1.sprite.rect.height);
                wall.Image = wall.Icon;
                wall.Id = Guid.NewGuid().ToString();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/"+wall.Id+".prefab",wall.gameObject);
                DestroyImmediate(wall.gameObject);
                CreateObj = new GameObject();
                wall = CreateObj.AddComponent<Wall>();
                break;
            case 2:
                pickup.gameObject.tag = tag;
                SpriteRenderer image2 = pickup.gameObject.AddComponent<SpriteRenderer>();
                image2.sortingLayerName = Constant.LAYER_ITEM;
                image2.sprite = pickup.Icon;
                BoxCollider2D collider2 = pickup.gameObject.AddComponent<BoxCollider2D>();
                collider2.size = new Vector2(image2.sprite.rect.width, image2.sprite.rect.height);
                pickup.Image = pickup.Icon;
                pickup.Id = Guid.NewGuid().ToString();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/" + pickup.Id + ".prefab", pickup.gameObject);
                DestroyImmediate(pickup.gameObject);
                CreateObj = new GameObject();
                pickup = CreateObj.AddComponent<Pickup>();
                
                break;
            case 3:
                SpriteRenderer image3 = obstacle.gameObject.AddComponent<SpriteRenderer>();
                image3.sortingLayerName = Constant.LAYER_ITEM;
                image3.sprite = obstacle.Icon;          
                BoxCollider2D collider3 = obstacle.gameObject.AddComponent<BoxCollider2D>();
                collider3.size = new Vector2(image3.sprite.rect.width, image3.sprite.rect.height);
                obstacle.Image = obstacle.Icon;
                obstacle.gameObject.tag = tag;
                obstacle.Id = Guid.NewGuid().ToString();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/" + obstacle.Id + ".prefab", obstacle.gameObject);
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
                temp.Name = door.Name;
                temp.Icon = door.Icon;
                temp.Image = door.Icon;
                temp.gameObject.tag = tag;
                BoxCollider2D collider4 = temp.gameObject.AddComponent<BoxCollider2D>();
                collider4.size = new Vector2(image4.sprite.rect.width, image4.sprite.rect.height);
                temp.Id = Guid.NewGuid().ToString();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/" + temp.Id + ".prefab", temp.gameObject);
                DestroyImmediate(temp.gameObject);
                CreateObj = new GameObject();
                door = CreateObj.AddComponent<Door>();
                break;
            case 5:
                SpriteRenderer image5 = house.gameObject.AddComponent<SpriteRenderer>();
                image5.sortingLayerName = Constant.LAYER_ITEM;
                image5.sprite = house.Icon;
                BoxCollider2D collider5 = house.gameObject.AddComponent<BoxCollider2D>();
                collider5.size = new Vector2(image5.sprite.rect.width, image5.sprite.rect.height);
                house.Image = house.Icon;
                house.gameObject.tag = tag;
                house.Id = Guid.NewGuid().ToString();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/" + house.Id + ".prefab", house.gameObject);
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
                tile.Id = Guid.NewGuid().ToString();
                PrefabUtility.CreatePrefab("Assets/Resources/" + type + "/" + tile.Id + ".prefab", tile.gameObject);
                DestroyImmediate(tile.gameObject);
                CreateObj = new GameObject();
                tile = CreateObj.AddComponent<Tile>();
                break;
        }
        ClearFields();
       
    }

     void SaveSelected()
    {
        var obj = Selected.GetComponent<MapObject>();
        obj.Name = name;
        obj.Icon = texture;
        obj.gameObject.layer = (int)MapObjectType;

        SpriteRenderer Image = Selected.gameObject.GetComponent<SpriteRenderer>();
        Image.sprite = obj.Image = obj.Icon;

        switch (tab)
        {
            case 1: 

                break;
            case 2:
                var temp6 = obj as Pickup;
                temp6.ItemArmor =  pickup.ItemArmor ;
                temp6.ItemUsable =  pickup.ItemUsable;
                temp6.ItemWeapon = pickup.ItemWeapon;
                temp6.Sound = pickup.Sound;
                break;
            case 3:
                var temp1 = obj as Obstacle;
                temp1.hp = obstacle.hp ;
                temp1.Sound = obstacle.Sound;
                break;
            case 4:
                Door aux = GameObject.Find("New Game Object").GetComponent<Door>();
                var temp2 = obj as Door;
                temp2.InMap = door.InMap;
                DestroyImmediate(aux.gameObject);
                CreateObj = new GameObject();
                door = CreateObj.AddComponent<Door>();

                break;
            case 5:
                var temp3 = obj as House;
                temp3.Width = house.Width;
                temp3.Heigth = house.Heigth;
                break;
            default:

                break;
        }
        ClearFields();
    }
     void AddObject()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated") 
        {
            if (EditorGUIUtility.GetObjectPickerControlID() == 1)
            {
                texture = (Sprite)EditorGUIUtility.GetObjectPickerObject();
                if (texture != null)
                {
                    textureName = texture.name;
                }
                Repaint(); 
            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 2)
            {
                pickup.Sound = (AudioClip)EditorGUIUtility.GetObjectPickerObject();
            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 3)
            {
                obstacle.Sound = (AudioClip)EditorGUIUtility.GetObjectPickerObject();
            }
        }
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
        pickupName = string.Empty;
        itemType = Constant.ItemType.Armor;
        if ( tab == 3)
            obstacle.hp = 0;
        if (tab == 4)
        {
            door.InMap = new AbstractMap() ;
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
            GUI.DrawTextureWithTexCoords(position, sprite.texture,Constant.GetTextureCoordinate(sprite));
            if (comp.Name.Length > 8)
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
               if (temp.ItemArmor.ItemName != "")
               {
                   pickupName = temp.ItemArmor.ItemName;
                   itemType = Constant.ItemType.Armor;

               }
               if (temp.ItemUsable.ItemName != "")
               {
                   pickupName = temp.ItemUsable.ItemName;
                   itemType = Constant.ItemType.Usable;
               }
               if (temp.ItemWeapon.ItemName != "")
               {
                   pickupName = temp.ItemWeapon.ItemName;
                   itemType = Constant.ItemType.Weapon;
               }
               pickup.ItemArmor = temp.ItemArmor;
               pickup.ItemUsable = temp.ItemUsable;
               pickup.ItemWeapon = temp.ItemWeapon;
               pickup.Sound = temp.Sound;
                break;
            case 3:
                var temp1 = obj as Obstacle;
                obstacle.hp = temp1.hp;
                obstacle.Image = temp1.Image;
                obstacle.Sound = temp1.Sound;
                break;
            case 4:
                var temp2 = obj as Door;
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

