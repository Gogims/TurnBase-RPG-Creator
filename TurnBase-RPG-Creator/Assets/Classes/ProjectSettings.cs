using System.Collections;
using System.IO;
using UnityEngine;
using System.Diagnostics;

public class ProjectSettings  {
    static public string UnityPath { get; set; }
    static private string unitypath; 
    const string path = @"Assets/settings.txt";
    static  ProjectSettings()
    {
        LoadSettings();
    }
    static private void LoadSettings()
    {
        
        if (File.Exists(path))
        {
            string text = File.ReadAllText(path);
            var settings = text.Split('|');
            UnityPath = settings[0];
        }
        else
            File.Create(path);
    }
    static public void SaveSettings()
    {
        string content = UnityPath;
        File.WriteAllText(path,content);
    }
    static public bool CheckPath() {
        if ( File.Exists(UnityPath))
            return true;
        else 
            return false;
    }
}
