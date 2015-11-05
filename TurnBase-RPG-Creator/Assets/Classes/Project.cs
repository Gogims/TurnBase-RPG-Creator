using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.IO;

/// <summary>
/// Clase que se encarga de manejar los proyectos de RPG
/// </summary>
public class Project  {
    public string ProjectName { get; set; }
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
        string dirpath1 = @""""+dirpath + "\\Assets"+@"""";
        string dirpath2 = @""""+dirpath + "\\ProjectSettings"+@"""";
        string projectPath = @""""+Path + @"\" + ProjectName+@"""";
        string projectAssets= @""""+Path + @"\" + ProjectName+"\\Assets"+@"""";
        string projectSettings = @""""+Path + @"\" + ProjectName+"\\ProjectSettings"+@"""";
        using (StreamWriter sw = p.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine("");
                sw.WriteLine("mkdir "+projectPath);
                sw.WriteLine("mkdir " + projectSettings);
                sw.WriteLine("mkdir " + projectAssets);
                sw.WriteLine("ROBOCOPY "+dirpath1+ " "+projectAssets+" *.* /E");
                sw.WriteLine("ROBOCOPY " + dirpath2 + " " + projectSettings+ " *.* /E");
                sw.WriteLine(ProjectSettings.UnityPath.Replace('/','\\')+" -createProject "+projectPath);
            }
        }
        return;
    }
    /// <summary>
    /// Abre un proyecto
    /// </summary>
    /// <param name="path"></param>
    public static void Open(string path)
    {
        Process p = new Process();
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = "cmd.exe";
        info.RedirectStandardInput = true;
        info.UseShellExecute = false;
        p.StartInfo = info;
        p.Start();
        path = @""""+path.Replace('/', '\\')+@"""";
        UnityEngine.Debug.Log(path);
        using (StreamWriter sw = p.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine("");
                sw.WriteLine(ProjectSettings.UnityPath.Replace('/', '\\') + " -projectPath  " + path);
            }
        }   
    }
}
