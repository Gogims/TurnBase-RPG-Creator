using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.IO;
using UnityEditor;

/// <summary>
/// Clase que se encarga de manejar los proyectos de RPG
/// </summary>
public class Project  {
    public string Name { get; set; }
    public string UnityPath { get; set; }
    public string Description { get; set; }
    public string Path { get; set; }
    /// <summary>
    /// Crea un proyecto nuevo de RPG dado un path.
    /// </summary>
    public void CreateProject() {
        Process p = new Process();
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "cmd.exe";
        info.RedirectStandardInput = true;
        info.UseShellExecute = false;
        p.StartInfo = info;
        p.Start();
        Path = Path.Replace('/', '\\');
        string dirpath = Directory.GetCurrentDirectory().Replace("C:", "");
        string dirpath1 = @"""" + dirpath + "\\Assets\\TurnBaseRPG-Creator" + @"""";
        string dirpath2 = @""""+dirpath + "\\ProjectSettings"+@"""";
        string projectPath = @""""+Path + @"\" + Name+@"""";
        string projectAssets = Path + @"\" + Name + "\\Assets" ;
        string projectSettings = @""""+Path + @"\" + Name+"\\ProjectSettings"+@"""";
 
        using (StreamWriter sw = p.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine("");
                sw.WriteLine("mkdir "+projectPath);
                sw.WriteLine("mkdir " + projectSettings);
                sw.WriteLine("mkdir " + @"""" + projectAssets +"\\TurnBaseRPG-Creator"+@"""");
                sw.WriteLine("mkdir " + @"""" + projectAssets + "\\Sprites" + @"""");
                sw.WriteLine("mkdir " + @"""" + projectAssets + "\\Maps" + @"""");
                sw.WriteLine("mkdir " + @"""" + projectAssets + "\\Anmation" + @"""");
                sw.WriteLine("mkdir " + @"""" + projectAssets + "\\Resources" + @"""");
                sw.WriteLine("mkdir " + @"""" + projectAssets + "\\AnimationController" + @"""");
                sw.WriteLine("ROBOCOPY " + dirpath1 + " "+@""""+projectAssets + "\\TurnBaseRPG-Creator"+@""""+" *.* /E");
                sw.WriteLine("ROBOCOPY " + dirpath2 + " " + projectSettings+ " *.* /E");
                //Copia de la carpeta de rpg-resources a resources
                sw.WriteLine("ROBOCOPY "+@""""+projectAssets + "\\TurnBaseRPG-Creator\\RPG-Resources"+@""""+
                                         @" """+projectAssets + "\\Resources"+@""""+ " *.* /E");
                //Copia de la carpeta de rpg-Maps a maps
                sw.WriteLine("ROBOCOPY " + @"""" + projectAssets + "\\TurnBaseRPG-Creator\\RPG-Maps" + @"""" +
                           @" """ + projectAssets + "\\Maps" + @"""" + " *.* /E");
                //Copia de la carpeta de rpg-animation a animation
                sw.WriteLine("ROBOCOPY " + @"""" + projectAssets + "\\TurnBaseRPG-Creator\\RPG-Animation" + @"""" +
                           @" """ + projectAssets + "\\Animation" + @"""" + " *.* /E");
                //Copia de la carpeta de rpg-animation a animation
                sw.WriteLine("ROBOCOPY " + @"""" + projectAssets + "\\TurnBaseRPG-Creator\\RPG-Sprites" + @"""" +
                           @" """ + projectAssets + "\\Sprites" + @"""" + " *.* /E");
                //Copia de la carpeta de rpg-animationController a animationController
                sw.WriteLine("ROBOCOPY " + @"""" + projectAssets + "\\TurnBaseRPG-Creator\\RPG-AnimationController" + @"""" +
                           @" """ + projectAssets + "\\AnimationController" + @"""" + " *.* /E");
                sw.WriteLine(ProjectSettings.UnityPath.Replace('/','\\')+" -createProject "+projectPath);
            }
        }
        return;
    }
    /// <summary>
    /// Abre un proyecto
    /// </summary>
    /// <param name="path">path del proyecto</param>
    public static void Open(string path)
    {
        EditorApplication.OpenProject(path);
    }


}
