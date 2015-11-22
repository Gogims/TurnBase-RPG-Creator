using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

public class TroopUI : CRUD<Troop>
{
    Vector2 ScrollPosition;
    const int previewX = 10;
    const int previewY = 20;

    TroopUI():base("Troop", new Rect(0, 0, 300, 940)) { }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 100), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Name = EditorGUILayout.TextField("Name: ", element.Name);        

        GUILayout.Label("Background: ", EditorStyles.boldLabel);
        if (GUILayout.Button("Top Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Texture>(null, false, "Background", 1);
        }
        if (GUILayout.Button("Bottom Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Texture>(null, false, "Background", 2);
        }

        GUILayout.EndArea();

        // Enemies
        GUILayout.BeginArea(new Rect(300, 100, 600, 200), "Enemies", EditorStyles.helpBox);
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
            GUI.DrawTexture(new Rect(previewX, previewY, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundBottom); 
        }
        if (element.BackgroundTop != null)
        {
            GUI.DrawTexture(new Rect(previewX, previewY, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundTop); 
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

    protected override void Create()
    {
        SetIcon();
        base.Create();
    }

    protected override void Edit()
    {
        SetIcon();
        base.Edit();
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
                element.BackgroundTop = (Texture)EditorGUIUtility.GetObjectPickerObject();
            }

            if (EditorGUIUtility.GetObjectPickerControlID() == 2)
            {
                element.BackgroundBottom = (Texture)EditorGUIUtility.GetObjectPickerObject();
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