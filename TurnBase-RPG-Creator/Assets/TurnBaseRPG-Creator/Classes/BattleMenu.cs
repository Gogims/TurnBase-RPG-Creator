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
        p.Items = new Inventory();
        AbstractArmor aux = (Resources.Load("Armor/2fde446b-dd6e-459f-8ff8-47082c952525") as GameObject).GetComponent<Armor>().Data;
        AbstractWeapon aux2 = (Resources.Load("Weapon/dfaa859c-5ae4-44b4-9882-6ced915fd665") as GameObject).GetComponent<Weapon>().Data;
        AbstractUsable aux3 = (Resources.Load("Item/52a2fc86-91e8-44e8-98c7-203be647b54b") as GameObject).GetComponent<Usable>().Data;
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertArmor(aux);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        p.Items.InsertUsable(aux3);
        AbstractWeapon copy = new AbstractWeapon();
        copy.ItemName = "Weapon1";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Primera arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon2";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Segunda arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon3";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Tercera arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon4";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Cuarta arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon5";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Quinta arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon6";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Sexta arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon7";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Septima arama.";
         p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon8";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Octava arama.";
        p.Items.InsertWeapon(copy);
        copy = new AbstractWeapon();
        copy.ItemName = "Weapon9";
        copy.Image = aux2.Image;
        copy.Description = "Esta es la Novena arama.";
        p.Items.InsertWeapon(copy);
        player = p;
        UsableSelected = new AbstractUsable();
        ItemMenu = new ScrollNavigator<AbstractUsable, AbstractAbility>();
        SelectionMenu = new Navigator();
        ItemMenu.Init(new Vector3(-4, 25), new Vector3(82, 64),new Vector3(168,21), -30, 0, 5, GameObject.Find("List Panel").transform.FindChild("Arrow2").gameObject, GameObject.Find("List Panel").transform.FindChild("NextArrow").gameObject, GameObject.Find("List Panel").transform.FindChild("PrevArrow").gameObject, GameObject.Find("List Panel"));
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
        player.Data.Job = new AbstractJob();
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
