using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlayerUI : CRUD<Player>
{
    Animator animations;

    public PlayerUI() : base("Player", new Rect(0, 0, 300, 770)) { }    

    void OnGUI()
    {
        RenderLeftSide();        

        // Basic Settings Area
        GUILayout.BeginArea(new Rect(300, 0, 600, 290), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        element.Data.ActorName = element.Name = EditorGUILayout.TextField("Name", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description", element.Data.Description);
        element.Data.Level = EditorGUILayout.IntSlider(new GUIContent("Level:"), element.Data.Level, 1, 100);

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
        GUILayout.BeginArea(new Rect(300, 290, 600, 320), "Initial Equipment", EditorStyles.helpBox);
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
        GUILayout.BeginArea(new Rect(300, 610, 600, 140), "Design", EditorStyles.helpBox);
        GUILayout.Space(10);

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

        SaveButton = GUI.Button(new Rect(300, 750, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(400, 750, 100, 20), "Delete");
        GUI.enabled = true;
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
        ActorAnimation playerAnimation = new ActorAnimation("Player");
        List<Sprite> sprites;

        if (animations == null)
        {
            animations = elementObject.GetComponent<Animator>();
        }

        if (element.downSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.downSprites);
            sprites.Add(element.downSprites[0]);
            playerAnimation.down = playerAnimation.ConstructAnimation(sprites, element.Id, "down", 30, true);
            playerAnimation.downIdle = playerAnimation.ConstructAnimation(element.downSprites[0], element.Id, "downIdle", 30, true);
        }

        if (element.leftSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.leftSprites);
            sprites.Add(element.leftSprites[0]);
            playerAnimation.left = playerAnimation.ConstructAnimation(sprites, element.Id, "left", 30, true);
            playerAnimation.leftIdle = playerAnimation.ConstructAnimation(element.leftSprites[0], element.Id, "leftIdle", 30, true);
        }

        if (element.upSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.upSprites);
            sprites.Add(element.upSprites[0]);
            playerAnimation.up = playerAnimation.ConstructAnimation(sprites, element.Id, "up", 30, true);
            playerAnimation.upIdle = playerAnimation.ConstructAnimation(element.upSprites[0], element.Id, "upIdle", 30, true);
        }

        if (element.rightSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.rightSprites);
            sprites.Add(element.rightSprites[0]);
            playerAnimation.right = playerAnimation.ConstructAnimation(sprites, element.Id, "right", 30, true);
            playerAnimation.rightIdle = playerAnimation.ConstructAnimation(element.rightSprites[0], element.Id, "rightIdle", 30, true);
        }

        animations.runtimeAnimatorController = playerAnimation.ConstructAnimationControl(element.Id);
    }

    protected override void Create()
    {
        element.Id = System.Guid.NewGuid().ToString();
        element.Data.Image = element.Icon;

        CreateAnimation();

        if (element.downSprites.Count > 0)
        {
            SpriteRenderer character = elementObject.AddComponent<SpriteRenderer>();
            character.sprite = element.downSprites[0];
        }

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
        animations = elementObject.AddComponent<Animator>();

        element.downSprites = new List<Sprite>();
        element.leftSprites = new List<Sprite>();
        element.upSprites = new List<Sprite>();
        element.rightSprites = new List<Sprite>();

        return elementObject;
    }
}