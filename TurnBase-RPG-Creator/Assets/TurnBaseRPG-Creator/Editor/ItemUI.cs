using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

public class ItemUI : CRUD<Usable>
{
    private Vector2 ScrollPosition;

    public ItemUI() : base("Item", new Rect(0, 0, 300, 400)) { }
    
    void OnGUI()
    {
        RenderLeftSide();

        // Configuraciones básicas
        GUILayout.BeginArea(new Rect(300, 0, 600, 100), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = !Selected;
        element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Description = EditorGUILayout.TextField("Description: ", element.Description);
        element.Price = EditorGUILayout.IntField("Price: ", element.Price);
        GUILayout.EndArea();

        // Item Type
        GUILayout.BeginArea(new Rect(300, 100, 600, 100), "Item Type", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.Consumeable = EditorGUILayout.Toggle("Consumeable ", element.Consumeable);
        element.AreaOfEffect = (Constant.AOE)EditorGUILayout.EnumPopup("Area of Effect: ", element.AreaOfEffect);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        element.KeyItem = EditorGUILayout.Toggle("Key Item ", element.KeyItem);
        element.Available = (Constant.ItemAvailable)EditorGUILayout.EnumPopup("Available: ", element.Available);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        // Advance Settings
        GUILayout.BeginArea(new Rect(300, 200, 600, 180), "Advance Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.StateType = (Constant.OffenseDefense)EditorGUILayout.EnumPopup("Type: ", element.StateType);
        element.HitRate = EditorGUILayout.Slider("Hit Rate: ", element.HitRate, 0, 100);        
        GUILayout.EndHorizontal();

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.States, DrawStateUI, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();    
        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, 380, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(300, 380, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 380, 100, 20), "Delete");
            GUI.enabled = true;
        }
    }

    private AbstractState DrawStateUI(Rect position, AbstractState state)
    {
        if (state == null)
        {
            state = new AbstractState();
        }

        GUI.enabled = false;
        GUI.TextField(new Rect(position.x, position.y, position.width - 100, position.height), state.State);
        GUI.enabled = true;
        if (GUI.Button(new Rect(position.width - 100, position.y, 100, position.height), "Select State"))
        {
            var window = EditorWindow.GetWindow<StateUI>();
            window.Selected = true;
            window.Initialize(ref state);
            window.Show();
        }

        return state;
    }
}