using UnityEngine;
using System.Collections;
using UnityEditor;

public class WeaponUI : EditorWindow
{
    bool save;
    Weapon weapon;

    public void Init()
    {
        weapon = new Weapon();
        save = false;
    }

    void OnGUI()
    {
        //Left side area
        GUILayout.BeginArea(new Rect(0, 0, 300, 280), "Weapons", EditorStyles.helpBox);
        GUILayout.Space(10);

        ListBox listweapons = new ListBox(new Rect(0, 0, 300, 280), new Rect(0, 0, 300, 280), false, true);
        listweapons.AddItem("Test1");
        listweapons.AddItem("Test2");
        listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2"); listweapons.AddItem("Test2");
        listweapons.AddItem("Test3");
        listweapons.ReDraw();

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

        save = GUILayout.Button("Save", GUILayout.Width(100));

        GUILayout.EndArea();
    }

    void Update()
    {
        if (save)
        {
            GameObject weaponObject = new GameObject("Weapon");
            Weapon wcomponent = weaponObject.AddComponent<Weapon>();            

            wcomponent.Name = weapon.Name;
            wcomponent.Description = weapon.Description;
            wcomponent.HitType = weapon.HitType;
            wcomponent.HitRate = weapon.HitRate;
            wcomponent.NumberHit = weapon.NumberHit;
            wcomponent.Stats = weapon.Stats;


            PrefabUtility.CreatePrefab("Assets/Prefabs/Weapon/test.prefab", wcomponent.gameObject);
            DestroyImmediate(weaponObject);
        }
    }
    
}
