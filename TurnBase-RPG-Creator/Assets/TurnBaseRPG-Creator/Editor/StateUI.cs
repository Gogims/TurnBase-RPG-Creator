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
        GUILayout.BeginArea(new Rect(300, 0, 600, 200), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);
        GUI.enabled = !Selected;

        element.Data.State = element.Name = EditorGUILayout.TextField("Name:", element.Name);
        element.Data.Priority = EditorGUILayout.IntSlider(new GUIContent("Priority:"), element.Data.Priority, 1, 100);
        element.Data.ActionRestriction = (Constant.ActionType)EditorGUILayout.EnumPopup("Behavior:", element.Data.ActionRestriction);
        GUI.enabled = GUI.enabled & element.Data.ActionRestriction != Constant.ActionType.None;

        if (!GUI.enabled)
        {
            element.Data.RestrictionRate = 0;
        }

        element.Data.RestrictionRate = EditorGUILayout.Slider("Apply Behavior(%):", element.Data.RestrictionRate, 0, 100);
        GUI.enabled = !Selected;

        if (GUI.Button(new Rect(0, 100, 100, 20), "Select Picture"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        AddObject();

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(100, 100, element.Icon.textureRect.width, element.Icon.textureRect.height), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        GUILayout.EndArea();

        // Daño
        GUILayout.BeginArea(new Rect(300, 200, 600, 100), "Action", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = element.Data.ActionRestriction != Constant.ActionType.DoNothing & !Selected;

        if (!GUI.enabled)
        {
            element.Data.ActionRate = element.Data.FixedValue = 0;
        }

        element.Data.Type = (Constant.DamageHeal)EditorGUILayout.EnumPopup("Type: ", element.Data.Type);        
        element.Data.FixedValue = EditorGUILayout.IntField("Constant " + element.Data.Type.ToString() + ":", element.Data.FixedValue);

        EditorGUILayout.BeginHorizontal();
        element.Data.AttackType = (Constant.AttackType)EditorGUILayout.EnumPopup("Attack Type: ", element.Data.AttackType);
        element.Data.ActionRate = EditorGUILayout.Slider(element.Data.Type.ToString() +" Rate(%)", element.Data.ActionRate, 0, 100);
        EditorGUILayout.EndHorizontal();       
        
        GUI.enabled = !Selected;
        GUILayout.EndArea();

        // Recuperación
        GUILayout.BeginArea(new Rect(300, 300, 600, 100), "Recovery Conditions (Remove State)", EditorStyles.helpBox);
        GUILayout.Space(15);
        element.Data.RemoveBattleEnd = EditorGUILayout.Toggle("End of Battle", element.Data.RemoveBattleEnd);
        
        EditorGUILayout.BeginHorizontal();
        element.Data.RemoveByDamage = EditorGUILayout.Toggle("By Taking Damage(%)", element.Data.RemoveByDamage);
        GUI.enabled = element.Data.RemoveByDamage & !Selected;
        element.Data.PercentRemoveByDamage = EditorGUILayout.Slider(element.Data.PercentRemoveByDamage, 0, 100);
        GUI.enabled = !Selected;
        EditorGUILayout.EndHorizontal();
                
        EditorGUILayout.BeginHorizontal();
        element.Data.AutoRemovalTiming = EditorGUILayout.Toggle("On Turn", element.Data.AutoRemovalTiming);
        GUI.enabled = element.Data.AutoRemovalTiming;        
        element.Data.TurnTotal = EditorGUILayout.IntSlider(element.Data.TurnTotal, 0, 100);
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
        StateSelected.AutoRemovalTiming = element.Data.AutoRemovalTiming;
        StateSelected.FixedValue = element.Data.FixedValue;
        StateSelected.TurnTotal = element.Data.TurnTotal;
        StateSelected.MessageActor = element.Data.MessageActor;
        StateSelected.MessageEnemy = element.Data.MessageEnemy;
        StateSelected.MessageRecovery = element.Data.MessageRecovery;
        StateSelected.MessageRemains = element.Data.MessageRemains;
        StateSelected.RestrictionRate = element.Data.RestrictionRate;
        StateSelected.ActionRate = element.Data.ActionRate;
        StateSelected.PercentRemoveByDamage = element.Data.PercentRemoveByDamage;
        StateSelected.Priority = element.Data.Priority;
        StateSelected.RemoveBattleEnd = element.Data.RemoveBattleEnd;
        StateSelected.RemoveByDamage = element.Data.RemoveByDamage;
        StateSelected.State = element.Data.State;
        StateSelected.AttackType = element.Data.AttackType;
    }    
}
