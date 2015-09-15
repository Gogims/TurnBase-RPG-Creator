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
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

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
