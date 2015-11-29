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
    private int xImage = 53;
    /// <summary>
    /// Posicion en x del texto del item
    /// </summary>
    private int xItem = -30;
    /// <summary>
    /// Diferencia en y entre cada item;
    /// </summary>
    private int diffy = 39;
    /// <summary>
    /// Seleccion del menu
    /// </summary>
    private string selection = "Weapon";
    /// <summary>
    /// Lista de items visibles
    /// </summary>
    private List<KeyValuePair<GameObject, GameObject>> ActiveItems;
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
    #endregion
    public void Start() {
        Feet.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Feet.ItemName;
        Ring.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Ring.ItemName;
        Weapon.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.MainHand.ItemName;
        Body.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Body.ItemName;
        Necklace.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Necklace.ItemName;
        Head.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Helmet.ItemName;
        MaxHP.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.MaxHP.ToString();
        MaxMP.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.MaxMP.ToString();
        Attack.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.Attack.ToString();
        Magic.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.Magic.ToString();
        MagicDefense.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.MagicDefense.ToString();
        Agility.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.Agility.ToString();
        Luck.GetComponent<Text>().text = GameObject.Find("PLAYER").GetComponent<Player>().Data.Stats.Luck.ToString();
        ListBody = new List<AbstractArmor>();
        ListFeet = new List<AbstractArmor>();
        ListHelmet = new List<AbstractArmor>();
        ListNecklace = new List<AbstractArmor>();
        ListRing = new List<AbstractArmor>();
        ListWeapon = new List<AbstractWeapon>();
        ActiveItems = new List<KeyValuePair<GameObject, GameObject>>();
        ListBody = GameObject.Find("PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Body);
        ListFeet = GameObject.Find("PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Feet);
        ListHelmet = GameObject.Find("PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Helmet);
        ListNecklace = GameObject.Find("PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Necklace);
        ListRing = GameObject.Find("PLAYER").GetComponent<Player>().Items.TypeArmor(AbstractArmor.ArmorType.Ring);
        ListWeapon = GameObject.Find("PLAYER").GetComponent<Player>().Items.Weapons;
        Item = Resources.Load("Menus/MenuItems") as GameObject;
        ItemImage = Resources.Load("Menus/MenuItemsImage") as GameObject;
        fillWeapon();
    }
    public void Select() {
        select = false;
    }
    public void unSelect() {
        select = true;
    }
    public void Update() {
    }
    private void DestroyAll()
    {
        foreach (KeyValuePair<GameObject, GameObject> i in ActiveItems) {
            Destroy(i.Key);
            Destroy(i.Value);
        }
        ActiveItems.Clear();
    }
    private void DestroyIndex(int i) { 
        Destroy(ActiveItems[i].Key);
        Destroy(ActiveItems[i].Value);
        ActiveItems.RemoveAt(i);
    }
    private void fillWeapon(){
        lastY = startY;
        if (ListWeapon.Count > 0)
        {
            Item.GetComponent<Text>().text = ListWeapon[0].ItemName;
            ItemImage.GetComponent<Image>().sprite = ListWeapon[0].Image;
            GameObject x = GameObject.Instantiate(Item);
            GameObject y = GameObject.Instantiate(ItemImage);
            x.transform.parent = ItemPanel.transform;
            x.transform.localPosition = new Vector3(xItem, lastY);
            y.transform.parent = ItemPanel.transform;
            y.transform.localPosition = new Vector3(xImage, lastY);
            KeyValuePair<GameObject, GameObject> newObj = new KeyValuePair<GameObject, GameObject>(x, y);
           ActiveItems.Add(newObj);
            lastY -= diffy;
        }
        if (ListWeapon.Count > 1)
        {
            int cant = 1;
            foreach (AbstractWeapon i in ListWeapon.GetRange(1, ListWeapon.Count-1))
            {
                if (cant == 6)
                    break;
                Item.GetComponent<Text>().text = i.ItemName;
                ItemImage.GetComponent<Image>().sprite = i.Image;
                GameObject ax = GameObject.Instantiate(Item);
                GameObject ay = GameObject.Instantiate(ItemImage);
                ax.transform.parent = ItemPanel.transform;
                ax.transform.localPosition = new Vector3(xItem, lastY);
                ay.transform.parent = ItemPanel.transform;
                ay.transform.localPosition = new Vector3(xImage, lastY);
                KeyValuePair<GameObject, GameObject> newObj = new KeyValuePair<GameObject, GameObject>(ax, ay);
                ActiveItems.Add(newObj);
                lastY -= diffy;
                cant++;
            }
        }
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
        lastY = startY;
        if (list.Count > 0)
        {
            Item.GetComponent<Text>().text = list[0].ItemName;
            ItemImage.GetComponent<Image>().sprite = list[0].Image;
            GameObject x = GameObject.Instantiate(Item);
            GameObject y = GameObject.Instantiate(ItemImage);
            x.transform.parent = ItemPanel.transform;
            x.transform.localPosition = new Vector3(xItem, lastY);
            y.transform.parent = ItemPanel.transform;
            y.transform.localPosition = new Vector3(xImage, lastY);
            KeyValuePair<GameObject, GameObject> newObj = new KeyValuePair<GameObject, GameObject>(x, y);
            ActiveItems.Add(newObj);
            lastY -= diffy;
        }
        if (list.Count > 1)
        {
            int cant = 1;
            foreach (AbstractArmor i in list.GetRange(1,list.Count-1))
            {
                if (cant == 6)
                    break;
                Item.GetComponent<Text>().text = i.ItemName;
                ItemImage.GetComponent<Image>().sprite = i.Image;
                GameObject ax = GameObject.Instantiate(Item);
                GameObject ay = GameObject.Instantiate(ItemImage);
                ax.transform.parent = ItemPanel.transform;
                ax.transform.localPosition = new Vector3(xItem, lastY);
                ay.transform.parent = ItemPanel.transform;
                ay.transform.localPosition = new Vector3(xImage, lastY);
                KeyValuePair<GameObject, GameObject> newObj = new KeyValuePair<GameObject, GameObject>(ax, ay);
                ActiveItems.Add(newObj);
                lastY -= diffy;
                cant++;
            }
        }
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
