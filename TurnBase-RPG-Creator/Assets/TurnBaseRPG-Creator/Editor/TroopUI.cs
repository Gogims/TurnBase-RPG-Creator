using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections.Generic;

public class TroopUI : CRUD<Troop>
{
    Vector2 ScrollPosition;
    Animator animations;
    const int previewX = 10;
    const int previewY = 20;

    TroopUI():base("Troop", new Rect(0, 0, 300, 940)) { }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 160), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Name = EditorGUILayout.TextField("Name: ", element.Name);        

        GUILayout.Label("Background: ", EditorStyles.boldLabel);
        if (GUILayout.Button("Top Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Background", 1);
        }
        if (GUILayout.Button("Bottom Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Background", 2);
        }

        GUILayout.Label("Animation", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        AddAnimation("Down", ref element.downSprites);
        AddAnimation("Left", ref element.leftSprites);
        AddAnimation("Up", ref element.upSprites);
        AddAnimation("Right", ref element.rightSprites);
        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        // Enemies
        GUILayout.BeginArea(new Rect(300, 160, 600, 140), "Enemies", EditorStyles.helpBox);
        GUILayout.Space(15);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.Enemies, DrawBattleEnemy, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();

        GUILayout.EndArea();        

        // Preview
        GUILayout.BeginArea(new Rect(300, 300, 600, 620), "Preview", EditorStyles.helpBox);
        GUILayout.Space(15);
        AddBackground();

        if (element.BackgroundBottom != null)
        {
            GUI.DrawTexture(new Rect(previewX, previewY, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundBottom.texture); 
        }
        if (element.BackgroundTop != null)
        {
            GUI.DrawTexture(new Rect(previewX, previewY, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundTop.texture); 
        }
        
        foreach (var item in element.Enemies)
        {
            DrawEnemy(item);
        }
        GUILayout.EndArea();

        SaveButton = GUI.Button(new Rect(300, 920, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(400, 920, 100, 20), "Delete");
        GUI.enabled = true;
    }

    protected override GameObject NewGameObject()
    {
        elementObject = base.NewGameObject();
        animations = elementObject.AddComponent<Animator>();        

        return elementObject;
    }

    protected override void Create()
    {
        element.tag = "RPG-PLAYER";
        element.Id = System.Guid.NewGuid().ToString();        

        SetIcon();
        CreateAnimation();

        if (element.downSprites.Count > 0)
        {
            SpriteRenderer character = elementObject.AddComponent<SpriteRenderer>();
            character.sprite = element.downSprites[0];
            character.sortingLayerName = "Actors";
        }

        var rb2D = elementObject.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
        rb2D.angularDrag = 0;
        rb2D.freezeRotation = true;

        var collider = elementObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(element.downSprites[0].textureRect.width, element.downSprites[0].textureRect.height);

        //element.CreateTroopScene();
        CreatePrefab(element);
    }

    private void CreateAnimation()
    {
        ActorAnimation animation = new ActorAnimation("Troop");
        List<Sprite> sprites;

        if (animations == null)
        {
            animations = elementObject.GetComponent<Animator>();
        }

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

    protected override void Edit()
    {
        SetIcon();
        CreateAnimation();
        element.CreateTroopScene();
        base.Edit();
    }

    private void AddAnimation(string name, ref List<Sprite> animation)
    {
        if (GUILayout.Button(name + " (" + animation.Count.ToString() + ")"))
        {
            var window = EditorWindow.GetWindow<AnimationUI>();
            window.Init(ref animation, name + " Sprites");
            window.Show();
        }
    }

    private void SetIcon()
    {
        if (element.Enemies.Count > 0)
        {
            element.Icon = element.Enemies[0].Enemy.Image;
        }
    }

    private EnemyBattle DrawBattleEnemy(Rect position, EnemyBattle current)
    {
        if (current == null)
        {
            current = new EnemyBattle();
        }

        EditorGUI.TextField(new Rect(position.x, position.y, 120, position.height), current.Enemy.ActorName);
        if (GUI.Button(new Rect(position.x+120, position.y, 60, position.height), "Select"))
        {
            var window = EditorWindow.GetWindow<EnemyUI>();
            window.Selected = true;
            window.Initialize(ref current.Enemy);
            window.Show();
        }

        EditorGUI.LabelField(new Rect(position.x+180, position.y, 20, position.height), "X: ");
        current.EnemyPosition.x = EditorGUI.Slider(new Rect(position.x+200, position.y, 150, position.height), current.EnemyPosition.x, 0, Constant.BackgroundWidth);
        EditorGUI.LabelField(new Rect(position.x+350, position.y, 20, position.height), "Y: ");
        current.EnemyPosition.y = EditorGUI.Slider(new Rect(position.x+370, position.y, 150, position.height), current.EnemyPosition.y, 0, Constant.BackgroundHeight);

        return current;
    }

    private void AddBackground()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            if (EditorGUIUtility.GetObjectPickerControlID() == 1)
            {
                element.BackgroundTop = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            }

            if (EditorGUIUtility.GetObjectPickerControlID() == 2)
            {
                element.BackgroundBottom = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            } 
        }

        Repaint();
    }

    private void DrawEnemy(EnemyBattle obj)
    {
        if (obj != null)
        {
            if (obj.Enemy.Image != null && obj.EnemyPosition != null)
            {
                GUI.DrawTextureWithTexCoords(new Rect(obj.EnemyPosition.x + previewX, obj.EnemyPosition.y + previewY, obj.Enemy.Image.textureRect.width, obj.Enemy.Image.textureRect.height),
                                                obj.Enemy.Image.texture,
                                                Constant.GetTextureCoordinate(obj.Enemy.Image)
                                            );
            } 
        }
    }
}