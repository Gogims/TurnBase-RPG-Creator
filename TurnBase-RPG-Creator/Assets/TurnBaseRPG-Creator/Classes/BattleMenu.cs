using UnityEngine;
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
        GameObject.Find("BattleMenu").transform.FindChild("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Init(GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>());
    }
    /// <summary>
    /// player que va usar el menu.
    /// </summary>
    Player player;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    public void Init(Player p){
        player = p;
        UsableSelected = new AbstractUsable();
        ItemMenu = new ScrollNavigator<AbstractUsable, AbstractAbility>();
        SelectionMenu = new Navigator();
        ItemMenu.Init(new Vector3(-74, -3), new Vector3(-40, 35),new Vector3(92,-11), -24, 0,4, GameObject.Find("List Panel").transform.FindChild("Arrow2").gameObject, GameObject.Find("List Panel").transform.FindChild("NextArrow").gameObject, GameObject.Find("List Panel").transform.FindChild("PrevArrow").gameObject, GameObject.Find("List Panel"));
        GameObject SelectPanel = GameObject.Find("Select Panel");
        GameObject Arrow3 = new GameObject();
        List<GameObject> SelectItem = new List<GameObject>();
        for(int i =0;  i < SelectPanel.transform.childCount; i++)
        {
            GameObject ic = SelectPanel.transform.GetChild(i).gameObject;
            if (!ic.name.Contains("Arrow"))
                SelectItem.Add(ic);
            else
                Arrow3 = ic;
        }
        SelectionMenu.Init(Arrow3, SelectItem);
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
                GameObject.Find("Description").GetComponent<Text>().text = UsableSelected.Description + "." + UsableSelected.AreaOfEffect.ToString() + "," + UsableSelected.Attribute.ToString();
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
                GameObject.Find("Description").GetComponent<Text>().text = AbilitySelected.Description + "." + AbilitySelected.AreaOfEffect.ToString();
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
        MenuSelection++;
        if (MenuSelection > 2)
            MenuSelection = 2;
        if (SelectionName == "Use" && MainSelection == "Items")
        {
            //GameObject.FindWithTag("RPG-BM").GetComponent<BattleManager>().UseItem(UsableSelected);
            MenuSelection = 0;
            ItemMenu.HideList();
        }
        if (SelectionName == "Use" && MainSelection == "Ability")
        {
           // GameObject.FindWithTag("RPG-BM").GetComponent<BattleManager>().UseAbility(AbilitySelected);
            MenuSelection = 0;
            ItemMenu.HideList();
        }
        else if (SelectionName == "Cancel")
        {
            MenuSelection--;
        }
        else if (SelectionName == "Attack") 
        {
           // GameObject.FindWithTag("RPG-BM").GetComponent<BattleManager>().Attack();
            MenuSelection = 0;
        }
        else if (SelectionName == "Run")
        {
            //GameObject.FindWithTag("RPG-BM").GetComponent<BattleManager>().Run();
            MenuSelection = 0;
        }
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
