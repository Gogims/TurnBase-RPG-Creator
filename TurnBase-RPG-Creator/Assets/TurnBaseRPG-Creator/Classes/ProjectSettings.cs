using System.Collections;
using System.IO;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Clase que se encarga de las configuraciones del proyecto.
/// </summary>
public class ProjectSettings  {
    /// <summary>
    /// Path de unity.exe
    /// </summary>
    static public string UnityPath { get; set; }
    /// <summary>
    /// Archivo de configuracion
    /// </summary>
    const string path = "Assets/settings.txt";
    /// <summary>
    /// Constructor Estatico
    /// </summary>
    static  ProjectSettings()
    {
        LoadSettings();
        
    }
    /// <summary>
    /// Carga las configuraciones del proyecto
    /// </summary>
    static private void LoadSettings()
    {

        if (File.Exists(path))
        {
            string text = File.ReadAllText(path);
            var settings = text.Split('|');
            UnityPath = settings[0].Replace(System.Environment.NewLine, "");
        }
        else
        {
            var file = File.Create(path);
            file.Close();
        }
    }
    /// <summary>
    /// Guarda las configuraciones del proyecto.
    /// </summary>
    static public bool SaveSettings(string pathSave,string unitypath)
    {

        if (CheckPath(unitypath))
        {
            UnityEngine.Debug.Log(pathSave);
            string content = unitypath;
            File.WriteAllText(pathSave, content);
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// Guarda las configuraciones del proyecto.
    /// </summary>
    static public bool SaveSettings()
    {
        return SaveSettings(path,UnityPath);
    }
    /// <summary>
    /// Revisa si el path que se suministro es correcto.
    /// </summary>
    /// <returns></returns>
    static private bool CheckPath(string unitypath)
    {
        string[] unity = UnityPath.Split('/');
        string unityexe = (string)unity.GetValue(unity.Length - 1);
        if (unityexe == "Unity.exe" && File.Exists(UnityPath))
            return true;
        else
            return false;
    }
}
