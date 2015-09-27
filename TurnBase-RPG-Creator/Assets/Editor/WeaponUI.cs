using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class WeaponUI : CRUD
{    
    Weapon weapon = new Weapon();
    ListBox listweapons = new ListBox(new Rect(0, 0, 300, 260), new Rect(0, 0, 285, 280), false, true);
    
    GameObject weaponObject;
    int prevId = -1;

    public WeaponUI() : base(@"\Assets\Resources\Weapon\") { }

    public void Init()
    {
        weapon = new Weapon();        
        ListObjects = (Resources.LoadAll("Weapon", typeof(GameObject)));

        for (int i = 0; i < ListObjects.Length; i++)
        {
            weaponObject = (GameObject)Instantiate(ListObjects[i]);
            Weapon temp = weaponObject.GetComponent<Weapon>();
            listweapons.AddItem(temp.Name, temp.Id);
            DestroyImmediate(weaponObject);
        }

        GetId();
    }

    void OnGUI()
    {
        //Left side area
        GUILayout.BeginArea(new Rect(0, 0, 300, 280), "Weapons", EditorStyles.helpBox);
        GUILayout.Space(10);        
        
        listweapons.ReDraw();

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

        SaveButton = GUILayout.Button("Save", GUILayout.Width(100));

        GUILayout.EndArea();
    }

    void Update()
    {
        //Si se elige otra arma para ver
        if (listweapons.GetSelectedID() != prevId)
        {
            if (weaponObject != null)
            {
                DestroyImmediate(weaponObject);
            }

            weaponObject = (GameObject)Instantiate(ListObjects[listweapons.GetSelectedIndex()]);
            weapon = weaponObject.GetComponent<Weapon>();

            prevId = listweapons.GetSelectedID();
            Creating = false;
        }

        //Funcionamineto de guardado
        if (SaveButton)
        {
            SaveWeapon();
        }

        //Si se crea una nueva arma
        if (CreateButton)
        {
            weapon = new Weapon();
            Creating = true;
            prevId = 0;
        }
    }

    public void SaveWeapon()
    {
        if (Creating)
        {
            weaponObject = new GameObject("Weapon");
        }

        Weapon wcomponent = weaponObject.AddComponent<Weapon>();

        if (Creating)
        {
            Id++;
            wcomponent.Id = Id;
            listweapons.AddItem(wcomponent.Name, wcomponent.Id);
            Creating = false;
        }

        wcomponent.Name = weapon.Name;
        wcomponent.Description = weapon.Description;
        wcomponent.HitType = weapon.HitType;
        wcomponent.HitRate = weapon.HitRate;
        wcomponent.NumberHit = weapon.NumberHit;
        wcomponent.Stats = weapon.Stats;

        PrefabUtility.CreatePrefab("Assets/Resources/Weapon/" + wcomponent.Id + ".prefab", wcomponent.gameObject);
        DestroyImmediate(weaponObject);
        SetId();
    }
}
