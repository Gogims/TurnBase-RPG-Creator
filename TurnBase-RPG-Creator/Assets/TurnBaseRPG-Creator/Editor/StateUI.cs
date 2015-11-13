using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class StateUI : CRUD<State>
{
    AbstractState StateSelected;

    public StateUI() : base("State", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref AbstractState state)
    {
        StateSelected = state;
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
        element.Data.State = element.Name = EditorGUILayout.TextField("Name", element.Name);

        EditorGUILayout.BeginHorizontal();
        element.Data.ActionRestriction = (AbstractState.ActionType)EditorGUILayout.EnumPopup("Action Restriction ", element.Data.ActionRestriction);
        element.Data.Priority = EditorGUILayout.IntSlider(new GUIContent("Priority:"), element.Data.Priority, 1, 100);
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

        // Daño
        GUILayout.BeginArea(new Rect(300, 50, 600, 100), "Damage", EditorStyles.helpBox);
        GUILayout.Space(15);
        GUI.enabled = element.Data.ActionRestriction != AbstractState.ActionType.UnableToAct & !Selected;
        element.Data.ConstantDamage = EditorGUILayout.IntField("Constant Damage: ", element.Data.ConstantDamage);
        element.Data.PercentageDamage = EditorGUILayout.Slider("Damage Rate(%)", element.Data.PercentageDamage, 0, 100);
        element.Data.ActiveOnSteps = EditorGUILayout.IntField("Steps to Activate: ", element.Data.ActiveOnSteps);
        GUI.enabled = GUI.enabled & element.Data.ActionRestriction == AbstractState.ActionType.AttackEnemyOrAlly;
        element.Data.PercentageAttackAlly = EditorGUILayout.Slider("Chance Attack Ally(%)", element.Data.PercentageAttackAlly, 0, 100);
        GUI.enabled = !Selected;
        GUILayout.EndArea();

        // Recuperación
        GUILayout.BeginArea(new Rect(300, 150, 600, 120), "Recovery Conditions", EditorStyles.helpBox);
        GUILayout.Space(15);
        element.Data.RemoveBattleEnd = EditorGUILayout.Toggle("Remove End of Battle", element.Data.RemoveBattleEnd);

        EditorGUILayout.BeginHorizontal();
        element.Data.RemoveByWalking = EditorGUILayout.Toggle("Remove by Steps Taken", element.Data.RemoveByWalking);
        GUI.enabled = element.Data.RemoveByWalking;
        element.Data.NumberOfSteps = EditorGUILayout.IntField(element.Data.NumberOfSteps);
        GUI.enabled = !Selected;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        element.Data.RemoveByDamage = EditorGUILayout.Toggle("Inflicted Damage: ", element.Data.RemoveByDamage);
        GUI.enabled = element.Data.RemoveByDamage & !Selected;
        element.Data.PercentRemoveByDamage = EditorGUILayout.Slider(element.Data.PercentRemoveByDamage, 0, 100);
        GUI.enabled = !Selected;
        EditorGUILayout.EndHorizontal();

        element.Data.AutoRemovalTiming = (AbstractState.RemovalTiming)EditorGUILayout.EnumPopup("Automatic Timed Release: ", element.Data.AutoRemovalTiming);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = element.Data.AutoRemovalTiming != AbstractState.RemovalTiming.None & !Selected;
        EditorGUILayout.LabelField("Number of Turns Taken: ");
        element.Data.DurationStartTurn = EditorGUILayout.IntField(element.Data.DurationStartTurn);
        element.Data.DurationEndTurn = EditorGUILayout.IntField(element.Data.DurationEndTurn);        
        GUI.enabled = !Selected;
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();

        // Mensajes
        GUILayout.BeginArea(new Rect(300, 270, 600, 130), "Messages", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.MessageActor = EditorGUILayout.TextField("Inflicted Ally:", element.Data.MessageActor);
        element.Data.MessageEnemy = EditorGUILayout.TextField("Inflicted Enemy:", element.Data.MessageEnemy);
        element.Data.MessageRemains = EditorGUILayout.TextField("Continued Infliction:", element.Data.MessageRemains);
        element.Data.MessageRecovery = EditorGUILayout.TextField("Recovery:", element.Data.MessageRecovery);        

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
        StateSelected.ActionRestriction = element.Data.ActionRestriction;
        StateSelected.ActiveOnSteps = element.Data.ActiveOnSteps;
        StateSelected.AutoRemovalTiming = element.Data.AutoRemovalTiming;
        StateSelected.ConstantDamage = element.Data.ConstantDamage;
        StateSelected.DurationEndTurn = element.Data.DurationEndTurn;
        StateSelected.DurationStartTurn = element.Data.DurationStartTurn;
        StateSelected.MessageActor = element.Data.MessageActor;
        StateSelected.MessageEnemy = element.Data.MessageEnemy;
        StateSelected.MessageRecovery = element.Data.MessageRecovery;
        StateSelected.MessageRemains = element.Data.MessageRemains;
        StateSelected.NumberOfSteps = element.Data.NumberOfSteps;
        StateSelected.PercentageAttackAlly = element.Data.PercentageAttackAlly;
        StateSelected.PercentageDamage = element.Data.PercentageDamage;
        StateSelected.PercentRemoveByDamage = element.Data.PercentRemoveByDamage;
        StateSelected.Priority = element.Data.Priority;
        StateSelected.RemoveBattleEnd = element.Data.RemoveBattleEnd;
        StateSelected.RemoveByDamage = element.Data.RemoveByDamage;
        StateSelected.RemoveByWalking = element.Data.RemoveByWalking;
        StateSelected.State = element.Data.State;
        StateSelected.Steps = element.Data.Steps;        
    }    
}
