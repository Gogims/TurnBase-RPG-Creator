﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleMenu : Menus {
    /// <summary>
    /// Habilidad seleccionada
    /// </summary>
    AbstractAbility AbilitySelected;
    /// <summary>
    /// Nombre de la seleccion en el menu principal ( Attack,Run,Ability,Items)
    /// </summary>
    string MainSelection = string.Empty;
    /// <summary>
    /// Nombre de la opcion seleccionada.
    /// </summary>
    string SelectionName = string.Empty;
    /// <summary>
    /// Menu para el listado de item
    /// </summary>
    ScrollNavigator<AbstractUsable, AbstractAbility> ItemMenu;
    /// <summary>
    /// Menu de seleccion ( Use , Cancel)
    /// </summary>
    Navigator SelectionMenu;
    /// <summary>
    /// Indica cual es el menu que esta seleccionado
    /// </summary>
    private int MenuSelection = 0;
    /// <summary>
    /// Item seleccionado
    /// </summary>
    private AbstractUsable UsableSelected;

    /// <summary>
    /// Codigo de prueba
    /// </summary>
    public void Start() {
        Init(GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>());
    }
    /// <summary>
    /// player que va usar el menu.
    /// </summary>
    Player player;
    private GameObject Message;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    public void Init(Player p){
        player = p;
        UsableSelected = new AbstractUsable();
        ItemMenu = new ScrollNavigator<AbstractUsable, AbstractAbility>();
        SelectionName = "Attack";
        SelectionMenu = new Navigator();
        ItemMenu.Init(new Vector3(-76, 40), new Vector3(-33, 40),new Vector3(92,-11), -24, 0,4, GameObject.Find("List Panel").transform.FindChild("Arrow2").gameObject, GameObject.Find("List Panel").transform.FindChild("NextArrow").gameObject, GameObject.Find("List Panel").transform.FindChild("PrevArrow").gameObject, GameObject.Find("List Panel"));
        GameObject SelectPanel = GameObject.Find("Select Panel");
        Message = GameObject.Find("Message");
        List<GameObject> SelectItem = new List<GameObject>();
        GameObject.Find("List Panel").transform.FindChild("Quantity").gameObject.SetActive(false);
        for(int i =0;  i < SelectPanel.transform.childCount; i++)
        {
            GameObject ic = SelectPanel.transform.GetChild(i).gameObject;
            if (!ic.name.Contains("Arrow"))
                SelectItem.Add(ic);
               
        }
        SelectionMenu.Init(GameObject.Find("Arrow3"), SelectItem);
        ItemMenu.HideList();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="selected"></param>
    public override void On(Item selected)
    {
        UsableSelected = selected as AbstractUsable;
        switch (MenuSelection)
        {
            case 0:
                break;
            case 1:
                GameObject.Find("Description").GetComponent<Text>().text = "Attribute affected " + UsableSelected.Attribute.ToString() + " " + UsableSelected.Power.ToString()+", "+UsableSelected.AreaOfEffect.ToString()+"."+UsableSelected.Description;
                break;
                
            default:
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="selected"></param>
    public override void On(AbstractAbility selected)
    {
        AbilitySelected = selected;
        switch (MenuSelection)
        {
            case 0:
                break;
            case 1:
                GameObject.Find("Description").GetComponent<Text>().text = "MP Cost " + AbilitySelected.MPCost+", Power "+AbilitySelected.AttackPower+" "+AbilitySelected.Type.ToString() + "." + AbilitySelected.Description;
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Select()
    {
        Message.GetComponent<Text>().text = "";
        MenuSelection++;
        if (MenuSelection > 2)
            MenuSelection = 2;
        if (SelectionName == "Ability" || SelectionName == "Items")
        {
            ItemMenu.OnFirst();
        }
        else if (SelectionName == "Use" && MainSelection == "Items")
        {
            
            GameObject.Find("BattleManager").GetComponent<BattleManager>().UseItem(UsableSelected);
            MenuSelection = 0;
            ItemMenu.HideList();
        }
        else if (SelectionName == "Use" && MainSelection == "Ability")
        {
            if (player.GetComponent<Player>().Data.MP < AbilitySelected.MPCost)
            {
                Message.GetComponent<Text>().text = "NO MANA!!!!";
            }
            else
            {
                GameObject.Find("BattleManager").GetComponent<BattleManager>().UseAbility(AbilitySelected);
                MenuSelection = 0;
                ItemMenu.HideList();
            }
        }
        else if (SelectionName == "Cancel")
        {
            MenuSelection--;
        }
        else if (SelectionName == "Attack") 
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().Attack();
            MenuSelection = 0;
        }
        else if (SelectionName == "Run")
        {
            GameObject.Find("BattleManager").GetComponent<BattleManager>().Run();
            MenuSelection = 0;
        }
        if (MenuSelection == 2)
            SelectionName = "Use";
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public override void On(string name)
    {
        SelectionName = name;
        switch (MenuSelection)
        {
            case 0:
                MainSelection = name;
                if (name == "Items")
                {
                    GameObject.Find("List Panel").transform.FindChild("Quantity").gameObject.SetActive(true);
                    ItemMenu.HideList();
                    ItemMenu.setList(player.Items.GetUsables());
                    ItemMenu.DisplayList();
                }
                else if (name == "Attack")
                {
                    ItemMenu.HideList();
                }
                else if (name == "Ability")
                {
                    GameObject.Find("List Panel").transform.FindChild("Quantity").gameObject.SetActive(false);
                    ItemMenu.HideList();
                    ItemMenu.setList(player.Data.Job.Abilities);
                    ItemMenu.DisplayList();
                }
                else if (name == "Run")
                {
                    ItemMenu.HideList();
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public override void unSelect()
    {
        if (SelectionName != "Use")
            GameObject.Find("Description").GetComponent<Text>().text = "";
        MenuSelection--;
        if (MenuSelection < 0)
            MenuSelection = 0;
    }
    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        switch (MenuSelection)
        {
            case 0:
                Menu.update();
                break;
            case 1:
                ItemMenu.update();
                break;
            case 2:
                SelectionMenu.update();
                break;
            default:
                break;
        } 
    }

}
