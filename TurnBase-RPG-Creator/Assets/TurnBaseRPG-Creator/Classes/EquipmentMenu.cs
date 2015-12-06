using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentMenu : Menus {
    /// <summary>
    /// Descripcion del item seleccionado
    /// </summary>
    public GameObject Description;
    /// <summary>
    /// Equipamento de la cabeza del player
    /// </summary>
    public GameObject Head;
    /// <summary>
    /// Equipamento del cuello del player
    /// </summary>
    public GameObject Necklace;
    /// <summary>
    /// Equipamento del cuerpo del player
    /// </summary>
    public GameObject Body;
    /// <summary>
    /// Equipamento del arma del player
    /// </summary>
    public GameObject Weapon;
    /// <summary>
    /// Equipamento del anillo del player
    /// </summary>
    public GameObject Ring;
    /// <summary>
    /// Equipamento de los pies del player
    /// </summary>
    public GameObject Feet;
    /// <summary>
    /// Panel donde se colocan los items
    /// </summary>
    public GameObject ItemPanel;
    /// <summary>
    /// Listado de armaduras.
    /// </summary>
    private List<AbstractArmor> ListHelmet;
    /// <summary>
    /// Listado de armaduras.
    /// </summary>
    private List<AbstractArmor> ListBody;
    /// <summary>
    /// Listado de armaduras.
    /// </summary>
    private List<AbstractArmor> ListFeet;
    /// <summary>
    /// Listado de armaduras.
    /// </summary>
    private List<AbstractArmor> ListNecklace;
    /// <summary>
    /// Listado de armaduras.
    /// </summary>
    private List<AbstractArmor> ListRing;
    /// <summary>
    /// Listado de armas;
    /// </summary>
    private List<AbstractWeapon> ListWeapon;
    /// <summary>
    /// Prefab del elemento que se coloca como item
    /// </summary>
    private GameObject Item;
    /// <summary>
    /// Prefab de la imagen del item
    /// </summary>
    private GameObject ItemImage;
    /// <summary>
    /// Posicion de y donde se coloco el ultimo item
    /// </summary>
    private int lastY = 97;
    /// <summary>
    /// La posicion en x de la imagen del item
    /// </summary>
    private int xImage = 66;
    /// <summary>
    /// Posicion en x del texto del item
    /// </summary>
    private int xItem = -17;
    /// <summary>
    /// Diferencia en y entre cada item;
    /// </summary>
    private int diffy = -39;
    /// <summary>
    /// Seleccion del menu
    /// </summary>
    private string Itemselection = "Weapon";
    private string selectionName = "Weapon";
    /// <summary>
    /// Lista de items visibles
    /// </summary>
    private List<Item> ActiveItems;
    /// <summary>
    /// Selector de items
    /// </summary>
    public GameObject Arrow2;
    /// <summary>
    /// Flecha que indica que hay  mas elementos en el panel de los items
    /// </summary>
    public GameObject NextArrow;
    /// <summary>
    /// Flecha que indica que hay mas elementos hacia arriba en el panel de los items
    /// </summary>
    public GameObject PrevArrow;
    /// <summary>
    /// Menu de scroll de los items
    /// </summary>
    private ScrollNavigator<Item> MenuScroll;
    /// <summary>
    /// Jugador en la scene.
    /// </summary>
    private Player player;
    /// <summary>
    /// Indica el menu seleccionado ( 0 main menu, 1 item menu , 2 select menu)
    /// </summary>
    private int MenuSelected = 0;
    /// <summary>
    /// Menu para equipar.
    /// </summary>
    private Navigator Menu2;
    /// <summary>
    /// El selector del menu de equipaje
    /// </summary>
    public GameObject Arrow3;
    /// <summary>
    /// Item seleccionado
    /// </summary>
    private Equippable EquipSelect;
    /// <summary>
    /// Stats del player
    /// </summary>
    #region Stats
    public GameObject MaxHP;
    public GameObject MaxMP;
    public GameObject Attack;
    public GameObject Magic;
    public GameObject Defense;
    public GameObject MagicDefense;
    public GameObject Agility;
    public GameObject Luck;
    private bool select2 = false;
    #endregion
    public void Start() {
        ClearDiffText();
        player = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>();
        SetPlayerLabels();
        ListBody = new List<AbstractArmor>();
        ListFeet = new List<AbstractArmor>();
        ListHelmet = new List<AbstractArmor>();
        ListNecklace = new List<AbstractArmor>();
        ListRing = new List<AbstractArmor>();
        ListWeapon = new List<AbstractWeapon>();
        ActiveItems = new List<Item>();
        ListBody = player.Items.TypeArmor(AbstractArmor.ArmorType.Body);
        ListFeet = player.Items.TypeArmor(AbstractArmor.ArmorType.Feet);
        ListHelmet = player.Items.TypeArmor(AbstractArmor.ArmorType.Helmet);
        ListNecklace = player.Items.TypeArmor(AbstractArmor.ArmorType.Necklace);
        ListRing = player.Items.TypeArmor(AbstractArmor.ArmorType.Ring);
        ListWeapon = player.Items.Weapons;
        Item = Resources.Load("Menus/MenuItems") as GameObject;
        ItemImage = Resources.Load("Menus/MenuItemsImage") as GameObject;
        MenuScroll = new ScrollNavigator<Item>();
        MenuScroll.Init(new Vector3(xItem, lastY), new Vector3(xImage, lastY), diffy, 0, 6, Arrow2, NextArrow, PrevArrow, ItemPanel);
        fillWeapon();
        Menu2 = new Navigator();
        GameObject menusObj = GameObject.Find("Canvas").transform.FindChild("Select Panel").gameObject;
        List<GameObject> Options = new List<GameObject>();
        for (int i = 0; i < menusObj.transform.childCount; i++)
        {
            GameObject obj = menusObj.transform.GetChild(i).gameObject;
            if (!obj.name.Contains("Arrow"))
            {
                Options.Add(obj);
            }
        }
        Menu2.Init(Arrow3, Options);
        
    }
    /// <summary>
    /// Asigna los valores del player en el UI
    /// </summary>
    public void SetPlayerLabels() {
        Feet.GetComponent<Text>().text = player.Data.Feet.ItemName;
        Ring.GetComponent<Text>().text = player.Data.Ring.ItemName;
        Weapon.GetComponent<Text>().text = player.Data.MainHand.ItemName;
        Body.GetComponent<Text>().text = player.Data.Body.ItemName;
        Necklace.GetComponent<Text>().text = player.Data.Necklace.ItemName;
        Head.GetComponent<Text>().text = player.Data.Helmet.ItemName;
        MaxHP.GetComponent<Text>().text = player.Data.Stats.MaxHP.ToString();
        MaxMP.GetComponent<Text>().text = player.Data.Stats.MaxMP.ToString();
        Attack.GetComponent<Text>().text = player.Data.Stats.Attack.ToString();
        Magic.GetComponent<Text>().text = player.Data.Stats.Magic.ToString();
        MagicDefense.GetComponent<Text>().text = player.Data.Stats.MagicDefense.ToString();
        Agility.GetComponent<Text>().text = player.Data.Stats.Agility.ToString();
        Luck.GetComponent<Text>().text = player.Data.Stats.Luck.ToString();
        Feet.GetComponent<Armor>().Data.Stats = player.Data.Feet.Stats;
        Ring.GetComponent<Armor>().Data.Stats = player.Data.Ring.Stats;
        Weapon.GetComponent<Weapon>().Data.Stats = player.Data.MainHand.Stats;
        Body.GetComponent<Armor>().Data.Stats = player.Data.Body.Stats;
        Necklace.GetComponent<Armor>().Data.Stats = player.Data.Necklace.Stats;
        GameObject.Find("HelmetImage").GetComponent<Image>().sprite = player.Data.Helmet.Image;
        GameObject.Find("BodyImage").GetComponent<Image>().sprite = player.Data.Body.Image;
        GameObject.Find("FeetImage").GetComponent<Image>().sprite = player.Data.Feet.Image;
        GameObject.Find("WeaponImage").GetComponent<Image>().sprite = player.Data.MainHand.Image;
        GameObject.Find("NecklaceImage").GetComponent<Image>().sprite = player.Data.Necklace.Image;
        GameObject.Find("RingImage").GetComponent<Image>().sprite = player.Data.Ring.Image;
    }
    /// <summary>
    /// 
    /// </summary>
    public override void Select()
    {
        if (MenuSelected > 2)
        {
            MenuSelected = 2;
            return;
        }
        switch (MenuSelected)
        {
            case 0:
                MenuScroll.OnFirst();
                MenuSelected++;
                break;
            case 1:
                MenuSelected++;
                break;
            case 2:
                if (selectionName == "Equip" && player.Data.Level >= EquipSelect.MinLevel)
                {
                    switch (Itemselection)
                    {
                        case "Weapon":
                            player.Data.MainHand = EquipSelect as AbstractWeapon;
                            break;
                        case "Body":
                            player.Data.Body = EquipSelect as AbstractArmor;
                            break;
                        case "Helmet":
                            player.Data.Helmet = EquipSelect as AbstractArmor;
                            break;
                        case "Feet":
                            player.Data.Feet = EquipSelect as AbstractArmor;
                            break;
                        case "Necklace":
                            player.Data.Necklace = EquipSelect as AbstractArmor;
                            break;
                        case "Ring":
                            player.Data.Ring = EquipSelect as AbstractArmor;
                            break;
                        default:
                            break;
                    }
                    SetPlayerLabels();
                }
                MenuSelected--;
                break;
            default:
                break;
        }

    }
    public override void unSelect()
    {
        switch (MenuSelected)
        {
            case 0 :
                ///TODO
                MenuSelected--;
                break;
            case 1:
                ClearDiffText();
                MenuScroll.Reset();
                Description.GetComponent<Text>().text = "";
                MenuSelected--;
                break;
            case 2:
                MenuSelected--;
                break;
            default:
                break;
        }
        if (MenuSelected < 0)
            MenuSelected = 0;
    }
    /// <summary>
    /// Asigna el valor del texto que contiene la diferencia entre los stats.
    /// </summary>
    /// <param name="name">Nombre de la propiedad</param>
    /// <param name="val1">Valor del item equipado</param>
    /// <param name="val2">Valor del item Seleccionado</param>
    private void SetDiffVal(string name,int val1,int val2) {
        GameObject TextStats = GameObject.Find(name);
        TextStats.GetComponent<Text>().text = (val2-val1).ToString();
        TextStats.GetComponent<Text>().fontSize = 20;
        TextStats.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
        TextStats.GetComponent<Text>().verticalOverflow = VerticalWrapMode.Overflow;
        if (val2 >= val1)
        {
            TextStats.GetComponent<Text>().color = Color.blue;
        }
        else
        {
            TextStats.GetComponent<Text>().color = Color.red;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Selected"></param>
    public override void On(Item Selected) {
        EquipSelect = Selected as Equippable;
        string Desc = Selected.Description + ".Level Require " + EquipSelect.MinLevel;
        if (EquipSelect.State.State != "")
        {
            Desc += "Effect " + EquipSelect.State.State + ", Prob.(%)" + EquipSelect.State.RestrictionRate;
        }
        Description.GetComponent<Text>().text = Desc;
        switch (Itemselection)
        {
            case "Weapon":
                SetDiffText(player.Data.MainHand.Stats, EquipSelect.Stats);
                break;
            case "Body":
                SetDiffText(player.Data.Body.Stats, EquipSelect.Stats);
                break;
            case "Helmet":
                SetDiffText(player.Data.Helmet.Stats, EquipSelect.Stats);
                break;
            case "Feet":
                SetDiffText(player.Data.Feet.Stats, EquipSelect.Stats);
                break;
            case "Necklace":
                SetDiffText(player.Data.Necklace.Stats, EquipSelect.Stats);
                break;
            case "Ring":
                SetDiffText(player.Data.Ring.Stats, EquipSelect.Stats);
                break;
            default:
                break;
        }
    }
    private void SetDiffText(Attribute stats, Attribute stats2) {
        SetDiffVal("AgilityDiff", stats.Agility, stats2.Agility);
        SetDiffVal("DefenseDiff", stats.Defense, stats2.Defense);
        SetDiffVal("AttackDiff", stats.Attack, stats2.Attack);
        SetDiffVal("LuckDiff", stats.Luck, stats2.Luck);
        SetDiffVal("MagicDiff", stats.Magic, stats2.Magic);
        SetDiffVal("MagicDefenseDiff", stats.MagicDefense, stats2.MagicDefense);
        SetDiffVal("MaxHPDiff", stats.MaxHP, stats2.MaxHP);
        SetDiffVal("MaxMPDiff", stats.MaxMP, stats2.MaxMP);
    }
    /// <summary>
    /// Pone inactivo todos los text que muestran la diferencia de los stats.
    /// </summary>
    private void ClearDiffText(){
        GameObject.Find("MaxHPDiff").GetComponent<Text>().text = "";
        GameObject.Find("MaxMPDiff").GetComponent<Text>().text = "";
        GameObject.Find("AttackDiff").GetComponent<Text>().text = "";
        GameObject.Find("MagicDiff").GetComponent<Text>().text = "";
        GameObject.Find("DefenseDiff").GetComponent<Text>().text = "";
        GameObject.Find("MagicDefenseDiff").GetComponent<Text>().text = "";
        GameObject.Find("AgilityDiff").GetComponent<Text>().text = "";
        GameObject.Find("LuckDiff").GetComponent<Text>().text = "";
    }
    public void Update() {
        switch (MenuSelected)
        {
            case 0:
                ///TODO
                Menu.update();
                break;
            case 1:
                MenuScroll.update();
                break;
            case 2:
                Menu2.update();
                break;
            default:
                break;
        }
        
        delay = 0;
    }
    private void DestroyAll()
    {
        ActiveItems.Clear();
    }
    private void fillWeapon(){
            foreach (AbstractWeapon i in ListWeapon)
            {
                    ActiveItems.Add(i);
            }
            MenuScroll.ChangeList(ActiveItems);
        
    }
    private void fillArmor(AbstractArmor.ArmorType type){
        List<AbstractArmor> list = new List<AbstractArmor>();
        switch (type)
        {
            case AbstractArmor.ArmorType.Body:
                list = ListBody;
                break;
            case AbstractArmor.ArmorType.Feet:
                list = ListFeet;
                break;
            case AbstractArmor.ArmorType.Necklace:
                list = ListNecklace;
                break;
            case AbstractArmor.ArmorType.Ring:
                list = ListRing;
                break;
            case AbstractArmor.ArmorType.Helmet:
                list = ListHelmet;
                break;
            default:
                break;
        }
        
            foreach (AbstractArmor i in list)
            {
                ActiveItems.Add(i);
            }
            MenuScroll.ChangeList(ActiveItems);
    }
    public override void On(string name)
    {
        if (name == Itemselection)
            return;
        selectionName = name;
        switch (name)
        {
            case "Weapon":
                DestroyAll();
                fillWeapon();
                Itemselection = name;
                break;
            case "Helmet":
                DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Helmet);
                Itemselection = name;
                break;
            case "Body":
                DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Body);
                Itemselection = name;
                break;
            case "Feet":
               DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Feet);
                Itemselection = name;
                break;
            case "Necklace":
                DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Necklace);
                Itemselection = name;
                 break;
            case "Ring":
                 DestroyAll();
                 fillArmor(AbstractArmor.ArmorType.Ring);
                 Itemselection = name;
                 break;
            default:
                break;
        }
    }
}
