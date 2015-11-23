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
    /// Elemento que se utilizará en el CRUD
    /// </summary>
    protected T element;
    /// <summary>
    /// Elemento que se seleccionará para el CRUD de un objeto más complejo
    /// </summary>
    protected T AssignedElement;
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
    /// Variable que se encarga del manejo de errores
    /// </summary>
    protected ErrorHandler err = new ErrorHandler();
    /// <summary>
    /// Nombre del sprite del formulario
    /// </summary>
    //protected string SpriteName = string.Empty;

    /// <summary>
    /// Dirección absoluta del objeto
    /// </summary>
    private string _path;
    /// <summary>
    /// Dirección relativa del objeto
    /// </summary>
    private string relativepath;
    /// <summary>
    /// Size of the listing of the left size
    /// </summary>
    private Rect LeftSide;
    /// <summary>
    /// Variable para el scroll del listado
    /// </summary>
    private Vector2 ScrollPosition;    

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
        err = new ErrorHandler();
    }

    /// <summary>
    /// Initializa el formulario, se debe llamar en el Init de la clase concreta
    /// </summary>
    public virtual void Init()
    {
        elementObject = NewGameObject();
        ListObjects = (Resources.LoadAll(Type, typeof(GameObject)));
        InitErrors();
        Creating = true;
    }

    /// <summary>
    /// Crea un nuevo prefab de tipo T
    /// </summary>
    protected virtual void Create()
    {
        element.Id = System.Guid.NewGuid().ToString();
        CreatePrefab(element);
    }

    /// <summary>
    /// Edita un prefab existente de tipo T
    /// </summary>
    protected virtual void Edit()
    {
        CreatePrefab(element);
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
        GUILayout.BeginArea(new Rect(LeftSide.x, LeftSide.y, LeftSide.width, LeftSide.height-20), string.Empty, EditorStyles.helpBox);
        GUILayout.Space(10);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        DrawObjectList();
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        if (!Selected)
        {
            CreateButton = GUI.Button(new Rect(LeftSide.x, LeftSide.height - 20, 100, 20), "Create");
        }        
    }

    /// <summary>
    /// Dibuja los objetos que estan en el arreglo
    /// </summary>
    void DrawObjectList()
    {
        int x = 10;
        int y = 26;
        foreach (var obj in ListObjects)
        {
            GameObject temp = (GameObject)obj;            
            T comp = temp.GetComponent<T>();
            Rect position = new Rect(x, y, 64, 64);

            if (FilterList(comp))
            {
                if (comp.Icon != null)
                {
                    GUI.DrawTextureWithTexCoords(position, comp.Icon.texture, Constant.GetTextureCoordinate(comp.Icon));
                }

                if (comp.Name.Length > 8)
                    GUI.Label(new Rect(x, y + 74, 64, 20), comp.Name.Substring(0, 6) + "...");
                else
                    GUI.Label(new Rect(x, y + 74, 64, 20), comp.Name);

                if (GUI.Button(position, "", new GUIStyle()))
                {
                    if (elementObject != null)
                    {
                        DestroyImmediate(elementObject);
                    }

                    elementObject = Instantiate(temp);
                    element = elementObject.GetComponent<T>();
                    Creating = false;

                    //if (element.Icon != null)
                    //{
                    //    SpriteName = element.Icon.name;
                    //}
                }

                if (x + 84 + 64 < LeftSide.width)
                    x += 84;
                else
                {
                    GUILayout.Label("", GUILayout.Height(64 + 25), GUILayout.Width(x + 60));
                    y += 94;
                    x = 10;
                } 
            }
        }
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
        if (SaveButton && !err.CheckErrors())
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

        UpdateForm();
        UpdateValidations();
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

    protected virtual GameObject NewGameObject()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject);
        }

        //SpriteName = string.Empty;
        GameObject newGameObject = new GameObject(Type);
        element = newGameObject.AddComponent<T>();

        return newGameObject;
    }

    protected void AddObject()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            element.Icon = (Sprite)EditorGUIUtility.GetObjectPickerObject();            
            Repaint();
        }
    }

    protected virtual void AssignElement() { }

    protected virtual void UpdateForm() { }
    protected virtual void InitErrors() { }
    protected virtual void UpdateValidations() { }
    protected virtual bool FilterList(T component) { return true; }
}
