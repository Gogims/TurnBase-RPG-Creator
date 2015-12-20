﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Clase del menu principal.
/// </summary>
public class MainMenu : Menus {

    string selectName;
    public void Start() { 
        Object x = Resources.Load("Menus/FirstScene", typeof(GameObject));
        Object [] player = Resources.LoadAll("Player", typeof(GameObject));
        foreach (var i in player)
        {
            GameObject p = Instantiate(i as GameObject);
            p.name = "PLAYER";
            p.GetComponent<Player>().Data.HP = p.GetComponent<Player>().Data.Stats.MaxHP;
            p.GetComponent<Player>().Data.MP = p.GetComponent<Player>().Data.Stats.MaxMP;
            DontDestroyOnLoad(p);
            break;
        }
       
        DontDestroyOnLoad(GameObject.Find("MobileSingleStickControl"));
        DontDestroyOnLoad(GameObject.Find("EventSystem"));
        Constant.LastSceneLoaded = "MainMenu";
        GameObject.Find("SelectNewGame").GetComponent<MenuOptionScen>().SceneName = (x as GameObject).GetComponent<MenuOptionScen>().SceneName;
    }
    public void Update() {
        Menu.update();
    }
    public override void Select()
    {
        if (selectName == "Quit Game")
            Application.Quit();
    }
    public override void On(string name)
    {
        selectName = name;
    }
}
