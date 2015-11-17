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

    void OnGUI()
    {
        RenderLeftSide();

        // Configuraciones básicas
        GUILayout.BeginArea(new Rect(300, 0, 600, 200), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = !Selected;
        element.Name = element.Data.Ability = EditorGUILayout.TextField("Name: ", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description: ", element.Data.Description);
        element.Data.MinLevel = EditorGUILayout.IntSlider("Minimum Level: ", element.Data.MinLevel, 1, 99);
        element.Data.MPCost = EditorGUILayout.IntField("Mana Cost: ", element.Data.MPCost);
                
        if (GUI.Button(new Rect(0, 100, 400, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 100, element.Icon.textureRect.width, element.Icon.textureRect.height), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        GUILayout.EndArea();

        // Attack
        GUILayout.BeginArea(new Rect(300, 200, 600, 80), "Attack (Power + Type)", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.AttackType = (Constant.AttackType)EditorGUILayout.EnumPopup("Type: ", element.Data.AttackType);
        element.Data.AttackPower = EditorGUILayout.IntField("Power: ", element.Data.AttackPower);

        GUILayout.EndArea();

        // State
        GUILayout.BeginArea(new Rect(300, 280, 600, 100), "State (Offense = Apply. Defense = Remove)", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.Type = (Constant.OffenseDefense)EditorGUILayout.EnumPopup("Ability: ", element.Data.Type);
        element.Data.AreaOfEffect = (Constant.AOE)EditorGUILayout.EnumPopup("Area of Effect: ", element.Data.AreaOfEffect);

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
