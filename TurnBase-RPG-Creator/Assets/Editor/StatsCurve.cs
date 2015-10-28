using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class StatsCurve : EditorWindow
{
    public int Type;

    Formula curve;
    private GUIStyle currentStyle;
    List<Rect> positions = new List<Rect>();

    public void Init(int _type)
    {
        Type = _type;
        curve = new Formula(Type);
    }

    void OnGUI()
    {        
        int i = 0;
        int graphHeight = curve.GetValue(Formula.MaxLevel) / 10;
        InitStyles();

        foreach (var item in curve.GenerateCurve())
        {
            i++;
            GUI.Box(new Rect(i * 10, graphHeight + 20, 10, -item / 10), "", currentStyle);
        }

        EditorGUI.LabelField(new Rect(0, graphHeight + 60, 100, 20), "Base Value");
        curve.BaseValue = EditorGUI.IntSlider(new Rect(100, graphHeight + 60, 300, 20), curve.BaseValue, Formula.MinValue, 100);

        EditorGUI.LabelField(new Rect(0, graphHeight + 80, 100, 20), "Extra Value");
        curve.ExtraValue = EditorGUI.IntSlider(new Rect(100, graphHeight + 80, 300, 20), curve.ExtraValue, 1, 50);

        for (i = 1; i <= Formula.MaxLevel; i = i+4)
        {
            EditorGUI.LabelField(new Rect(0, graphHeight + 120 + i*5, 300, 20), "Lv. " + i.ToString() + ": " + curve.GetValue(i) + curve.GetName());
            EditorGUI.LabelField(new Rect(300, graphHeight + 120 + i*5, 300, 20), "Lv. " + (i+1).ToString() + ": " + curve.GetValue(i+1) + curve.GetName());
            EditorGUI.LabelField(new Rect(600, graphHeight + 120 + i*5, 300, 20), "Lv. " + (i+2).ToString() + ": " + curve.GetValue(i+2) + curve.GetName());

            if (i + 3 > 99)
                break;

            EditorGUI.LabelField(new Rect(900, graphHeight + 120 + i*5, 300, 20), "Lv. " + (i+3).ToString() + ": " + curve.GetValue(i+3) + curve.GetName());
        }
    }

    void Update()
    {
        curve.UpdateStat(Type);
    }

    private void InitStyles()
    {
        if (currentStyle == null)
        {
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2, Color.yellow);
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}