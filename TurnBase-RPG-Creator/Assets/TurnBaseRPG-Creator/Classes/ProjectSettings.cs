using System.Collections;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using System;
using UnityEditor;

/// <summary>
/// Clase que se encarga de las configuraciones del proyecto.
/// </summary>
public class ProjectSettings:MonoBehaviour  {
    /// <summary>
    /// Imagen que se va usar en los menus para seleccionar las opciones
    /// </summary>
    public static Sprite SelectImage; 
    /// <summary>
    /// El nombre del proyecto.
    /// </summary>
    public static string ProjectName = "Second Game";
    /// <summary>
    /// Path de unity.exe
    /// </summary>
    public static string UnityPath = EditorApplication.applicationPath;
    /// <summary>
    /// Scala con la que se va trabajar el proyecto.
    /// </summary>
    static public int pixelPerUnit = 1;
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

        //if (File.Exists(path))
        //{
        //    try {
        //    string text = File.ReadAllText(path);
        //    var settings = text.Split('|');
        //    }
        //    catch{
        //    }
        //}
        //else
        //{
        //    var file = File.Create(path);
        //    file.Close();
        //}
    }
    /// <summary>
    /// Guarda las configuraciones del proyecto.
    /// </summary>
    static public bool SaveSettings()
    {
            //string content = "";
            //File.WriteAllText(path, content);
            //return true;
        return true;
    }
}
