using UnityEngine;
using UnityEditor;

public class AbilityUI : CRUD<Ability>
{
    AbstractAbility AbilitySelected;

    public AbilityUI() : base("Ability", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref AbstractAbility ability)
    {
        AbilitySelected = ability;
        Init();
    }

    override public void Init()
    {
        base.Init();

        foreach (var item in GetObjects())
        {
            listElements.AddItem(item.Name, item.Id);
        }
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Configuraciones básicas
        GUILayout.BeginArea(new Rect(300, 0, 600, 400), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = !Selected;
        element.Name = element.Data.Ability = EditorGUILayout.TextField("Name: ", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description: ", element.Data.Description);
        element.Data.MinLevel = EditorGUILayout.IntSlider("Minimum Level: ", element.Data.MinLevel, 1, 99);
        element.Data.AttackPower = EditorGUILayout.IntField("Attack Power: ", element.Data.AttackPower);
        element.Data.MPCost = EditorGUILayout.IntField("Mana Cost: ", element.Data.MPCost);

        GUILayout.Label("Type", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        element.Data.Type = (AbstractAbility.AbilityType)EditorGUILayout.EnumPopup("Ability: ", element.Data.Type);
        element.Data.AreaOfEffect = (AbstractAbility.AOE)EditorGUILayout.EnumPopup("Area of Effect: ", element.Data.AreaOfEffect);
        GUILayout.EndHorizontal();


        GUILayout.Label("State (Offense = Apply. Defense = Remove)", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.TextField(element.Data.State.State);
        if (GUILayout.Button("Select State"))
        {
            var window = EditorWindow.GetWindow<StateUI>();
            window.Selected = true;
            window.Initialize(ref element.Data.State);
            window.Show();
        }
        element.Data.ApplyStatePorcentage = EditorGUILayout.Slider("Apply State(%): ", element.Data.ApplyStatePorcentage, 0, 100);
        GUILayout.EndHorizontal();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(0, 380, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(0, 380, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(100, 380, 100, 20), "Delete");
            GUI.enabled = true;
        }

        GUILayout.EndArea();
    }

    override protected void AssignElement()
    {
        AbilitySelected.Ability = element.Name;
        AbilitySelected.ApplyStatePorcentage = element.Data.ApplyStatePorcentage;
        AbilitySelected.AttackPower = element.Data.AttackPower;
        AbilitySelected.Description = element.Data.Description;
        AbilitySelected.MinLevel = element.Data.MinLevel;
        AbilitySelected.MPCost = element.Data.MPCost;
        AbilitySelected.State = element.Data.State;        
    }
}
