using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class JobUI : CRUD<Job>
{
    public JobUI(): base("Job", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref Job a)
    {
        AssignedElement = a;
        Init();
    }

    public override void Init()
    {
        base.Init();
        elementObject = NewGameObject();

        foreach (var item in GetObjects())
        {
            listElements.AddItem(item.Name, item.Id);
        }
    }

    void OnGUI()
    {
        RenderLeftSide();

        GUI.enabled = !Selected;

        GUILayout.BeginArea(new Rect(300, 0, 600, 250), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        element.Name = EditorGUILayout.TextField("Name", element.Name);
        CurveUI(ref element.XP, Color.white);
        CurveUI(ref element.MaxHP, Color.red);
        CurveUI(ref element.MaxMP, Color.blue);
        CurveUI(ref element.Attack, Color.magenta);
        CurveUI(ref element.Defense, Color.grey);
        CurveUI(ref element.MagicAttack, Color.cyan);
        CurveUI(ref element.MagicDefense, Color.grey);
        CurveUI(ref element.Agility, Color.green);
        CurveUI(ref element.Luck, Color.yellow);

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(300, 250, 600, 150), "Skills", EditorStyles.helpBox);
        GUILayout.Space(10);

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(0, 130, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(0, 130, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(100, 130, 100, 20), "Delete");
            GUI.enabled = true;
        }

        GUILayout.EndArea();
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
        AssignedElement.Agility = element.Agility;
        AssignedElement.Attack = element.Attack;
        AssignedElement.Defense = element.Defense;
        AssignedElement.Id = element.Id;
        AssignedElement.Luck = element.Luck;
        AssignedElement.MagicAttack = element.MagicAttack;
        AssignedElement.MagicDefense = element.MagicDefense;
        AssignedElement.MaxHP = element.MaxHP;
        AssignedElement.MaxMP = element.MaxMP;
        AssignedElement.Name = element.Name;
        AssignedElement.XP = element.XP;
    }

    protected override GameObject NewGameObject()
    {
        elementObject = base.NewGameObject();

        element.XP = new Formula();
        element.MaxHP = new Formula(Formula.FormulaType.MaxHP);
        element.MaxMP = new Formula(Formula.FormulaType.MaxMP);
        element.Attack = new Formula(Formula.FormulaType.Attack);
        element.Defense = new Formula(Formula.FormulaType.Defense);
        element.MagicAttack = new Formula(Formula.FormulaType.MagicAttack);
        element.MagicDefense = new Formula(Formula.FormulaType.MagicDefense);
        element.Agility = new Formula(Formula.FormulaType.Agility);
        element.Luck = new Formula(Formula.FormulaType.Luck);

        return elementObject;
    }
}
