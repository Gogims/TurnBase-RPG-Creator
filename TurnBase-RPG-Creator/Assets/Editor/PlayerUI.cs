using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlayerUI : CRUD<Player>
{
    public PlayerUI() : base("Player") { }

    public override void Init()
    {
        element = new Player();
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

        // Text field to upload image
        GUI.enabled = false;
        GUI.TextField(new Rect(0, 280, 300, 20), spritename);
        GUI.enabled = true;

        // Button to upload image
        if (GUI.Button(new Rect(300, 280, 100, 20), "Select Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 280, element.Image.textureRect.width, element.Image.textureRect.height), element.Image.texture, element.GetTextureCoordinate());
        }

        SaveButton = GUI.Button(new Rect(0, 380, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 380, 100, 20), "Delete");
        GUI.enabled = true;

        GUI.Label(new Rect(0, 320, 100, 20), "Animation", EditorStyles.boldLabel);
        if (GUI.Button(new Rect(0, 340, 50, 20), "Down"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.downSprites, "Down Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(75, 340, 50, 20), "Left"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.leftSprites, "Left Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(150, 340, 50, 20), "Up"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.upSprites, "Up Sprites");
            window.Show();
        }
        if (GUI.Button(new Rect(225, 340, 50, 20), "Right"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref element.rightSprites, "Right Sprites");
            window.Show();
        }

        GUILayout.EndArea();

        if (CreateButton)
        {
            CreateAnimation();
        }
    }

    public override void GetNewObject(ref Player e)
    {
        e = new Player();
    }

    public override void AssignElement(ref Player component)
    {
        component.Name = element.Name;
        component.Description = element.Description;
        component.Stats = element.Stats;
        component.Id = element.Id;
        component.Image = element.Image;
        component.Level = element.Level;
        component.HP = element.HP;
        component.MP = element.MP;
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
            animation.down = ActorAnimation.ConstructAnimation(sprites, "test", "down", 30, true);
            animation.downIdle = ActorAnimation.ConstructAnimation(element.downSprites[0], "test", "downIdle", 30, true);
        }

        if (element.leftSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.leftSprites);
            sprites.Add(element.leftSprites[0]);
            animation.left = ActorAnimation.ConstructAnimation(sprites, "test", "left", 30, true);
            animation.leftIdle = ActorAnimation.ConstructAnimation(element.leftSprites[0], "test", "leftIdle", 30, true);
        }

        if (element.upSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.upSprites);
            sprites.Add(element.upSprites[0]);
            animation.up = ActorAnimation.ConstructAnimation(sprites, "test", "up", 30, true);
            animation.upIdle = ActorAnimation.ConstructAnimation(element.upSprites[0], "test", "upIdle", 30, true);
        }

        if (element.rightSprites.Count > 0)
        {
            sprites = new List<Sprite>(element.rightSprites);
            sprites.Add(element.rightSprites[0]);
            animation.right = ActorAnimation.ConstructAnimation(sprites, "test", "right", 30, true);
            animation.rightIdle = ActorAnimation.ConstructAnimation(element.rightSprites[0], "test", "rightIdle", 30, true);
        }

        animation.ConstructAnimationControl("test");
    }
}