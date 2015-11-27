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
    public GameObject MenuName;
    /// <summary>
    /// Imagen que se va mostrar en el background del menu
    /// </summary>
    public GameObject BackgroundImage;
    /// <summary>
    /// Imagen que se va mostrar como selector del menu.
    /// </summary>
    public GameObject Arrow;
    /// <summary>
    /// Estilo de las letras del menu.
    /// </summary>
    public GUIStyle Style;
    /// <summary>
    /// Indice de la opcion seleccionada.
    /// </summary>
    protected int selected = 0;
    /// <summary>
    /// Lista de todas las opciones.
    /// </summary>
    protected List<GameObject> Options;
    /// <summary>
    /// Delay para el cambio de una opcion a otra.
    /// </summary>
    protected int delay;
    /// <summary>
    /// Indica que no se mueva el cursor de seleccion.
    /// </summary>
    protected  bool select = true;
    /// <summary>
    /// Funcion que se llama cuando se inicia la scene.
    /// </summary>
    public void Awake() {
        //Arrow.GetComponent<Image>().sprite = ProjectSettings.SelectImage;
        GameObject menusObj = GameObject.Find("Canvas");
        Options = new List<GameObject>();
        for (int i = 0; i < menusObj.transform.childCount; i++)
        {
            GameObject obj = menusObj.transform.GetChild(i).gameObject;
            if (obj.name.Contains("Select"))
            {
                Options.Add(obj);
            }
        }
    }
    /// <summary>
    /// Funcion que se llama cada frame.
    /// </summary>
    void OnGUI() {

        if (delay < 15){
            delay++;
            return;
        }
        if (ProxyInput.GetInstance().B())
        {
            Options[selected].GetComponent<MenuOption>().UnSelect();
            return;
        }
        if (!select)
            return;
        if (ProxyInput.GetInstance().Down())
        {
            if (selected == Options.Count - 1)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected = 0;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            else
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected++;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().Up()) 
        {
            if (selected == 0)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected = Options.Count-1;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            else
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected--;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().A())
        {
            Options[selected].GetComponent<MenuOption>().OnSelect();
        }

        delay = 0;
    }
}