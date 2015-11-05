using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlayerUI : CRUD<Player>
{
    public List<Sprite> downSprites;
    public List<Sprite> leftSprites;
    public List<Sprite> upSprites;
    public List<Sprite> rightSprites;

    public Weapon MainHand;
    public Armor Helmet;
    public Armor UpperBody;
    public Armor LowerBody;
    public Armor Feet;
    public Armor Ring;
    public Armor Necklace;

    public PlayerUI() : base("Player", new Rect(0, 0, 300, 730)) { }

    public override void Init()
    {
        element = new Player();
        base.Init();

        downSprites = new List<Sprite>();
        leftSprites = new List<Sprite>();
        upSprites = new List<Sprite>();
        rightSprites = new List<Sprite>();

        MainHand = elementObject.AddComponent<Weapon>();
        Helmet = elementObject.AddComponent<Armor>();
        UpperBody = elementObject.AddComponent<Armor>();
        LowerBody = elementObject.AddComponent<Armor>();
        Ring = elementObject.AddComponent<Armor>();
        Necklace = elementObject.AddComponent<Armor>();
        Feet = elementObject.AddComponent<Armor>();

        foreach (var item in GetObjects())
        {
            listElements.AddItem(item.Name, item.Id);
        }
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings Area
        GUILayout.BeginArea(new Rect(300, 0, 600, 250), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.Description = EditorGUILayout.TextField("Description", element.Description);
        element.Level = EditorGUILayout.IntSlider(new GUIContent("Level:"), element.Level, 1, 100);

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
        GUILayout.BeginArea(new Rect(300, 250, 600, 320), "Initial Equipment", EditorStyles.helpBox);
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

        AddArmor("Helmet", ref Helmet, Armor.Type.Helmet);
        AddArmor("Upper Body", ref UpperBody, Armor.Type.Chest);
        AddArmor("Lower Body", ref LowerBody, Armor.Type.Leg);
        AddArmor("Feet", ref Feet, Armor.Type.Feet);
        AddArmor("Necklace", ref Necklace, Armor.Type.Necklace);
        AddArmor("Ring", ref Ring, Armor.Type.Ring);        

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(300, 570, 600, 160), "Design", EditorStyles.helpBox);
        GUILayout.Space(10);

        // Text field to upload image
        GUILayout.Label("Avatar", EditorStyles.boldLabel);
        GUI.enabled = false;
        GUI.TextField(new Rect(0, 40, 300, 20), spritename);
        GUI.enabled = true;

        // Button to upload image
        if (GUI.Button(new Rect(300, 40, 100, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 40, element.Image.textureRect.width, element.Image.textureRect.height), element.Image.texture, element.GetTextureCoordinate());
        }        

        GUI.Label(new Rect(0, 60, 100, 20), "Animation", EditorStyles.boldLabel);        

        if (GUI.Button(new Rect(0, 80, 50, 20), "Down"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref downSprites, "Down Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(75, 80, 50, 20), "Left"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref leftSprites, "Left Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(150, 80, 50, 20), "Up"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref upSprites, "Up Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(225, 80, 50, 20), "Right"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref rightSprites, "Right Sprites");
            window.Show();
        }

        SaveButton = GUI.Button(new Rect(0, 140, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 140, 100, 20), "Delete");
        GUI.enabled = true;

        GUILayout.EndArea();

        if (CreateButton)
        {
            CreateAnimation();
        }
    }    

    private void AddArmor(string name, ref Armor current, Armor.Type armortype)
    {
        GUILayout.Label(name, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(current.Name);
        if (GUILayout.Button("Select Armor"))
        {
            var window = EditorWindow.GetWindow<ArmorUI>();
            window.Selected = true;
            window.Initialize(ref current, (int)armortype);
            window.Show();
        }
        GUILayout.EndHorizontal();
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
            Repaint();
        }
    }

    private void CreateAnimation()
    {
        ActorAnimation animation = new ActorAnimation();
        List<Sprite> sprites;

        if (downSprites.Count > 0)
        {
            sprites = new List<Sprite>(downSprites);
            sprites.Add(downSprites[0]);
            animation.down = ActorAnimation.ConstructAnimation(sprites, "test", "down", 30, true);
            animation.downIdle = ActorAnimation.ConstructAnimation(downSprites[0], "test", "downIdle", 30, true);
        }

        if (leftSprites.Count > 0)
        {
            sprites = new List<Sprite>(leftSprites);
            sprites.Add(leftSprites[0]);
            animation.left = ActorAnimation.ConstructAnimation(sprites, "test", "left", 30, true);
            animation.leftIdle = ActorAnimation.ConstructAnimation(leftSprites[0], "test", "leftIdle", 30, true);
        }

        if (upSprites.Count > 0)
        {
            sprites = new List<Sprite>(upSprites);
            sprites.Add(upSprites[0]);
            animation.up = ActorAnimation.ConstructAnimation(sprites, "test", "up", 30, true);
            animation.upIdle = ActorAnimation.ConstructAnimation(upSprites[0], "test", "upIdle", 30, true);
        }

        if (rightSprites.Count > 0)
        {
            sprites = new List<Sprite>(rightSprites);
            sprites.Add(rightSprites[0]);
            animation.right = ActorAnimation.ConstructAnimation(sprites, "test", "right", 30, true);
            animation.rightIdle = ActorAnimation.ConstructAnimation(rightSprites[0], "test", "rightIdle", 30, true);
        }

        animation.ConstructAnimationControl(element.Name);
    }

    protected override void Create()
    {
        Id++;
        element.Id = Id;
        CreatePrefab(element);
        listElements.AddItem(element.Name, element.Id);
        SetId();
    }

    protected override void UpdateListBox()
    {
        base.UpdateListBox();

        MainHand = elementObject.GetComponent<Weapon>();
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
}