using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ExpCurve : EditorWindow
{
    Formula curve;

    public void Init()
    {
        curve = new Formula();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Base Value");
        curve.BaseValue = EditorGUILayout.IntSlider(curve.BaseValue, Formula.MinValue, Formula.MaxValue);
        int i = 0;

        foreach (var item in curve.GenerateCurve())
        {
            i++;
            //EditorGUILayout.LabelField(item.ToString());

            GUI.Box(new Rect(i, 0, 1, item), "test");
        }        
    }

    void Update()
    {

    }
}