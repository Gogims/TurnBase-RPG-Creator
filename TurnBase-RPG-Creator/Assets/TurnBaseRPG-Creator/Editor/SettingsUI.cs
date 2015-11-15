using UnityEngine;
using System.Collections;
using UnityEditor;

public class SettingsUI : EditorWindow {

    bool browse = false;
    bool save = false;
    bool error = false;

    ErrorHandler err;
	public void Init() {
        err = new ErrorHandler();
        err.InsertPropertyError("Path", ProjectSettings.UnityPath.Length, "*You have to select unity path",new Rect(0,0,100,20));
        err.InsertCondition("Path", 0, ErrorCondition.Greater, LogicalOperators.None);
	}
    void OnGUI() {
        if (err == null)
            Init();

        GUI.Label(new Rect(0, 30, 150, 20), "Unity Path");
        ProjectSettings.UnityPath = GUI.TextField(new Rect(100, 30, 300, 20), ProjectSettings.UnityPath);
        browse = GUI.Button(new Rect(100, 60, 150, 20), "Browse");
        save = GUI.Button(new Rect(0, 90, 100, 20), "Save") ;
        UpdateValidationVal();        
        error = err.CheckErrors();
        err.ShowErrors();
        if (save && ProjectSettings.SaveSettings() && !error)
        {
           this.Close();
        }
        else if (save && !ProjectSettings.SaveSettings())
        {
           
            ProjectSettings.UnityPath = "";    

        }
        
        
       
    }
    void Update()
    {
        if (browse)
        {
            ProjectSettings.UnityPath = EditorUtility.OpenFilePanel("Select Unity.exe", "", "");
        }
    }
    void UpdateValidationVal()
    {
        err.UpdateValue("Path", ProjectSettings.UnityPath.Length);
    }
}
