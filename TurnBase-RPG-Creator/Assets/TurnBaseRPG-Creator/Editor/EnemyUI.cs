using UnityEngine;
using UnityEditor;
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

        element.BattleEnemy.Data.ActorName = element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.BattleEnemy.Data.Description = EditorGUILayout.TextField("Description", element.BattleEnemy.Data.Description);
        element.BattleEnemy.Data.Level = EditorGUILayout.IntSlider(new GUIContent("Level:"), element.BattleEnemy.Data.Level, 1, 100);
        element.BattleEnemy.Data.RewardExperience = EditorGUILayout.IntField("Experience Earned: ", element.BattleEnemy.Data.RewardExperience);
        element.BattleEnemy.Data.RewardCurrency = EditorGUILayout.IntField("Currency Earned: ", element.BattleEnemy.Data.RewardCurrency);

        GUILayout.Label("Class:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(element.BattleEnemy.Data.Job.JobName);
        if (GUILayout.Button("Select Class"))
        {
            var window = EditorWindow.GetWindow<JobUI>();
            window.Selected = true;
            window.Initialize(ref element.BattleEnemy.Data.Job);
            window.Show();
        }
        GUILayout.EndHorizontal();

        // Attributes section
        GUILayout.Label("Attributes", EditorStyles.boldLabel);
        element.BattleEnemy.Data.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.BattleEnemy.Data.Stats.MaxHP);
        element.BattleEnemy.Data.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.BattleEnemy.Data.Stats.MaxMP);
        element.BattleEnemy.Data.Stats.Attack = EditorGUILayout.IntField("Attack: ", element.BattleEnemy.Data.Stats.Attack);
        element.BattleEnemy.Data.Stats.Defense = EditorGUILayout.IntField("Defense: ", element.BattleEnemy.Data.Stats.Defense);
        element.BattleEnemy.Data.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.BattleEnemy.Data.Stats.Agility);
        element.BattleEnemy.Data.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.BattleEnemy.Data.Stats.Luck);
        element.BattleEnemy.Data.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.BattleEnemy.Data.Stats.Magic);
        element.BattleEnemy.Data.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.BattleEnemy.Data.Stats.MagicDefense);
        GUILayout.EndArea();

        // Initial Equipment Area
        GUILayout.BeginArea(new Rect(300, 330, 600, 320), "Initial Equipment", EditorStyles.helpBox);
        GUILayout.Space(10);

        GUILayout.Label("Weapon", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(element.BattleEnemy.Data.MainHand.ItemName);
        if (GUILayout.Button("Select Weapon"))
        {
            var window = EditorWindow.GetWindow<WeaponUI>();
            window.Selected = true;
            window.Initialize(ref element.BattleEnemy.Data.MainHand);
            window.Show();
        }
        GUILayout.EndHorizontal();

        AddArmor("Helmet", ref element.BattleEnemy.Data.Helmet, AbstractArmor.ArmorType.Helmet);
        AddArmor("Body", ref element.BattleEnemy.Data.Body, AbstractArmor.ArmorType.Body);
        AddArmor("Feet", ref element.BattleEnemy.Data.Feet, AbstractArmor.ArmorType.Feet);
        AddArmor("Necklace", ref element.BattleEnemy.Data.Necklace, AbstractArmor.ArmorType.Necklace);
        AddArmor("Ring", ref element.BattleEnemy.Data.Ring, AbstractArmor.ArmorType.Ring);

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
        ReorderableListGUI.ListField(element.BattleEnemy.Data.Items, DrawItem, ReorderableListFlags.DisableReordering);
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
        element.BattleEnemy.Data.Image = element.Icon;
        base.Create();
    }

    protected override void Edit()
    {
        element.BattleEnemy.Data.Image = element.Icon;
        base.Edit();
    }

    protected override void AssignElement()
    {
        EnemySelected.ActorName = element.BattleEnemy.Data.ActorName;
        EnemySelected.Description = element.BattleEnemy.Data.Description;
        EnemySelected.HP = element.BattleEnemy.Data.HP;
        EnemySelected.Image = element.BattleEnemy.Data.Image;
        EnemySelected.Items = element.BattleEnemy.Data.Items;
        EnemySelected.Level = element.BattleEnemy.Data.Level;
        EnemySelected.MP = element.BattleEnemy.Data.MP;
        EnemySelected.RewardCurrency = element.BattleEnemy.Data.RewardCurrency;
        EnemySelected.RewardExperience = element.BattleEnemy.Data.RewardExperience;
        EnemySelected.Stats = element.BattleEnemy.Data.Stats;
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