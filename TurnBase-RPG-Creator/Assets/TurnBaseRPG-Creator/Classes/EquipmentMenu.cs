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
    private string selection = "Weapon";
    /// <summary>
    /// Lista de items visibles
    /// </summary>
    private List<Item> ActiveItems;
    public GameObject Arrow2;
    public GameObject NextArrow;
    public GameObject PrevArrow;
    private int startY = 97;
    ScrollMenu<Item> MenuScroll;
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
        Feet.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Feet.ItemName;
        Ring.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Ring.ItemName;
        Weapon.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.MainHand.ItemName;
        Body.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Body.ItemName;
        Necklace.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Necklace.ItemName;
        Head.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Helmet.ItemName;
        MaxHP.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MaxHP.ToString();
        MaxMP.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MaxMP.ToString();
        Attack.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.Attack.ToString();
        Magic.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.Magic.ToString();
        MagicDefense.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MagicDefense.ToString();
        Agility.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.Agility.ToString();
        Luck.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.Luck.ToString();
        ListBody = new List<AbstractArmor>();
        ListFeet = new List<AbstractArmor>();
        ListHelmet = new List<AbstractArmor>();
        ListNecklace = new List<AbstractArmor>();
        ListRing = new List<AbstractArmor>();
        ListWeapon = new List<AbstractWeapon>();
        ActiveItems = new List<Item>();
        ListBody = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Body);
        ListFeet = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Feet);
        ListHelmet = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Helmet);
        ListNecklace = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Necklace);
        ListRing = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Ring);
        ListWeapon = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Items.Weapons;
        Item = Resources.Load("Menus/MenuItems") as GameObject;
        ItemImage = Resources.Load("Menus/MenuItemsImage") as GameObject;
        MenuScroll = new ScrollMenu<Item>();
        MenuScroll.Init(new Vector3(xItem, lastY), new Vector3(xImage, lastY), diffy, 0, 6, Arrow2, NextArrow, PrevArrow, ItemPanel);
        fillWeapon();
        
    }
    public void Select() {
        select = false;
        select2 = true;
    }
    public void unSelect() {
        select = true;
        select2 = false;
    }
    public void Update() {
        if (!select2)
            return;
        if (delay < 15)
        {
            delay++;
            return;
        }

            MenuScroll.update();
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
    public void Load(string name)
    {
        if (name == selection)
            return;
        switch (name)
        {
            case "Weapon":
                DestroyAll();
                fillWeapon();
                selection = "Weapon";
                break;
            case "Helmet":
                DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Helmet);
                selection = "Helmet";
                break;
            case "Body":
                DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Body);
                selection = "Body";
                break;
            case "Feet":
               DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Feet);
                selection = "Feet";
                break;
            case "Necklace":
                DestroyAll();
                fillArmor(AbstractArmor.ArmorType.Necklace);
                selection = "Necklace";
                 break;
            case "Ring":
                 DestroyAll();
                 fillArmor(AbstractArmor.ArmorType.Ring);
                 selection = "Ring";
                 break;
            default:
                break;
        }
    }
}
