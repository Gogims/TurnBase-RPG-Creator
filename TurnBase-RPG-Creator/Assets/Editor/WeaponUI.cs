using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class WeaponUI : CRUD
{    
    Weapon weapon = new Weapon();
    ListBox listweapons = new ListBox(new Rect(0, 0, 300, 260), new Rect(0, 0, 285, 280), false, true);    
    
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
        GUILayout.BeginArea(new Rect(0, 0, 300, 280), "Weapons", EditorStyles.helpBox);
        GUILayout.Space(10);

        if (listweapons.ReDraw())
        {
            UpdateListBox();
        }

        CreateButton = GUI.Button(new Rect(0, 260, 100, 20), "Create");

        GUILayout.EndArea();       

        //Right side area        
        GUILayout.BeginArea(new Rect(300, 0, 400, 280), "Basic Settings", EditorStyles.helpBox);

        GUILayout.Space(10);

        weapon.Name = EditorGUILayout.TextField("Name", weapon.Name);
        weapon.Description = EditorGUILayout.TextField("Description", weapon.Description);

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

        SaveButton = GUI.Button(new Rect(0, 260, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 260, 100, 20), "Delete");
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
        Weapon wcomponent = elementObject.AddComponent<Weapon>();
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
    }

    private void CreatePrefab(ref GameObject weaponObject, Weapon wcomponent)
    {
        PrefabUtility.CreatePrefab("Assets/Resources/Weapon/" + wcomponent.Id + ".prefab", wcomponent.gameObject);
        DestroyImmediate(weaponObject);        
    }
}
