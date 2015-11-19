using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Rotorz.ReorderableList;

public class EnemyUI : CRUD<Enemy>
{
    Weapon MainHand;
    Armor Helmet;
    Armor UpperBody;
    Armor LowerBody;
    Armor Feet;
    Armor Ring;
    Armor Necklace;
    Job PlayerJob;
    Animator animations;
    Vector2 ScrollPosition;

    public EnemyUI() : base("Enemy", new Rect(0, 0, 300, 930)) { }

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
        element.Description = EditorGUILayout.TextField("Description", element.Description);
        element.Level = EditorGUILayout.IntSlider(new GUIContent("Level:"), element.Level, 1, 100);
        element.RewardExperience = EditorGUILayout.IntField("Experience Earned: ", element.RewardExperience);
        element.RewardCurrency = EditorGUILayout.IntField("Currency Earned: ", element.RewardCurrency);

        GUILayout.Label("Class:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField("");
        if (GUILayout.Button("Select Class"))
        {
            var window = EditorWindow.GetWindow<JobUI>();
            window.Selected = true;
            window.Initialize(ref PlayerJob);
            window.Show();
        }
        GUILayout.EndHorizontal();

        // Attributes section
        GUILayout.Label("Attributes", EditorStyles.boldLabel);
        element.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.Stats.MaxHP);
        element.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.Stats.MaxMP);
        element.Stats.Attack = EditorGUILayout.IntField("Attack: ", element.Stats.Attack);
        element.Stats.Defense = EditorGUILayout.IntField("Defense: ", element.Stats.Defense);
        element.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.Stats.Agility);
        element.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.Stats.Luck);
        element.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.Stats.Magic);
        element.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.Stats.MagicDefense);
        GUILayout.EndArea();

        // Initial Equipment Area
        GUILayout.BeginArea(new Rect(300, 330, 600, 320), "Initial Equipment", EditorStyles.helpBox);
        GUILayout.Space(10);

        GUILayout.Label("Weapon", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(MainHand.Name);
        if (GUILayout.Button("Select Weapon"))
        {
            var window = EditorWindow.GetWindow<WeaponUI>();
            window.Selected = true;
            window.Initialize(ref MainHand);
            window.Show();
        }
        GUILayout.EndHorizontal();

        AddArmor("Helmet", ref Helmet, Armor.ArmorType.Helmet);
        AddArmor("Upper Body", ref UpperBody, Armor.ArmorType.Chest);
        AddArmor("Lower Body", ref LowerBody, Armor.ArmorType.Leg);
        AddArmor("Feet", ref Feet, Armor.ArmorType.Feet);
        AddArmor("Necklace", ref Necklace, Armor.ArmorType.Necklace);
        AddArmor("Ring", ref Ring, Armor.ArmorType.Ring);

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

        if (element.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 40, element.Image.textureRect.width, element.Image.textureRect.height), element.Image.texture, Constant.GetTextureCoordinate(element.Image));
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
        ReorderableListGUI.ListField(element.Items, DrawItem, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        SaveButton = GUI.Button(new Rect(300, 910, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(400, 910, 100, 20), "Delete");
        GUI.enabled = true;
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

    private void AddArmor(string name, ref Armor current, Armor.ArmorType armortype)
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(current.Name);
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
            animation.downIdle = animation.ConstructAnimation(element.downSprites[0],  element.Id, "downIdle", 30, true);
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

    protected override void Create()
    {
        element.Id = element.GetInstanceID();

        CreateAnimation();
        CreatePrefab(element);   
    }

    protected override void Edit()
    {
        CreateAnimation();
        base.Edit();
    }

    protected override void UpdateForm()
    {
        MainHand = elementObject.GetComponent<Weapon>();
        PlayerJob = elementObject.GetComponent<Job>();
        animations = elementObject.GetComponent<Animator>();
        Armor[] armors = elementObject.GetComponents<Armor>();

        for (int i = 0; i < armors.Length; i++)
        {

            if (i == 0)
                Helmet = armors[i];
            if (i == 1)
                UpperBody = armors[i];
            if (i == 2)
                LowerBody = armors[i];
            if (i == 3)
                Feet = armors[i];
            if (i == 4)
                Necklace = armors[i];
            if (i == 5)
                Ring = armors[i];
        }
    }

    protected override GameObject NewGameObject()
    {
        elementObject = base.NewGameObject();
        NewEnemy();

        return elementObject;
    }

    private void NewEnemy()
    {
        MainHand = elementObject.AddComponent<Weapon>();

        Helmet = elementObject.AddComponent<Armor>();
        UpperBody = elementObject.AddComponent<Armor>();
        LowerBody = elementObject.AddComponent<Armor>();
        Ring = elementObject.AddComponent<Armor>();
        Necklace = elementObject.AddComponent<Armor>();
        Feet = elementObject.AddComponent<Armor>();

        PlayerJob = elementObject.AddComponent<Job>();        

        animations = elementObject.AddComponent<Animator>();

        element.downSprites = new List<Sprite>();
        element.leftSprites = new List<Sprite>();
        element.upSprites = new List<Sprite>();
        element.rightSprites = new List<Sprite>();        
    }
}