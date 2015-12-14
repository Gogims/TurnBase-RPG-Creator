﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// Representa un navegador de menu (sin scroll)
/// </summary>
public class Navigator : AbstractNavigator{
    /// <summary>
    /// Lista de todas las opciones.
    /// </summary>
    private List<GameObject> Options;
    /// <summary>
    /// Funcion para inicializar los valores del selector
    /// </summary>
    /// <param name="arrow">Objeto que representa el selector</param>
    /// <param name="options">Objetos que van a ser navegados por el selector</param>
    public  override void Init(GameObject arrow, List<GameObject> options) {
        Options = new List<GameObject>();
        Arrow = arrow;
        Options = options;
        selected = 0;
    }
    /// <summary>
    /// Funcion para actualizar el estado del selector
    /// </summary>
    public override void update()
    {
        if (ProxyInput.GetInstance().AUp())
        {
            Options[selected].GetComponent<MenuOption>().OnSelect();
        }
        else if (ProxyInput.GetInstance().BUp())
        {
            delay = 0;
            Options[selected].GetComponent<MenuOption>().UnSelect();

            return;
        }
        else if (ProxyInput.GetInstance().DownUp())
        {
            if (selected == Options.Count - 1)
            {
                selected = 0;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            else
            {
                selected++;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().UpUp())
        {
            if (selected == 0)
            {
                selected = Options.Count - 1;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            else
            {
                selected--;
                Options[selected].GetComponent<MenuOption>().On(Options[selected].GetComponent<Text>().text);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].gameObject.transform.position.y);
        }

        delay = 0;
    }
}
