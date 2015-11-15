using UnityEngine;
using System.Collections;
using UnityEditor;
public class ProjectUI : EditorWindow {
    Project newProject;
    bool browse;
    bool browsePath;
    ErrorHandler err;
    public void Init() {
        newProject = new Project();
        newProject.Name = "new_Project1";
        newProject.Description = "This is my first Project";
        newProject.Path = ""; 
        newProject.UnityPath = "";
        err = new ErrorHandler();
        err.InsertPropertyError("name", newProject.Name.Length, "The name of the project has to be greater than 5", new Rect(450, 30, 100, 20));
        err.InsertPropertyError("path", newProject.Path.Length, "You have to select the project path", new Rect(450, 140, 100, 20));
        err.InsertPropertyError("unity", newProject.UnityPath.Length, "You have to select unity path", new Rect(450, 90, 100, 20));
        err.InsertCondition("name", 5, ErrorCondition.Greater, LogicalOperators.None);
        err.InsertCondition("path", 0, ErrorCondition.Greater, LogicalOperators.None);
        err.InsertCondition("unity", 0, ErrorCondition.Greater, LogicalOperators.None);
    }
    void OnGUI() {
        if (newProject == null)
            Init();
        GUI.Label(new Rect(0,10,150,20),"Settings", EditorStyles.boldLabel);
        GUI.Label(new Rect(0, 30, 150, 20), "Project Name");
        newProject.Name = GUI.TextField(new Rect(150, 30, 300, 20), newProject.Name);
        GUI.Label(new Rect(0, 60, 150, 20), "Project Description");
        newProject.Description = GUI.TextField(new Rect(150, 60, 300, 20), newProject.Description);
        GUI.Label(new Rect(0, 90, 150, 20), "Unity Path");
        GUI.enabled = false;
        newProject.UnityPath = GUI.TextField(new Rect(150, 90, 300, 20), newProject.UnityPath);
        GUI.enabled = true;
        browsePath = GUI.Button(new Rect(150, 110, 150, 20), "Browse");
        GUI.Label(new Rect(0, 140, 150, 20), "Location");
        GUI.enabled = false;
        newProject.Path = GUI.TextField(new Rect(150, 140, 300, 20), newProject.Path);
        // Button to upload image
        GUI.enabled = true;
        browse = GUI.Button(new Rect(150, 160, 150, 20), "Browse");
        err.ShowErrors();
        UpdateValidationVal();
        if (GUI.Button(new Rect(0, 200, 300, 20), "Create") && !err.CheckErrors())
        {
            if (ValidateInput())
            {
                newProject.CreateProject();
                this.Close();
            }
            
        }
    }
    void UpdateValidationVal() {
        err.UpdateValue("name", newProject.Name.Length);
        err.UpdateValue("unity", newProject.UnityPath.Length);
        err.UpdateValue("path", newProject.Path.Length);
    }
    void Update()
    {
        if (browse)
        {
            newProject.Path = EditorUtility.OpenFolderPanel("Select Project","","");
        }
        if (browsePath)
        {
            newProject.UnityPath = EditorUtility.OpenFilePanel("Select Unity.exe", "", "");
        }
    }
    bool ValidateInput() {
        return true;
    }
    void onDestroy()
    {
        this.Close();
    }
}
