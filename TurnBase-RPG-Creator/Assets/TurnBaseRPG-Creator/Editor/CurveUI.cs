using UnityEngine;
using UnityEditor;

public class CurveUI : EditorWindow
{
    public int Type;

    Formula curve;
    private GUIStyle currentStyle;
    private int ratio;

    public void Init(ref Formula f, Color curveColor)
    {
        curve = f;
        currentStyle = new GUIStyle(GUI.skin.box);
        currentStyle.normal.background = MakeTex(2, 2, curveColor);
        ratio = 10;

        if (curve.GetFormulaType() == Formula.FormulaType.XP)
            ratio = 1500;
    }

    void OnGUI()
    {
        int i = 0;
        int graphHeight = curve.GetListValue(Formula.MaxLevel) / ratio;        

        foreach (var item in curve.GenerateCurve())
        {
            i++;
            GUI.Box(new Rect(i * 10, graphHeight + 20, 10, -item/ratio), "", currentStyle);
        }

        EditorGUI.LabelField(new Rect(0, graphHeight + 60, 100, 20), "Base Value");
        curve.BaseValue = EditorGUI.IntSlider(new Rect(100, graphHeight + 60, 300, 20), curve.BaseValue, Formula.MinValue, 100);

        EditorGUI.LabelField(new Rect(0, graphHeight + 80, 100, 20), "Extra Value");
        curve.ExtraValue = EditorGUI.IntSlider(new Rect(100, graphHeight + 80, 300, 20), curve.ExtraValue, 1, 50);

        if (curve.GetFormulaType() == Formula.FormulaType.XP)
        {
            EditorGUI.LabelField(new Rect(0, graphHeight + 100, 100, 20), "Acceleration");
            curve.Acceleration = EditorGUI.IntSlider(new Rect(100, graphHeight + 100, 300, 20), curve.Acceleration, Formula.MinValue, Formula.MaxValue);
            graphHeight += 20;
        }

        graphHeight += 100;

        for (i = 1; i <= Formula.MaxLevel; i = i+4)
        {
            EditorGUI.LabelField(new Rect(0, graphHeight + i * 5, 200, 20), "Lv. " + i.ToString() + ": " + curve.GetListValue(i) + " " + curve.GetFormulaType().ToString());
            EditorGUI.LabelField(new Rect(200, graphHeight + i * 5, 200, 20), "Lv. " + (i + 1).ToString() + ": " + curve.GetListValue(i + 1) + " " + curve.GetFormulaType().ToString());
            EditorGUI.LabelField(new Rect(400, graphHeight + i * 5, 200, 20), "Lv. " + (i + 2).ToString() + ": " + curve.GetListValue(i + 2) + " " + curve.GetFormulaType().ToString());

            if (i + 3 > 99)
                break;

            EditorGUI.LabelField(new Rect(600, graphHeight + i * 5, 200, 20), "Lv. " + (i + 3).ToString() + ": " + curve.GetListValue(i + 3) + " " + curve.GetFormulaType().ToString());
            EditorGUI.LabelField(new Rect(800, graphHeight + i * 5, 200, 20), "Lv. " + (i + 4).ToString() + ": " + curve.GetListValue(i + 4) + " " + curve.GetFormulaType().ToString());
        }

        if (GUI.Button(new Rect(800, graphHeight + 20 + 20 * (Formula.MaxLevel / 4), 100, 20), "Exit"))
        {
            this.Close();
        }
    }

    void Update()
    {
        curve.Update();
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