using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public abstract class CRUD : EditorWindow
{
    /// <summary>
    /// Maneja el ID que se encuentra almacenado en el txt del recurso
    /// </summary>
    protected int Id;
    /// <summary>
    /// Propiedad para devolver la variable _path, no se puede modificar
    /// </summary>
    protected string Path { get { return _path; } }    
    /// <summary>
    /// Se está creando un nuevo objeto en el formulario?
    /// </summary>
    protected bool Creating;
    /// <summary>
    /// Listado de los objetos que se han creado en el formulario
    /// </summary>
    protected Object[] ListObjects;
    /// <summary>
    /// Objeto que se utiliza para crear un prefab
    /// </summary>
    protected GameObject elementObject;
    /// <summary>
    /// Nombre de la imagen del objeto
    /// </summary>
    protected string spritename;

    /// <summary>
    /// El botón para salvar, del formulario fue presionado?
    /// </summary>
    protected bool SaveButton;
    /// <summary>
    /// El botón para crear, del formulario fue presionado?
    /// </summary>
    protected bool CreateButton;
    /// <summary>
    /// El botón para eliminar, del formulario fue presionado?
    /// </summary>
    protected bool DeleteButton;

    /// <summary>
    /// Dirección de la imagen del objeto
    /// </summary>
    private string _path;
    /// <summary>
    /// Dirección del txt que contiene el último ID
    /// </summary>
    private string idPath;

    public CRUD(string p)
    {
        _path = Directory.GetCurrentDirectory() + p;
        idPath = _path + "id.txt";
    }

    /// <summary>
    /// Obtiene el ID del txt
    /// </summary>
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

    /// <summary>
    /// Actualiza el ID del txt
    /// </summary>
    public virtual void SetId()
    {
        File.WriteAllText(idPath, Id.ToString());
    }

    public virtual void Create(){}

    public virtual void Edit(){}

    public virtual void Delete() { }

    public virtual void AddObject() { }


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
