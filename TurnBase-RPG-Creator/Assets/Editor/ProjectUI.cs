using UnityEngine;
using System.Collections;
using UnityEditor;
public class ProjectUI : EditorWindow {
    Project newProject;
    bool browse;
    public void Init() {
        newProject = new Project();
        newProject.ProjectName = "new_Project1";
        newProject.Description = "This is my first Project";
        newProject.Path = "";
    }
    void OnGUI() {
        GUI.Label(new Rect(0,10,150,20),"Settings", EditorStyles.boldLabel);

        GUI.Label(new Rect(0, 30, 150, 20), "Project Name");
        if (newProject.ProjectName.Length == 0)
            newProject.ProjectName= GUI.TextField(new Rect(150,30,300,20),"new_Project1");
        else
            newProject.ProjectName = GUI.TextField(new Rect(150, 30, 300, 20), newProject.ProjectName);
        GUI.Label(new Rect(0, 60, 150, 20), "Project Description");
        newProject.Description = GUI.TextField(new Rect(150, 60, 300, 20), newProject.Description);
        GUI.Label(new Rect(0, 90, 150, 20), "Location");
        GUI.enabled = false;
        newProject.Path = GUI.TextField(new Rect(150, 90, 300, 20), newProject.Path);
        // Button to upload image
        GUI.enabled = true;
        browse = GUI.Button(new Rect(150, 110, 150, 20), "Browse");
        if (GUI.Button(new Rect(0, 150, 300, 20), "Create"))
        {
            newProject.CreateProject();
        }
    }
    void Update()
    {
        if (browse)
        {
            newProject.Path = EditorUtility.OpenFolderPanel("Select Project","","");
        }
    }
    void onDestroy()
    {
        this.Close();
    }
}
