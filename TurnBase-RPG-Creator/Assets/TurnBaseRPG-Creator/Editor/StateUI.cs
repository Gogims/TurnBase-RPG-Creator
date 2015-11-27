using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class StateUI : CRUD<State>
{
    AbstractState StateSelected;

    public StateUI() : base("State", new Rect(0, 0, 300, 530)) { }

    public void Initialize(ref AbstractState state)
    {
        StateSelected = state;
        Init();
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Configuraciones básicas
        GUILayout.BeginArea(new Rect(300, 0, 600, 180), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);
        GUI.enabled = !Selected;

        GUILayout.BeginHorizontal();
        element.Data.State = element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Data.Priority = EditorGUILayout.IntSlider(new GUIContent("Priority:"), element.Data.Priority, 1, 100);
        GUILayout.EndHorizontal();        
        
        EditorGUILayout.BeginHorizontal();
        element.Data.ActionRestriction = (Constant.ActionType)EditorGUILayout.EnumPopup("Restriction: ", element.Data.ActionRestriction);
        element.Data.TriggerTurn = (Constant.TriggerTurnType)EditorGUILayout.EnumPopup("Trigger: ", element.Data.TriggerTurn);
        EditorGUILayout.EndHorizontal();

        element.Data.ActiveOnSteps = EditorGUI.IntField(new Rect(0, 60, 200, 20), "Steps to Activate: ", element.Data.ActiveOnSteps);
        
        if (GUI.Button(new Rect(300, 60, 100, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 60, element.Icon.textureRect.width, element.Icon.textureRect.height), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        GUILayout.EndArea();

        // Daño
        GUILayout.BeginArea(new Rect(300, 180, 600, 100), "Action", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.Type = (Constant.DamageHeal)EditorGUILayout.EnumPopup("Type: ", element.Data.Type);
        GUI.enabled = element.Data.ActionRestriction != Constant.ActionType.DoNothing & !Selected;
        element.Data.FixedValue = EditorGUILayout.IntField("Constant " + element.Data.Type.ToString() + ":", element.Data.FixedValue);

        EditorGUILayout.BeginHorizontal();
        element.Data.AttackType = (Constant.AttackType)EditorGUILayout.EnumPopup("Attack Type: ", element.Data.AttackType);
        element.Data.ActionRate = EditorGUILayout.Slider(element.Data.Type.ToString() +" Rate(%)", element.Data.ActionRate, 0, 100);
        EditorGUILayout.EndHorizontal();
        
        GUI.enabled = GUI.enabled & element.Data.ActionRestriction == Constant.ActionType.AttackEnemyOrAlly;
        element.Data.PercentageAttackAlly = EditorGUILayout.Slider("Chance " + element.Data.Type.ToString() +" Ally(%): ", element.Data.PercentageAttackAlly, 0, 100);
        GUI.enabled = !Selected;
        GUILayout.EndArea();

        // Recuperación
        GUILayout.BeginArea(new Rect(300, 280, 600, 120), "Recovery Conditions", EditorStyles.helpBox);
        GUILayout.Space(15);
        element.Data.RemoveBattleEnd = EditorGUILayout.Toggle("Remove End of Battle", element.Data.RemoveBattleEnd);

        EditorGUILayout.BeginHorizontal();
        element.Data.RemoveByWalking = EditorGUILayout.Toggle("Remove by Steps Taken: ", element.Data.RemoveByWalking);
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

        element.Data.AutoRemovalTiming = (Constant.TurnTiming)EditorGUILayout.EnumPopup("Automatic Timed Release: ", element.Data.AutoRemovalTiming);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = element.Data.AutoRemovalTiming != Constant.TurnTiming.None & !Selected;
        EditorGUILayout.LabelField("Number of Turns Taken (Range): ");
        element.Data.DurationStartTurn = EditorGUILayout.IntField(element.Data.DurationStartTurn);        
        element.Data.DurationEndTurn = EditorGUILayout.IntField(element.Data.DurationEndTurn);        
        GUI.enabled = !Selected;
        EditorGUILayout.EndHorizontal();        

        GUILayout.EndArea();

        // Mensajes
        GUILayout.BeginArea(new Rect(300, 400, 600, 110), "Messages", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.MessageActor = EditorGUILayout.TextField("Inflicted Ally: ", element.Data.MessageActor);
        element.Data.MessageEnemy = EditorGUILayout.TextField("Inflicted Enemy: ", element.Data.MessageEnemy);
        element.Data.MessageRemains = EditorGUILayout.TextField("Continued Infliction: ", element.Data.MessageRemains);
        element.Data.MessageRecovery = EditorGUILayout.TextField("Recovery: ", element.Data.MessageRecovery);
        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, 510, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            SaveButton = GUI.Button(new Rect(300, 510, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 510, 100, 20), "Delete");
            GUI.enabled = true;
        }
    } 

    override protected void AssignElement()
    {
        StateSelected.ActionRestriction = element.Data.ActionRestriction;
        StateSelected.ActiveOnSteps = element.Data.ActiveOnSteps;
        StateSelected.AutoRemovalTiming = element.Data.AutoRemovalTiming;
        StateSelected.FixedValue = element.Data.FixedValue;
        StateSelected.DurationEndTurn = element.Data.DurationEndTurn;
        StateSelected.DurationStartTurn = element.Data.DurationStartTurn;
        StateSelected.MessageActor = element.Data.MessageActor;
        StateSelected.MessageEnemy = element.Data.MessageEnemy;
        StateSelected.MessageRecovery = element.Data.MessageRecovery;
        StateSelected.MessageRemains = element.Data.MessageRemains;
        StateSelected.NumberOfSteps = element.Data.NumberOfSteps;
        StateSelected.PercentageAttackAlly = element.Data.PercentageAttackAlly;
        StateSelected.ActionRate = element.Data.ActionRate;
        StateSelected.PercentRemoveByDamage = element.Data.PercentRemoveByDamage;
        StateSelected.Priority = element.Data.Priority;
        StateSelected.RemoveBattleEnd = element.Data.RemoveBattleEnd;
        StateSelected.RemoveByDamage = element.Data.RemoveByDamage;
        StateSelected.RemoveByWalking = element.Data.RemoveByWalking;
        StateSelected.State = element.Data.State;
        StateSelected.Steps = element.Data.Steps;
        StateSelected.TriggerTurn = element.Data.TriggerTurn;
        StateSelected.AttackType = element.Data.AttackType;
    }    
}
