using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
/// <summary>
/// Clase abstracta de menu.
/// </summary>
public abstract class  Menus : MonoBehaviour {
    /// <summary>
    /// Nombre del menu
    /// </summary>
    public string MenuName;
    /// <summary>
    /// Imagen que se va mostrar en el background del menu
    /// </summary>
    public GameObject BackgroundImage;
    /// <summary>
    /// Imagen que se va mostrar como selector del menu.
    /// </summary>
    public GameObject Arrow;
    /// <summary>
    /// Menu principal.
    /// </summary>
    protected Navigator Menu;
    public bool disable; 
    /// <summary>
    /// Funcion que se llama cuando se inicia la scene.
    /// </summary>
    public void Awake() {
        GameObject menuC = new GameObject();
        Menu = menuC.AddComponent<Navigator>();
        Destroy(menuC);
        GameObject menusObj =null;
        if (MenuName == "BattleMenu")
        {
            menusObj = GameObject.Find("BattleMenu").transform.FindChild("Canvas").transform.FindChild("Menu Panel").gameObject;
        }
        else if (Constant.LastSceneLoaded == null)
             menusObj = GameObject.Find("Canvas").transform.FindChild("Menu Panel").gameObject;
        else 
             menusObj = GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").transform.FindChild("Menu Panel").gameObject;
        List<GameObject> Options = new List<GameObject>();
        for (int i = 0; i < menusObj.transform.childCount; i++)
        {
            GameObject obj = menusObj.transform.GetChild(i).gameObject;
            if (obj.name.Contains("Select"))
            {
                Options.Add(obj);
            }
        }
        Menu.Init(this.Arrow, Options);
    }
    public virtual void Select()
    {
    }
    public virtual void  unSelect(){
    }

    public virtual void On(Item selected)
    {
    }
    public virtual void On(AbstractAbility selected)
    {
    }
    public virtual void On(string name)
    {
       
    }
}