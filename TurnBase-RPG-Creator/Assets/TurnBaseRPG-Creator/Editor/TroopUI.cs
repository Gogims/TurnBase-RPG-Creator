using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections.Generic;

public class TroopUI : CRUD<Troop>
{
    Vector2 ScrollPosition;
    Animator animations;
    const int previewX = Constant.BackgroundWidth/2;
    const int previewY = Constant.BackgroundHeight/2 + 20;
    AudioClip clip;

    TroopUI():base("Troop", new Rect(0, 0, 300, 940)) { }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, Constant.BackgroundWidth, 160), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Type = (Constant.EnemyType)EditorGUILayout.EnumPopup("Type:", element.Type);
        GUILayout.EndHorizontal();

        GUILayout.Label("Background: ", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Top Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Background", 1);
        }
        if (GUILayout.Button("Bottom Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Background", 2);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        string audioname = element.Background != null ? element.Background.name : string.Empty;
        GUI.enabled = false;
        EditorGUILayout.TextField(audioname);
        GUI.enabled = true;
        if (GUILayout.Button("Select Audio"))
        {
            EditorGUIUtility.ShowObjectPicker<AudioClip>(null, false, "Background_", 3);
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Animation", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        AddAnimation("Down", ref element.downSprites);
        AddAnimation("Left", ref element.leftSprites);
        AddAnimation("Up", ref element.upSprites);
        AddAnimation("Right", ref element.rightSprites);
        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        // Enemies
        GUILayout.BeginArea(new Rect(300, 160, Constant.BackgroundWidth, 140), "Enemies", EditorStyles.helpBox);
        GUILayout.Space(15);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.Enemies, DrawBattleEnemy, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();

        GUILayout.EndArea();        

        // Preview
        GUILayout.BeginArea(new Rect(300, 300, Constant.BackgroundWidth, Constant.BackgroundHeight + 40), "Preview", EditorStyles.helpBox);
        GUILayout.Space(15);
        AddFiles();

        if (element.BackgroundBottom != null)
        {
            GUI.DrawTexture(new Rect(0, 20, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundBottom.texture); 
        }
        if (element.BackgroundTop != null)
        {
            GUI.DrawTexture(new Rect(0, 20, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundTop.texture); 
        }
        
        foreach (var enemy in element.Enemies)
        {
            if (enemy != null)
            {
                DrawEnemy(enemy);

                if (enemy.Data.Image != null)
                {

                    enemy.RelativePosition = new Vector2(enemy.EnemyPosition.x / (Constant.BackgroundWidth / 2),
                                                        enemy.EnemyPosition.y / (Constant.BackgroundHeight / 2));
                } 
            }
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
    protected override void Delete()
    {

        Constant.RemoveScene("Resources/BattleMap/" + element.Id+".unity");
        base.Delete();
    }
    protected override void Create()
    {
        element.tag = "RPG-ENEMY";
        element.Id = System.Guid.NewGuid().ToString();
        var rb2D = elementObject.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
        rb2D.angularDrag = 0;
        rb2D.freezeRotation = true;

        SetIcon();
        CreateAnimation();

        if (element.downSprites.Count > 0)
        {
            SpriteRenderer character = elementObject.AddComponent<SpriteRenderer>();
            character.sprite = element.downSprites[0];
            character.sortingLayerName = "Actors";
        }        

        var collider = elementObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(element.downSprites[0].textureRect.width, element.downSprites[0].textureRect.height);

        CreatePrefab(element);
        string scene = EditorApplication.currentScene;
        BattleManager.CreateTroopScene(element);
        EditorApplication.OpenScene(scene);
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

        if (element.downSprites.Count > 0)
        {
            SpriteRenderer character = elementObject.GetComponent<SpriteRenderer>();
            character.sprite = element.downSprites[0];
            character.sortingLayerName = "Actors";
        }

        var collider = elementObject.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(element.downSprites[0].textureRect.width, element.downSprites[0].textureRect.height);

        CreatePrefab(element);
        string scene = EditorApplication.currentScene;
        BattleManager.CreateTroopScene(element);
        EditorApplication.OpenScene(scene);
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
            element.Icon = element.Enemies[0].Data.Image;
        }
    }

    private BattleEnemy DrawBattleEnemy(Rect position, BattleEnemy current)
    {
        if (current == null)
        {
            current = new BattleEnemy();
        }

        EditorGUI.TextField(new Rect(position.x, position.y, 200, position.height), current.Data.ActorName);
        if (GUI.Button(new Rect(position.x+200, position.y, 60, position.height), "Select"))
        {
            var window = EditorWindow.GetWindow<EnemyUI>();
            window.Selected = true;
            window.Initialize(ref current.Data);
            window.Show();
        }

        float maxX = current.Data.Image != null ? (Constant.BackgroundWidth / 2) - current.Data.Image.textureRect.width : Constant.BackgroundWidth / 2;
        float maxY = current.Data.Image != null ? (Constant.BackgroundHeight / 2) - current.Data.Image.textureRect.height : Constant.BackgroundHeight / 2;

        EditorGUI.LabelField(new Rect(position.x+280, position.y, 20, position.height), "X: ");
        current.EnemyPosition.x = EditorGUI.Slider(new Rect(position.x+300, position.y, 200, position.height), current.EnemyPosition.x, -Constant.BackgroundWidth / 2, maxX);
        EditorGUI.LabelField(new Rect(position.x+520, position.y, 20, position.height), "Y: ");
        current.EnemyPosition.y = EditorGUI.Slider(new Rect(position.x+540, position.y, 200, position.height), current.EnemyPosition.y, -maxY, (Constant.BackgroundHeight / 2));

        return current;
    }

    private void AddFiles()
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

            if (EditorGUIUtility.GetObjectPickerControlID() == 3)
            {
                element.Background = (AudioClip)EditorGUIUtility.GetObjectPickerObject();                
            }
        }

        Repaint();
    }

    private void DrawEnemy(BattleEnemy obj)
    {
        if (obj != null)
        {
            if (obj.Data.Image != null)
            {
                GUI.DrawTextureWithTexCoords(new Rect(obj.EnemyPosition.x + previewX, -obj.EnemyPosition.y + previewY, obj.Data.Image.textureRect.width, obj.Data.Image.textureRect.height),
                                                obj.Data.Image.texture,
                                                Constant.GetTextureCoordinate(obj.Data.Image)
                                            );
            } 
        }
    }
}