using UnityEngine;
using UnityEditor;

public class TroopUI : CRUD<Troop>
{
    AbstractEnemy LeftEnemy;
    AbstractEnemy MiddleEnemy;
    AbstractEnemy RightEnemy;

    TroopUI():base("Troop", new Rect(0, 0, 300, 820)) { }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 800), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Name = EditorGUILayout.TextField("Name: ", element.Name);

        GUILayout.Label("Enemies: ", EditorStyles.boldLabel);
        AddEnemy("Left", ref LeftEnemy);
        AddEnemy("Middle", ref MiddleEnemy);
        AddEnemy("Right", ref RightEnemy);

        GUILayout.Label("Background: ", EditorStyles.boldLabel);
        if (GUILayout.Button("Top Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Texture>(null, false, "Background", 1);
        }
        if (GUILayout.Button("Bottom Background"))
        {
            EditorGUIUtility.ShowObjectPicker<Texture>(null, false, "Background", 2);
        }

        AddBackground();

        GUI.DrawTexture(new Rect(10, 200, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundBottom);
        GUI.DrawTexture(new Rect(10, 200, Constant.BackgroundWidth, Constant.BackgroundHeight), element.BackgroundTop);        

        GUILayout.EndArea();

        SaveButton = GUI.Button(new Rect(300, 800, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(400, 800, 100, 20), "Delete");
        GUI.enabled = true;
    }

    protected override GameObject NewGameObject()
    {
        elementObject = base.NewGameObject();

        LeftEnemy = new AbstractEnemy();
        MiddleEnemy = new AbstractEnemy();
        RightEnemy = new AbstractEnemy();

        return elementObject;
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
        if (LeftEnemy.Image != null)
        {
            element.Icon = LeftEnemy.Image;
        }
        else if (MiddleEnemy.Image != null)
        {
            element.Icon = MiddleEnemy.Image;
        }
        else if (RightEnemy.Image != null)
        {
            element.Icon = RightEnemy.Image;
        }
    }

    private void AddEnemy(string name, ref AbstractEnemy current)
    {
        GUILayout.BeginHorizontal();
        GUILayout.TextField(current.Description);
        if (GUILayout.Button("Select " + name + " Enemy"))
        {
            var window = EditorWindow.GetWindow<EnemyUI>();
            window.Selected = true;
            window.Initialize(ref current);
            window.Show();
        }
        GUILayout.EndHorizontal();
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
}