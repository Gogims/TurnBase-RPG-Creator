using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Rotorz.ReorderableList;

public class EnemyUI : CRUD<Enemy>
{    
    Vector2 ScrollPosition;
    AbstractEnemy EnemySelected;

    public EnemyUI() : base("Enemy", new Rect(0, 0, 300, 930)) { }

    public void Initialize(ref AbstractEnemy enemy)
    {
        EnemySelected = enemy;
        Init();
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings Area
        GUILayout.BeginArea(new Rect(300, 0, 600, 330), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        element.Data.ActorName = element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description", element.Data.Description);
        element.Data.Level = EditorGUILayout.IntSlider(new GUIContent("Level:"), element.Data.Level, 1, 100);
        element.Data.RewardExperience = EditorGUILayout.IntField("Experience Earned: ", element.Data.RewardExperience);
        element.Data.RewardCurrency = EditorGUILayout.IntField("Currency Earned: ", element.Data.RewardCurrency);

        GUILayout.Label("Class:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(element.Data.Job.JobName);
        if (GUILayout.Button("Select Class"))
        {
            var window = EditorWindow.GetWindow<JobUI>();
            window.Selected = true;
            window.Initialize(ref element.Data.Job);
            window.Show();
        }
        GUILayout.EndHorizontal();

        // Attributes section
        GUILayout.Label("Attributes", EditorStyles.boldLabel);
        element.Data.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.Data.Stats.MaxHP);
        element.Data.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.Data.Stats.MaxMP);
        element.Data.Stats.Attack = EditorGUILayout.IntField("Attack: ", element.Data.Stats.Attack);
        element.Data.Stats.Defense = EditorGUILayout.IntField("Defense: ", element.Data.Stats.Defense);
        element.Data.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.Data.Stats.Agility);
        element.Data.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.Data.Stats.Luck);
        element.Data.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.Data.Stats.Magic);
        element.Data.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.Data.Stats.MagicDefense);
        GUILayout.EndArea();

        // Initial Equipment Area
        GUILayout.BeginArea(new Rect(300, 330, 600, 320), "Initial Equipment", EditorStyles.helpBox);
        GUILayout.Space(10);

        GUILayout.Label("Weapon", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(element.Data.MainHand.ItemName);
        if (GUILayout.Button("Select Weapon"))
        {
            var window = EditorWindow.GetWindow<WeaponUI>();
            window.Selected = true;
            window.Initialize(ref element.Data.MainHand);
            window.Show();
        }
        GUILayout.EndHorizontal();

        AddArmor("Helmet", ref element.Data.Helmet, AbstractArmor.ArmorType.Helmet);
        AddArmor("Body", ref element.Data.Body, AbstractArmor.ArmorType.Body);
        AddArmor("Feet", ref element.Data.Feet, AbstractArmor.ArmorType.Feet);
        AddArmor("Necklace", ref element.Data.Necklace, AbstractArmor.ArmorType.Necklace);
        AddArmor("Ring", ref element.Data.Ring, AbstractArmor.ArmorType.Ring);

        GUILayout.EndArea();

        // Design
        GUILayout.BeginArea(new Rect(300, 650, 600, 160), "Design", EditorStyles.helpBox);
        GUILayout.Space(15);

        // Text field to upload image
        GUILayout.Label("Avatar", EditorStyles.boldLabel);

        // Button to upload image
        if (GUI.Button(new Rect(0, 40, 400, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 40, element.Icon.textureRect.width, element.Icon.textureRect.height), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }        

        
        GUILayout.EndArea();

        // Items
        GUILayout.BeginArea(new Rect(300, 810, 600, 100), "Items", EditorStyles.helpBox);
        GUILayout.Space(15);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.Data.Items, DrawItem, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, 910, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            GUI.enabled = true;
            SaveButton = GUI.Button(new Rect(300, 910, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 910, 100, 20), "Delete");
            GUI.enabled = true;
        }
    }

    protected override void Create()
    {
        element.Data.Image = element.Icon;
        base.Create();
    }

    protected override void Edit()
    {
        element.Data.Image = element.Icon;
        base.Edit();
    }

    protected override void AssignElement()
    {
        EnemySelected.ActorName = element.Data.ActorName;
        EnemySelected.Description = element.Data.Description;
        EnemySelected.HP = element.Data.HP;
        EnemySelected.Image = element.Data.Image;
        EnemySelected.Items = element.Data.Items;
        EnemySelected.Level = element.Data.Level;
        EnemySelected.MP = element.Data.MP;
        EnemySelected.RewardCurrency = element.Data.RewardCurrency;
        EnemySelected.RewardExperience = element.Data.RewardExperience;
        EnemySelected.Stats = element.Data.Stats;
        EnemySelected.Image = element.Icon;    
    }

    private Rate DrawItem(Rect position, Rate item)
    {
        if (item == null)
        {
            item = new Rate();
        }

        GUI.enabled = false;
        GUI.TextField(new Rect(position.x, position.y, position.width - 400, position.height), item.Element.ItemName);
        GUI.enabled = true;

        if (GUI.Button(new Rect(position.width - 400, position.y, 100, position.height), "Select Item"))
        {
            var window = EditorWindow.GetWindow<ItemUI>();
            window.Selected = true;
            window.Initialize(ref item.Element);
            window.Show();
        }

        item.ApplyRate = EditorGUI.Slider(new Rect(position.width - 300, position.y, 300, position.height), "Drop Rate(%): ", item.ApplyRate, 0, 100);

        return item;
    }    

    private void AddArmor(string name, ref AbstractArmor current, AbstractArmor.ArmorType armortype)
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(current.ItemName);
        if (GUILayout.Button("Select Armor"))
        {
            var window = EditorWindow.GetWindow<ArmorUI>();
            window.Selected = true;
            window.Initialize(ref current, armortype);
            window.Show();
        }
        GUILayout.EndHorizontal();
    }
}