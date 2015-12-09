using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Clase del menu principal.
/// </summary>
public class MainMenu : Menus {
    public void Start() { 
        Object x = Resources.Load("Menus/SelectNewGame", typeof(GameObject));

        GameObject.Find("SelectNewGame").GetComponent<MenuOptionScen>().SceneName = (x as GameObject).GetComponent<MenuOptionScen>().SceneName;
    }
    public void Update() {
        Menu.update();
    }
}
