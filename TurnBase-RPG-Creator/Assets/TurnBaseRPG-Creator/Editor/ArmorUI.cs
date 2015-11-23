using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ArmorUI : CRUD<Armor>
{
    AbstractArmor SelectedArmor;
    AbstractArmor.ArmorType ArmorType;
    bool filter;

    public ArmorUI(): base("Armor", new Rect(0, 0, 300, 400)) {    }

    public void Initialize(ref AbstractArmor armor, AbstractArmor.ArmorType type)
    {
        SelectedArmor = armor;
        ArmorType = type;
        filter = true;
        base.Init();
    }

    void OnGUI()
    {
        RenderLeftSide();
        
        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 180), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = !Selected;
        element.Data.ArmorName = element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description: ", element.Data.Description);
        element.Data.Type = (AbstractArmor.ArmorType) EditorGUILayout.EnumPopup("Armor Type: ", element.Data.Type);
        element.Data.Price = EditorGUILayout.IntField("Price: ", element.Data.Price);

        if (GUI.Button(new Rect(0, 100, 400, 20), "Select Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Armor", 1);
        }

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 100, Constant.SpriteWidth, Constant.SpriteHeight), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        AddObject();

        GUILayout.EndArea();

        //Attributes stats
        GUILayout.BeginArea(new Rect(300, 180, 600, 160), "Attributes", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.Data.Stats.Agility);
        element.Data.Stats.Defense = EditorGUILayout.IntField("Defense: ", element.Data.Stats.Defense);
        element.Data.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.Data.Stats.Luck);
        element.Data.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.Data.Stats.Magic);
        element.Data.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.Data.Stats.MagicDefense);
        element.Data.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.Data.Stats.MaxHP);
        element.Data.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.Data.Stats.MaxMP);

        GUILayout.EndArea();

        // State
        GUILayout.BeginArea(new Rect(300, 340, 600, 40), "State", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.Data.PercentageState = EditorGUILayout.Slider("Apply State(%)", element.Data.PercentageState, 0, 100);
        GUILayout.TextField(element.Data.State.State);
        if (GUILayout.Button("Select State"))
        {
            var window = EditorWindow.GetWindow<StateUI>();
            window.Selected = true;
            window.Initialize(ref element.Data.State);
            window.Show();
        }        
        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, 380, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            GUI.enabled = true;
            SaveButton = GUI.Button(new Rect(300, 380, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 380, 100, 20), "Delete");
            GUI.enabled = true;
        }
    }

    protected override void Create()
    {
        element.Data.Image = element.Icon;
        base.Create();
    }

    override protected void AssignElement()
    {
        SelectedArmor.ArmorName = element.Name;
        SelectedArmor.Description = element.Data.Description;
        SelectedArmor.Image = element.Data.Image;
        SelectedArmor.MinLevel = element.Data.MinLevel;
        SelectedArmor.PercentageState = element.Data.PercentageState;
        SelectedArmor.Price = element.Data.Price;
        SelectedArmor.State = element.Data.State;
        SelectedArmor.Stats = element.Data.Stats;
        SelectedArmor.Type = element.Data.Type;
    }

    protected override bool FilterList(Armor component)
    {
        if (!filter) return true;

        return ArmorType == component.Data.Type;
    }
}
