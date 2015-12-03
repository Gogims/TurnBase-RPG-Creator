using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuSelect : MonoBehaviour {

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
    /// Imagen que se va mostrar como selector del menu.
    /// </summary>
    public GameObject Arrow;
    
    public void Init(GameObject arrow, List<GameObject> options) {
        Options = new List<GameObject>();
        Arrow = arrow;
        Options = options;
    }
    public void update() {

        if (delay < 15)
        {
            delay++;
            return;
        }
        if (ProxyInput.GetInstance().B())
        {
            Options[selected].GetComponent<MenuOption>().UnSelect();
            return;
        }
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
                selected = Options.Count - 1;
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
