using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

public class ItemUI : CRUD<Usable>
{
    private Vector2 ScrollPosition;
    private AbstractUsable UsableSelected;

    public ItemUI() : base("Item", new Rect(0, 0, 300, 500)) { }

    public void Initialize(ref AbstractUsable item)
    {
        UsableSelected = item;
        Init();
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Configuraciones básicas
        GUILayout.BeginArea(new Rect(300, 0, 600, 200), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = !Selected;
        element.Data.ItemName = element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description: ", element.Data.Description);
        element.Data.Price = EditorGUILayout.IntField("Price: ", element.Data.Price);

        GUILayout.BeginHorizontal();
        element.Data.Attribute = (Constant.Attribute)EditorGUILayout.EnumPopup("Enhance/Dimish: ", element.Data.Attribute);

        GUI.enabled = element.Data.Attribute != Constant.Attribute.None;

        if (element.Data.Attribute == Constant.Attribute.None)
            element.Data.Power = 0;

        element.Data.Power = EditorGUILayout.IntField(element.Data.Power);
        GUI.enabled = true;

        GUILayout.EndHorizontal();

        if (GUI.Button(new Rect(0, 100, 400, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Item", 1);
        }

        AddObject();

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 100, element.Icon.textureRect.width, element.Icon.textureRect.height), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        GUILayout.EndArea();

        // Item Type
        GUILayout.BeginArea(new Rect(300, 200, 600, 100), "Item Type", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.Data.Consumeable = EditorGUILayout.Toggle("Consumeable ", element.Data.Consumeable);
        element.Data.AreaOfEffect = (Constant.AOE)EditorGUILayout.EnumPopup("Area of Effect: ", element.Data.AreaOfEffect);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        element.Data.KeyItem = EditorGUILayout.Toggle("Key Item ", element.Data.KeyItem);
        element.Data.Available = (Constant.ItemAvailable)EditorGUILayout.EnumPopup("Available: ", element.Data.Available);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        // Advance Settings
        GUILayout.BeginArea(new Rect(300, 300, 600, 180), "Advance Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.Data.StateType = (Constant.OffenseDefense)EditorGUILayout.EnumPopup("Type: ", element.Data.StateType);
        element.Data.HitRate = EditorGUILayout.Slider("Apply Rate: ", element.Data.HitRate, 0, 100);        
        GUILayout.EndHorizontal();

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.Data.States, DrawStateUI, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();    
        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, 480, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(300, 480, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 480, 100, 20), "Delete");
            GUI.enabled = true;
        }
    }

    protected override void Create()
    {
        element.Data.Image = element.Icon;
        base.Create();
    }

    protected override void AssignElement()
    {
        UsableSelected.AreaOfEffect = element.Data.AreaOfEffect;
        UsableSelected.Available = element.Data.Available;
        UsableSelected.Consumeable = element.Data.Consumeable;
        UsableSelected.HitRate = element.Data.HitRate;
        UsableSelected.ItemName = element.Data.ItemName;
        UsableSelected.KeyItem = element.Data.KeyItem;
        UsableSelected.States = element.Data.States;
        UsableSelected.StateType = element.Data.StateType;
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