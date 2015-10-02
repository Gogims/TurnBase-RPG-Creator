using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public abstract class CRUD : EditorWindow
{
    protected int Id;
    protected string Path { get { return _path; } }    
    protected bool Creating;
    protected Object[] ListObjects;
    protected GameObject elementObject;
    protected string spritename;

    protected bool SaveButton;
    protected bool CreateButton;
    protected bool DeleteButton;

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

    public virtual void Create(){}

    public virtual void Edit(){}

    public virtual void Delete() { }


    /// <summary>
    /// Revisa si no existe un game object instanceado, en caso de que sí, lo destruye antes de cerrar la ventana.
    /// </summary>
    public void OnDestroy()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject);
        }
    }
}
