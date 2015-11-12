using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class StateUI : CRUD<State>
{
    public StateUI() : base("State", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref State state)
    {
        AssignedElement = state;
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
        GUILayout.BeginArea(new Rect(300, 0, 600, 50), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(10);

        GUI.enabled = !Selected;
        element.Name = EditorGUILayout.TextField("Name", element.Name);

        EditorGUILayout.BeginHorizontal();
        element.ActionRestriction = (State.ActionType)EditorGUILayout.EnumPopup("Action Restriction ", element.ActionRestriction);
        element.Priority = EditorGUILayout.IntSlider(new GUIContent("Priority:"), element.Priority, 1, 100);
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

        // Daño
        GUILayout.BeginArea(new Rect(300, 50, 600, 100), "Damage", EditorStyles.helpBox);
        GUILayout.Space(15);
        GUI.enabled = element.ActionRestriction != State.ActionType.UnableToAct;
        element.ConstantDamage = EditorGUILayout.IntField("Constant Damage: ", element.ConstantDamage);
        element.PercentageDamage = EditorGUILayout.Slider("Damage Rate(%)", element.PercentageDamage, 0, 100);
        element.ActiveOnSteps = EditorGUILayout.IntField("Steps to Activate: ", element.ActiveOnSteps);
        GUI.enabled = GUI.enabled & element.ActionRestriction == State.ActionType.AttackEnemyOrAlly;
        element.PercentageAttackAlly = EditorGUILayout.Slider("Chance Attack Ally(%)", element.PercentageAttackAlly, 0, 100);
        GUI.enabled = true;
        GUILayout.EndArea();

        // Recuperación
        GUILayout.BeginArea(new Rect(300, 150, 600, 120), "Recovery Conditions", EditorStyles.helpBox);
        GUILayout.Space(15);
        element.RemoveBattleEnd = EditorGUILayout.Toggle("Remove End of Battle", element.RemoveBattleEnd);

        EditorGUILayout.BeginHorizontal();
        element.RemoveByWalking = EditorGUILayout.Toggle("Remove by Steps Taken", element.RemoveByWalking);
        GUI.enabled = element.RemoveByWalking;
        element.NumberOfSteps = EditorGUILayout.IntField(element.NumberOfSteps);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        element.RemoveByDamage = EditorGUILayout.Toggle("Inflicted Damage: ", element.RemoveByDamage);
        GUI.enabled = element.RemoveByDamage;
        element.PercentRemoveByDamage = EditorGUILayout.Slider(element.PercentRemoveByDamage, 0, 100);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        element.AutoRemovalTiming = (State.RemovalTiming)EditorGUILayout.EnumPopup("Automatic Timed Release: ", element.AutoRemovalTiming);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = element.AutoRemovalTiming != State.RemovalTiming.None;
        EditorGUILayout.LabelField("Number of Turns Taken: ");
        element.DurationStartTurn = EditorGUILayout.IntField(element.DurationStartTurn);
        element.DurationEndTurn = EditorGUILayout.IntField(element.DurationEndTurn);        
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();

        // Mensajes
        GUILayout.BeginArea(new Rect(300, 270, 600, 130), "Messages", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.MessageActor = EditorGUILayout.TextField("Inflicted Ally:", element.MessageActor);
        element.MessageEnemy = EditorGUILayout.TextField("Inflicted Enemy:", element.MessageEnemy);
        element.MessageRemains = EditorGUILayout.TextField("Continued Infliction:", element.MessageRemains);
        element.MessageRecovery = EditorGUILayout.TextField("Recovery:", element.MessageRecovery);

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(0, 110, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(0, 110, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(100, 110, 100, 20), "Delete");
            GUI.enabled = true;
        }

        GUILayout.EndArea();
    } 

    override protected void AssignElement()
    {
        AssignedElement.Name = element.Name;     
    }    
}
