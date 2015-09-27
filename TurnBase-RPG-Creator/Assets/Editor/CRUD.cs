using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CRUD : EditorWindow
{
    protected int Id;
    protected string Path { get { return _path; } }
    protected bool CreateButton;
    protected bool Creating;
    protected Object[] ListObjects;
    protected bool SaveButton; 

    private string _path;
    private string idPath;

    public CRUD(string p)
    {
        _path = Directory.GetCurrentDirectory() + p;
        idPath = _path + "id.txt";
    }

    public void GetId()
    {
        if (!File.Exists(idPath))
        {
            File.WriteAllText(idPath, "0");
            Id = 0;
        }
        else
        {
            Id = int.Parse(File.ReadAllText(idPath).Trim());
        }
    }

    public virtual void SetId()
    {
        File.WriteAllText(idPath, Id.ToString());
    }
}
