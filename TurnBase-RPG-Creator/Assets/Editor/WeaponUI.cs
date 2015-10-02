using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class WeaponUI : CRUD
{    
    Weapon weapon = new Weapon();
    ListBox listweapons = new ListBox(new Rect(0, 0, 300, 380), new Rect(0, 0, 285, 400), false, true);    
    
    int prevId = -1;

    public WeaponUI() : base(@"\Assets\Resources\Weapon\") { }

    public void Init()
    {
        weapon = new Weapon();        
        ListObjects = (Resources.LoadAll("Weapon", typeof(GameObject)));
        Creating = true;

        for (int i = 0; i < ListObjects.Length; i++)
        {
            elementObject = (GameObject)Instantiate(ListObjects[i]);
            Weapon temp = elementObject.GetComponent<Weapon>();
            listweapons.AddItem(temp.Name, temp.Id);
            DestroyImmediate(elementObject);
        }

        GetId();
    }

    void OnGUI()
    {
        //Left side area
        GUILayout.BeginArea(new Rect(0, 0, 300, 400), "Weapons", EditorStyles.helpBox);
        GUILayout.Space(10);

        if (listweapons.ReDraw())
        {
            UpdateListBox();
        }

        CreateButton = GUI.Button(new Rect(0, 380, 100, 20), "Create");

        GUILayout.EndArea();       

        //Right side area        
        GUILayout.BeginArea(new Rect(300, 0, 600, 400), "Basic Settings", EditorStyles.helpBox);

        GUILayout.Space(10);

        weapon.Name = EditorGUILayout.TextField("Name", weapon.Name);
        weapon.Description = EditorGUILayout.TextField("Description", weapon.Description);
        weapon.Price = EditorGUILayout.IntField("Price: ", weapon.Price);

        EditorGUILayout.PrefixLabel("Weapon Type");
        weapon.HitType = EditorGUILayout.Popup(weapon.HitType, Weapon.WeaponTypes());

        weapon.HitRate = EditorGUILayout.Slider("Hit Rate(%)", weapon.HitRate, 0, 100);
        
        //Attributes stats
        GUILayout.Label("Attributes", EditorStyles.boldLabel);

        weapon.Stats.Agility = EditorGUILayout.IntField("Agility: ", weapon.Stats.Agility);
        weapon.Stats.Defense = EditorGUILayout.IntField("Defense: ", weapon.Stats.Defense);
        weapon.Stats.Luck = EditorGUILayout.IntField("Luck: ", weapon.Stats.Luck);
        weapon.Stats.Magic = EditorGUILayout.IntField("Magic: ", weapon.Stats.Magic);
        weapon.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", weapon.Stats.MagicDefense);
        weapon.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", weapon.Stats.MaxHP);
        weapon.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", weapon.Stats.MaxMP);

        // Text field to upload image
        GUI.enabled = false;
        GUI.TextField(new Rect(0, 280, 300, 20), spritename);
        GUI.enabled = true;

        // Button to upload image
        if (GUI.Button(new Rect(300, 280, 100, 20), "Select Sprite"))
        {
            ObjectSelectorWrapper.ShowSelector(typeof(Sprite));
        }

        if (weapon.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 280, weapon.Image.textureRect.width, weapon.Image.textureRect.height), weapon.Image.texture, weapon.GetTextureCoordinate());
        }

        SaveButton = GUI.Button(new Rect(0, 380, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 380, 100, 20), "Delete");
        GUI.enabled = true;

        GUILayout.EndArea();
    }

    void UpdateListBox()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject);
        }

        elementObject = (GameObject)Instantiate(ListObjects[listweapons.GetSelectedIndex()]);
        weapon = elementObject.GetComponent<Weapon>();

        prevId = listweapons.GetSelectedID();
        Creating = false;
    }

    void Update()
    {
        //Si se crea una nueva arma
        if (CreateButton)
        {
            weapon = new Weapon();
            Creating = true;
        }

        //Funcionamineto de guardado
        if (SaveButton)
        {
            SaveWeapon();
            weapon = new Weapon();
        }        
        
        //Si se elimina un arma
        if (DeleteButton)
        {
            Delete();
            ListObjects = (Resources.LoadAll("Weapon", typeof(GameObject)));
            weapon = new Weapon();
        }

        //Seleccionar el Sprite
        if (ObjectSelectorWrapper.isVisible)
        {
            weapon.Image = ObjectSelectorWrapper.GetSelectedObject<Sprite>();

            if (weapon.Image != null)
            {
                spritename = weapon.Image.name;
            }
        }
    }

    public void SaveWeapon()
    {
        if (Creating)
        {
            Create();
        }
        else
        {
            Edit();
        }

        ListObjects = (Resources.LoadAll("Weapon", typeof(GameObject)));
    }

    public override void Create()
    {
        elementObject = new GameObject("Weapon");
        Weapon wcomponent = elementObject.AddComponent<Weapon>();

        AssignWeapon(ref wcomponent);        

        Id++;
        wcomponent.Id = Id;
        CreatePrefab(ref elementObject, wcomponent);
        listweapons.AddItem(weapon.Name, weapon.Id);
        Creating = false;
        SetId();
    }

    public override void Edit()
    {
        Weapon wcomponent = elementObject.GetComponent<Weapon>();
        AssignWeapon(ref wcomponent);
        CreatePrefab(ref elementObject, wcomponent);

        listweapons.ChangeName(listweapons.GetSelectedIndex(), weapon.Name);
    }

    public override void Delete()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject, true);
            AssetDatabase.DeleteAsset("Assets/Resources/Weapon/" + weapon.Id + ".prefab");
            listweapons.RemoveItemIndex(listweapons.GetSelectedIndex());
        }        
    }

    private void AssignWeapon(ref Weapon wcomponent)
    {
        wcomponent.Name = weapon.Name;
        wcomponent.Description = weapon.Description;
        wcomponent.HitType = weapon.HitType;
        wcomponent.HitRate = weapon.HitRate;
        wcomponent.NumberHit = weapon.NumberHit;
        wcomponent.Stats = weapon.Stats;
        wcomponent.Id = weapon.Id;
        wcomponent.Image = weapon.Image;
    }

    private void CreatePrefab(ref GameObject weaponObject, Weapon wcomponent)
    {
        PrefabUtility.CreatePrefab("Assets/Resources/Weapon/" + wcomponent.Id + ".prefab", wcomponent.gameObject);
        DestroyImmediate(weaponObject);        
    }
}
