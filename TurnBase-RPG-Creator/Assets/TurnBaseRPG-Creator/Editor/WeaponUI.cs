using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class WeaponUI : CRUD<Weapon>
{
    public WeaponUI() : base("Weapon", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref Weapon w)
    {
        AssignedElement = w;
        Init();
    }

    override public void Init()
    {
        base.Init();

        foreach (var item in GetObjects())
        {
            listElements.AddItem(item.Name, item.Id);
        }
    }

    void OnGUI()
    {
        RenderLeftSide();

        GUILayout.BeginArea(new Rect(300, 0, 600, 400), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        GUI.enabled = !Selected;
        element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.Description = EditorGUILayout.TextField("Description", element.Description);
        element.Price = EditorGUILayout.IntField("Price: ", element.Price);        
        element.HitType = EditorGUILayout.Popup("Weapon Type:", element.HitType, Weapon.WeaponTypes());
        element.HitRate = EditorGUILayout.Slider("Hit Rate(%)", element.HitRate, 0, 100);

        //Attributes stats
        GUILayout.Label("Attributes", EditorStyles.boldLabel);

        element.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.Stats.Agility);
        element.Stats.Defense = EditorGUILayout.IntField("Defense: ", element.Stats.Defense);
        element.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.Stats.Luck);
        element.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.Stats.Magic);
        element.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.Stats.MagicDefense);
        element.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.Stats.MaxHP);
        element.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.Stats.MaxMP);

        // Text field to upload image
        GUI.enabled = false;
        GUI.TextField(new Rect(0, 280, 300, 20), spritename);
        GUI.enabled = true && !Selected;

        // Button to upload image
        if (GUI.Button(new Rect(300, 280, 100, 20), "Select Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        GUI.enabled = true;

        if (element.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 280, element.Image.textureRect.width, element.Image.textureRect.height), element.Image.texture, element.GetTextureCoordinate());
        }

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(0, 380, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(0, 380, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(100, 380, 100, 20), "Delete");
            GUI.enabled = true;
        }        

        GUILayout.EndArea();
    } 

    override protected void AssignElement()
    {
        AssignedElement.Name = element.Name;
        AssignedElement.Description = element.Description;
        AssignedElement.HitType = element.HitType;
        AssignedElement.HitRate = element.HitRate;
        AssignedElement.NumberHit = element.NumberHit;
        AssignedElement.Stats = element.Stats;
        AssignedElement.Id = element.Id;
        AssignedElement.Image = element.Image;        
    }

    private void AddObject()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated") //&& Event.current.type == EventType.ExecuteCommand)
        {
            element.Image = (Sprite)EditorGUIUtility.GetObjectPickerObject();

            if (element.Image != null)
            {
                spritename = element.Image.name;
            }
        }
    }
}
