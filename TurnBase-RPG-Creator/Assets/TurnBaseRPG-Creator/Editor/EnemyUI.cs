using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Rotorz.ReorderableList;

public class EnemyUI : CRUD<Enemy>
{    
    Animator animations;
    Vector2 ScrollPosition;
    AbstractEnemy EnemySelected;

    public EnemyUI() : base("Enemy", new Rect(0, 0, 300, 930)) { }

    public void Initialize(ref AbstractEnemy enemy)
    {
        EnemySelected = enemy;
        Init();
    }

    public override void Init()
    {
        base.Init();
        NewEnemy();
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings Area
        GUILayout.BeginArea(new Rect(300, 0, 600, 330), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description", element.Data.Description);
        element.Data.Level = EditorGUILayout.IntSlider(new GUIContent("Level:"), element.Data.Level, 1, 100);
        element.Data.RewardExperience = EditorGUILayout.IntField("Experience Earned: ", element.Data.RewardExperience);
        element.Data.RewardCurrency = EditorGUILayout.IntField("Currency Earned: ", element.Data.RewardCurrency);

        GUILayout.Label("Class:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField("");
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
        GUILayout.TextField(element.Data.MainHand.WeaponName);
        if (GUILayout.Button("Select Weapon"))
        {
            var window = EditorWindow.GetWindow<WeaponUI>();
            window.Selected = true;
            window.Initialize(ref element.Data.MainHand);
            window.Show();
        }
        GUILayout.EndHorizontal();

        AddArmor("Helmet", ref element.Data.Helmet, AbstractArmor.ArmorType.Helmet);
        AddArmor("Upper Body", ref element.Data.UpperBody, AbstractArmor.ArmorType.Chest);
        AddArmor("Lower Body", ref element.Data.LowerBody, AbstractArmor.ArmorType.Leg);
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

        GUI.Label(new Rect(0, 60, 100, 20), "Animation", EditorStyles.boldLabel);
        AddAnimation("Down", ref element.downSprites, new Rect(0, 80, 75, 20));
        AddAnimation("Left", ref element.leftSprites, new Rect(100, 80, 75, 20));
        AddAnimation("Up", ref element.upSprites, new Rect(200, 80, 75, 20));
        AddAnimation("Right", ref element.rightSprites, new Rect(300, 80, 75, 20));
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
        element.Id = element.GetInstanceID();
        element.Data.Image = element.Icon;

        CreateAnimation();
        CreatePrefab(element);   
    }

    protected override void Edit()
    {
        CreateAnimation();
        base.Edit();
    }

    protected override GameObject NewGameObject()
    {
        elementObject = base.NewGameObject();
        NewEnemy();

        return elementObject;
    }

    protected override void AssignElement()
    {
        EnemySelected.Description = element.Data.Description;
        EnemySelected.HP = element.Data.HP;
        EnemySelected.Image = element.Data.Image;
        EnemySelected.Items = element.Data.Items;
        EnemySelected.Level = element.Data.Level;
        EnemySelected.MP = element.Data.MP;
        EnemySelected.RewardCurrency = element.Data.RewardCurrency;
        EnemySelected.RewardExperience = element.Data.RewardExperience;
        EnemySelected.Stats = element.Data.Stats;        
    }    

    private void NewEnemy()
    {
        animations = elementObject.AddComponent<Animator>();

        element.downSprites = new List<Sprite>();
        element.leftSprites = new List<Sprite>();
        element.upSprites = new List<Sprite>();
        element.rightSprites = new List<Sprite>();        
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

    private void AddAnimation(string name, ref List<Sprite> animation, Rect position)
    {
        if (GUI.Button(position, name + " (" + animation.Count.ToString() + ")"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref animation, name + " Sprites");
            window.Show();
        }
    }

    private void AddArmor(string name, ref AbstractArmor current, AbstractArmor.ArmorType armortype)
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(current.ArmorName);
        if (GUILayout.Button("Select Armor"))
        {
            var window = EditorWindow.GetWindow<ArmorUI>();
            window.Selected = true;
            window.Initialize(ref current, armortype);
            window.Show();
        }
        GUILayout.EndHorizontal();
    }

    private void CreateAnimation()
    {
        ActorAnimation animation = new ActorAnimation("Enemy");
        List<Sprite> sprites;

        if (element.downSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.downSprites);
            sprites.Add(element.downSprites[0]);
            animation.down = animation.ConstructAnimation(sprites, element.Id, "down", 30, true);
            animation.downIdle = animation.ConstructAnimation(element.downSprites[0], element.Id, "downIdle", 30, true);
        }

        if (element.leftSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.leftSprites);
            sprites.Add(element.leftSprites[0]);
            animation.left = animation.ConstructAnimation(sprites, element.Id, "left", 30, true);
            animation.leftIdle = animation.ConstructAnimation(element.leftSprites[0], element.Id, "leftIdle", 30, true);
        }

        if (element.upSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.upSprites);
            sprites.Add(element.upSprites[0]);
            animation.up = animation.ConstructAnimation(sprites, element.Id, "up", 30, true);
            animation.upIdle = animation.ConstructAnimation(element.upSprites[0], element.Id, "upIdle", 30, true);
        }

        if (element.rightSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.rightSprites);
            sprites.Add(element.rightSprites[0]);
            animation.right = animation.ConstructAnimation(sprites, element.Id, "right", 30, true);
            animation.rightIdle = animation.ConstructAnimation(element.rightSprites[0], element.Id, "rightIdle", 30, true);
        }

        animations.runtimeAnimatorController = animation.ConstructAnimationControl(element.Id);
    }
}