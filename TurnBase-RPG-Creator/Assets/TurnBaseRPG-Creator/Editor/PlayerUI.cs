using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlayerUI : CRUD<Player>
{
    public Weapon MainHand;
    public Armor Helmet;
    public Armor UpperBody;
    public Armor LowerBody;
    public Armor Feet;
    public Armor Ring;
    public Armor Necklace;

    Animator animations;

    public PlayerUI() : base("Player", new Rect(0, 0, 300, 730)) { }

    public override void Init()
    {
        element = new Player();
        base.Init();
        NewPlayer();        

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
            window.Init(ref element.downSprites, "Down Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(75, 80, 50, 20), "Left"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.leftSprites, "Left Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(150, 80, 50, 20), "Up"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.upSprites, "Up Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(225, 80, 50, 20), "Right"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.rightSprites, "Right Sprites");
            window.Show();
        }

        SaveButton = GUI.Button(new Rect(0, 140, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 140, 100, 20), "Delete");
        GUI.enabled = true;

        GUILayout.EndArea();
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

        if (element.downSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.downSprites);
            sprites.Add(element.downSprites[0]);
            animation.down = ActorAnimation.ConstructAnimation(sprites, element.Name, "down", 30, true);
            animation.downIdle = ActorAnimation.ConstructAnimation(element.downSprites[0], element.Name, "downIdle", 30, true);
        }

        if (element.leftSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.leftSprites);
            sprites.Add(element.leftSprites[0]);
            animation.left = ActorAnimation.ConstructAnimation(sprites, element.Name, "left", 30, true);
            animation.leftIdle = ActorAnimation.ConstructAnimation(element.leftSprites[0], element.Name, "leftIdle", 30, true);
        }

        if (element.upSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.upSprites);
            sprites.Add(element.upSprites[0]);
            animation.up = ActorAnimation.ConstructAnimation(sprites, element.Name, "up", 30, true);
            animation.upIdle = ActorAnimation.ConstructAnimation(element.upSprites[0], element.Name, "upIdle", 30, true);
        }

        if (element.rightSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.rightSprites);
            sprites.Add(element.rightSprites[0]);
            animation.right = ActorAnimation.ConstructAnimation(sprites, element.Name, "right", 30, true);
            animation.rightIdle = ActorAnimation.ConstructAnimation(element.rightSprites[0], element.Name, "rightIdle", 30, true);
        }

        animations.runtimeAnimatorController = animation.ConstructAnimationControl(element.Name);        
    }

    protected override void Create()
    {
        Id++;
        element.Id = Id;

        CreateAnimation();
        CreatePrefab(element);
        listElements.AddItem(element.Name, element.Id);
        SetId();
    }

    protected override void UpdateListBox()
    {
        base.UpdateListBox();

        MainHand = elementObject.GetComponent<Weapon>();
        Armor[] armors = elementObject.GetComponents<Armor>();
        animations = elementObject.GetComponent<Animator>();

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
        NewPlayer();

        return elementObject;
    }

    private void NewPlayer()
    {
        MainHand = elementObject.AddComponent<Weapon>();

        Helmet = elementObject.AddComponent<Armor>();
        UpperBody = elementObject.AddComponent<Armor>();
        LowerBody = elementObject.AddComponent<Armor>();
        Ring = elementObject.AddComponent<Armor>();
        Necklace = elementObject.AddComponent<Armor>();
        Feet = elementObject.AddComponent<Armor>();

        animations = elementObject.AddComponent<Animator>();

        element.downSprites = new List<Sprite>();
        element.leftSprites = new List<Sprite>();
        element.upSprites = new List<Sprite>();
        element.rightSprites = new List<Sprite>();        
    }
}