using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

/// <summary>
/// Clase encargada de hacer las operaciones de create, update, edit y delete de los objetos del Engine
/// </summary>
/// <typeparam name="T">Clase que se desea hacer CRUD</typeparam>
public abstract class CRUD<T> : EditorWindow
    where T : RPGElement
{
    /// <summary>
    /// El formulario está en modo selección
    /// </summary>
    public bool Selected;

    /// <summary>
    /// Maneja el ID que se encuentra almacenado en el txt del recurso
    /// </summary>
    protected int Id;
    /// <summary>
    /// Elemento que se utilizará en el CRUD
    /// </summary>
    protected T element;
    /// <summary>
    /// Elemento que se seleccionará para el CRUD de un objeto más complejo
    /// </summary>
    protected T AssignedElement;
    /// <summary>
    /// List de elementos del CRUD
    /// </summary>
    protected ListBox listElements;
    /// <summary>
    /// Nombre del folder del recurso y la clase (Armor, Weapon, etc.)
    /// </summary>
    protected string Type;
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
    /// El botón para seleccionar un elemento del formulario
    /// </summary>
    protected bool SelectButton; 
    /// <summary>
    /// Dirección absoluta del objeto
    /// </summary>
    private string _path;
    /// <summary>
    /// Dirección relativa del objeto
    /// </summary>
    private string relativepath;
    /// <summary>
    /// Dirección del txt que contiene el último ID
    /// </summary>
    private string idPath;
    /// <summary>
    /// Size of the listing of the left size
    /// </summary>
    private Rect LeftSide;

    /// <summary>
    /// Unico constructor de la clase CRUD
    /// </summary>
    /// <param name="type">Nombre del folder del recurso y la clase (debe ser el mismo). Ejemplo: Armor, Weapon, etc.</param>
    protected CRUD(string type, Rect left)
    {
        Type = type;
        relativepath = "Assets/Resources/" + type + "/";
        _path = Directory.GetCurrentDirectory() + @"\Assets\Resources\" + type + @"\";
        LeftSide = left;       
        
        idPath = _path + "id.txt";
    }

    /// <summary>
    /// Initializa el formulario, se debe llamar en el Init de la clase concreta
    /// </summary>
    public virtual void Init()
    {
        elementObject = NewGameObject();
        ListObjects = (Resources.LoadAll(Type, typeof(GameObject)));
        Creating = true;
        listElements = new ListBox(new Rect(LeftSide.x, LeftSide.y, LeftSide.width, LeftSide.height-20), new Rect(LeftSide.x, LeftSide.y, LeftSide.width-15, LeftSide.height), false, true);
        spritename = "";
        GetId();
    }

    /// <summary>
    /// Obtiene el listado de objetos de tipo T
    /// </summary>
    /// <returns>Lista de los prefabs de tipo T</returns>
    protected IEnumerable<T> GetObjects()
    {
        foreach (var item in ListObjects)
        {
            GameObject current = (GameObject)Instantiate(item);
            T temp = current.GetComponent<T>();            
            DestroyImmediate(current);
            yield return temp;
        }
    }

    /// <summary>
    /// Obtiene el ID del txt
    /// </summary>
    protected void GetId()
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
    protected virtual void SetId()
    {
        File.WriteAllText(idPath, Id.ToString());
    }

    /// <summary>
    /// Crea un nuevo prefab de tipo T
    /// </summary>
    protected virtual void Create()
    {
        Id++;
        element.Id = Id;
        CreatePrefab(element);
        listElements.AddItem(element.Name, element.Id);        
        SetId();
    }

    /// <summary>
    /// Edita un prefab existente de tipo T
    /// </summary>
    protected virtual void Edit()
    {
        CreatePrefab(element);
        listElements.ChangeName(listElements.GetSelectedIndex(), element.Name);
    }

    /// <summary>
    /// Elimina un prefab existente de tipo T
    /// </summary>
    protected virtual void Delete()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject, true);
            AssetDatabase.DeleteAsset(relativepath + element.Id + ".prefab");
            listElements.RemoveItemIndex(listElements.GetSelectedIndex());
            Init();
        }
    }    

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

    /// <summary>
    /// Revisa si el objeto a guardar es nuevo o no y llama a su respectiva función
    /// </summary>
    protected void SaveElement()
    {
        if (Creating)
        {
            Create();
        }
        else
        {
            Edit();
        }

        ListObjects = (Resources.LoadAll(Type, typeof(GameObject)));
    }

    #region Funciones Unity
    /// <summary>
    /// Renderiza el listado de objetos, en la parte izquierda del formulario
    /// </summary>
    protected void RenderLeftSide()
    {
        //Left side area
        GUILayout.BeginArea(LeftSide, string.Empty, EditorStyles.helpBox);
        GUILayout.Space(10);

        if (listElements.ReDraw())
        {
            UpdateListBox();
        }

        if (!Selected)
        {
            CreateButton = GUI.Button(new Rect(LeftSide.x, LeftSide.height-20, 100, 20), "Create"); 
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// Llama la función de actualizar del Unity
    /// </summary>
    void Update()
    {
        //Si se crea un nuevo elemento
        if (CreateButton)
        {
            elementObject = NewGameObject();
            Creating = true;
        }

        //Funcionamineto de guardado
        if (SaveButton)
        {
            SaveElement();
            elementObject = NewGameObject();
        }

        //Si se elimina un elemento
        if (DeleteButton)
        {
            Delete();
            ListObjects = (Resources.LoadAll(Type, typeof(GameObject)));
            elementObject = NewGameObject();
        }

        if (SelectButton)
        {
            AssignElement();
            this.Close();
        }
    }
    #endregion

    /// <summary>
    /// Crea un prefab y destruye el gameobject de la escena.
    /// </summary>
    /// <param name="ElementObject">Game object que se encuentra vivo en la escena</param>
    /// <param name="component">Componente que se desea guardar como prefab</param>
    protected void CreatePrefab(T component)
    {
        PrefabUtility.CreatePrefab(relativepath + component.Id + ".prefab", component.gameObject);
        DestroyImmediate(component.gameObject);
    }

    /// <summary>
    /// Actualiza el listado de objetos
    /// </summary>
    protected virtual void UpdateListBox()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject);
        }

        elementObject = (GameObject)Instantiate(ListObjects[listElements.GetSelectedIndex()]);
        element = elementObject.GetComponent<T>();        
        Creating = false;
    }

    protected virtual GameObject NewGameObject()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject);
        }

        GameObject newGameObject = new GameObject(Type);
        element = newGameObject.AddComponent<T>();

        return newGameObject;
    }

    protected virtual void AssignElement() { }
}
