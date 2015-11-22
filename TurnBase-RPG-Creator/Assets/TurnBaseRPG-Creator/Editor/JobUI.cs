using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

public class JobUI : CRUD<Job>
{
    private Vector2 ScrollPosition;
    private AbstractJob SelectedJob;

    public JobUI() : base("Job", new Rect(0, 0, 300, 600)) { }

    public void Initialize(ref AbstractJob job)
    {
        SelectedJob = job;
        Init();
    }

    void OnGUI()
    {
        RenderLeftSide();

        GUI.enabled = !Selected;

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 200), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        element.Name = EditorGUILayout.TextField("Name", element.Name);

        if (GUI.Button(new Rect(0, 40, 400, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 40, element.Icon.textureRect.width, element.Icon.textureRect.height), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(300, 200, 600, 200), "Curves", EditorStyles.helpBox);
        GUILayout.Space(10);

        CurveUI(ref element.Data.XP, Color.white);
        CurveUI(ref element.Data.MaxHP, Color.red);
        CurveUI(ref element.Data.MaxMP, Color.blue);
        CurveUI(ref element.Data.Attack, Color.magenta);
        CurveUI(ref element.Data.Defense, Color.grey);
        CurveUI(ref element.Data.MagicAttack, Color.cyan);
        CurveUI(ref element.Data.MagicDefense, Color.grey);
        CurveUI(ref element.Data.Agility, Color.green);
        CurveUI(ref element.Data.Luck, Color.yellow);

        GUILayout.EndArea();

        // Skills
        GUILayout.BeginArea(new Rect(300, 400, 600, 180), "Skills", EditorStyles.helpBox);
        GUILayout.Space(10);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.Data.Abilities, DrawAbilityUI, ReorderableListFlags.DisableReordering);
        GUILayout.EndScrollView();  
        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, position.height - 20, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(300, 580, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 580, 100, 20), "Delete");
            GUI.enabled = true;
        }
    }

    private AbstractAbility DrawAbilityUI(Rect position, AbstractAbility Ability)
    {
        if (Ability == null)
        {
            Ability = new AbstractAbility();
        }
        
        GUI.enabled = false;
        GUI.TextField(new Rect(position.x, position.y, position.width-100, position.height), Ability.Ability);
        GUI.enabled = true;
        if (GUI.Button(new Rect(position.width - 100, position.y, 100, position.height), "Select Ability"))
        {
            var window = EditorWindow.GetWindow<AbilityUI>();
            window.Selected = true;
            window.Initialize(ref Ability);
            window.Show();
        }

        return Ability;
    }

    private void CurveUI(ref Formula curve, Color curveColor)
    {
        GUILayout.BeginHorizontal();
        GUI.enabled = false;
        string description;

        if (curve.GetFormulaType() == Formula.FormulaType.XP)
            description = curve.BaseValue.ToString() + ", " + curve.Acceleration.ToString() + ", " + curve.ExtraValue.ToString();
        else
            description = curve.BaseValue.ToString() + ", " + curve.ExtraValue.ToString();

        EditorGUILayout.TextField(curve.GetName(), description);
        GUI.enabled = true && !Selected;

        if (GUILayout.Button("..."))
        {
            var window = EditorWindow.GetWindow<CurveUI>();
            window.Init(ref curve, curveColor);
            window.Show();
        }
        GUILayout.EndHorizontal();
    }

    protected override void AssignElement()
    {
        SelectedJob.Agility = element.Data.Agility;
        SelectedJob.Attack = element.Data.Attack;
        SelectedJob.Defense = element.Data.Defense;        
        SelectedJob.Luck = element.Data.Luck;
        SelectedJob.MagicAttack = element.Data.MagicAttack;
        SelectedJob.MagicDefense = element.Data.MagicDefense;
        SelectedJob.MaxHP = element.Data.MaxHP;
        SelectedJob.MaxMP = element.Data.MaxMP;
        SelectedJob.JobName = element.Name;
        SelectedJob.XP = element.Data.XP;
    }

    protected override GameObject NewGameObject()
    {
        elementObject = base.NewGameObject();

        element.Data.XP = new Formula();
        element.Data.MaxHP = new Formula(Formula.FormulaType.MaxHP);
        element.Data.MaxMP = new Formula(Formula.FormulaType.MaxMP);
        element.Data.Attack = new Formula(Formula.FormulaType.Attack);
        element.Data.Defense = new Formula(Formula.FormulaType.Defense);
        element.Data.MagicAttack = new Formula(Formula.FormulaType.MagicAttack);
        element.Data.MagicDefense = new Formula(Formula.FormulaType.MagicDefense);
        element.Data.Agility = new Formula(Formula.FormulaType.Agility);
        element.Data.Luck = new Formula(Formula.FormulaType.Luck);

        return elementObject;
    }
}
