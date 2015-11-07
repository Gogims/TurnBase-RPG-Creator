using UnityEngine;
using System.Collections;
using UnityEditor;
public class ProjectUI : EditorWindow {
    Project newProject;
    bool browse;
    bool browsePath;
    public void Init() {
        newProject = new Project();
        newProject.ProjectName = "new_Project1";
        newProject.Description = "This is my first Project";
        newProject.Path = ""; 
        newProject.UnityPath = ""; 
    }
    void OnGUI() {
        if (newProject == null)
            Init();
        GUI.Label(new Rect(0,10,150,20),"Settings", EditorStyles.boldLabel);
        GUI.Label(new Rect(0, 30, 150, 20), "Project Name");
        newProject.ProjectName = GUI.TextField(new Rect(150, 30, 300, 20), newProject.ProjectName);
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
        if (GUI.Button(new Rect(0, 200, 300, 20), "Create"))
        {
            if (ValidateInput())
            {
                newProject.CreateProject();
                this.Close();
            }
            
        }
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
