using UnityEngine;
using System.Collections;
using UnityEditor;

public class SettingsUI : EditorWindow {

    bool browse = false;
    bool save = false;
    bool valid = false;
	public void Init() {
	}
    void OnGUI() {
        GUI.Label(new Rect(0, 30, 150, 20), "Unity Path");
        ProjectSettings.UnityPath = GUI.TextField(new Rect(100, 30, 300, 20), ProjectSettings.UnityPath);
        browse = GUI.Button(new Rect(100, 60, 150, 20), "Browse");
        save = GUI.Button(new Rect(0, 90, 100, 20), "Save") ;
        valid = ProjectSettings.CheckPath();
        if (save && valid )
        {
           ProjectSettings.SaveSettings();
           Debug.Log(ProjectSettings.UnityPath);
           this.Close();
        }
        else if (save && !valid)
        {
            Debug.Log("Error en la ruta de unity");
        }
       
    }
    void Update()
    {
        if (browse)
        {
            ProjectSettings.UnityPath = EditorUtility.OpenFilePanel("Select Unity.exe", "", "");
        }
    }
}
