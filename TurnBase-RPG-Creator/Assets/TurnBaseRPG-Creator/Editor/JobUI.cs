﻿using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

public class JobUI : CRUD<Job>
{
    private Vector2 ScrollPosition;

    public JobUI() : base("Job", new Rect(0, 0, 300, 600)) { }

    public void Initialize(ref Job a)
    {
        AssignedElement = a;
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

        // Skills
        GUILayout.BeginArea(new Rect(300, 400, 600, 180), "Skills", EditorStyles.helpBox);
        GUILayout.Space(10);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        ReorderableListGUI.ListField(element.Abilities, DrawAbilityUI, ReorderableListFlags.DisableReordering);
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