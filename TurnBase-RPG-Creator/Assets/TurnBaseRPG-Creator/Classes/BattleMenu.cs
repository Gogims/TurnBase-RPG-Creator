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
        GameObject battle = GameObject.Find("BattleMenu");
        battle.transform.parent = GameObject.Find("BattleMap").transform;
        battle.transform.localPosition = new Vector3(0, 0, 90);
        battle.transform.localScale = new Vector3(1, 1);
        GameObject.Find("BattleMenu").transform.FindChild("Canvas").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        RectTransform canvas = GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").gameObject.GetComponent<RectTransform>();
        Vector2 worldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);
        canvas.sizeDelta = new Vector2(worldScreen.x, worldScreen.y);
        Init(GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>());
        GameObject.Find("BattleMap").transform.FindChild("BattleManager").gameObject.GetComponent<BattleManager>().BattleMenu = battle;
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
