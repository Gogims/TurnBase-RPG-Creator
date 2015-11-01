using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ArmorUI : CRUD<Armor>
{
    public ArmorUI(): base("Armor"){    }

    override public void Init()
    {
        element = new Armor();
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

        element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.Description = EditorGUILayout.TextField("Description", element.Description);

        element.ArmorType = EditorGUILayout.Popup(element.ArmorType, Armor.ArmorTypes());               

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
        GUI.TextField(new Rect(0, 240, 300, 20), spritename);
        GUI.enabled = true;

        // Button to upload image
        if (GUI.Button(new Rect(300, 240, 100, 20), "Select Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 240, element.Image.textureRect.width, element.Image.textureRect.height), element.Image.texture, element.GetTextureCoordinate()); 
        }

        SaveButton = GUI.Button(new Rect(0, 380, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 380, 100, 20), "Delete");
        GUI.enabled = true;
               

        GUILayout.EndArea();
    }

    public override void GetNewObject(ref Armor e)
    {
        e = new Armor();
    }

    public override void AssignElement(ref Armor wcomponent)
    {
        wcomponent.Name = element.Name;
        wcomponent.Description = element.Description;
        wcomponent.Stats = element.Stats;
        wcomponent.Id = element.Id;
        wcomponent.Image = element.Image;
    }

    private void AddObject()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated" && Event.current.type == EventType.ExecuteCommand)
        {
            element.Image = (Sprite)EditorGUIUtility.GetObjectPickerObject();

            if (element.Image != null)
            {
                spritename = element.Image.name;
            }
        }
    }    
}
