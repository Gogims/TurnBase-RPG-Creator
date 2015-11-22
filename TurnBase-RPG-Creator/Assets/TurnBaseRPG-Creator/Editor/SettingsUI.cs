using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class SettingsUI : EditorWindow {

    //bool browse = false;
    //bool save = false;
    //int scale;
    //ErrorHandler err;
	public void Init() {
     
	}
    void OnGUI() {
        //if (err == null)
        //    Init();
        GUILayout.Label("", GUILayout.Height(30));
        GUI.Label(new Rect(0, 10, 100, 20), "Scale");
        // scale = EditorGUI.Popup(new Rect(110, 10, this.position.width - 115, 10), scale, Scales);
        //save = GUILayout.Button("Save",GUILayout.Width(100)) ;
        //ProjectSettings.Scale = Int32.Parse(Scales[scale]);     
        //if (save && ProjectSettings.SaveSettings())
        //{
        //   this.Close();
        //}
        //else if (save && !ProjectSettings.SaveSettings())
        //{
        //    //...
        //}
        
        
       
    }
    void UpdateValidationVal()
    {
        //err.UpdateValue("Path", ProjectSettings.UnityPath.Length);
    }
}
