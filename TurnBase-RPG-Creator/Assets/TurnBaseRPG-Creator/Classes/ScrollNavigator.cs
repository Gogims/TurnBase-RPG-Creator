using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollNavigator<T,U> : AbstractNavigator
    where T: Item 
    where U : AbstractAbility
{
    /// <summary>
    /// Cantidad de elementos que se han agregado despues que se hizo el scroll.
    /// </summary>
    private int ScrollCant = 0;
    /// <summary>
    /// Opciones que aparecen en el menu Con su imagen.
    /// </summary>
    private List<KeyValuePair<GameObject,GameObject>> Options;
    /// <summary>
    /// Lista de items que van mostrar
    /// </summary>
    private List<Tuple<T,int>> Items;
    /// <summary>
    /// Lista de Abilidades que van mostrar
    /// </summary>
    private List<U> Ability;
    /// <summary>
    /// La posicion siguiente del texto que se va mostrar en el menu.
    /// </summary>
    private Vector3 Position;
    /// <summary>
    /// La posicion siguiente de la imagen que se va mostrar en el menu.
    /// </summary>
    private Vector3 ImagePosition;
    /// <summary>
    /// La cantidad maxima de items que se van a mostrar a la vez.
    /// </summary>
    private int MaxItem;
    /// <summary>
    /// La diferencia en y que va tener cada elemento.
    /// </summary>
    private int DifY;
    /// <summary>
    /// Diferencia en x que va tener cada elemento.
    /// </summary>
    private int DifX;
    /// <summary>
    /// Posicion donde se va iniciar a renderizar el texto del item
    /// </summary>
    #region StartPosition
    private int PosX;
    private int PosY;
    #endregion
    /// <summary>
    /// Posicion donde se va iniciar a renderizar la imagen del item
    /// </summary>
    #region ImageStartPosition
    private int ImagePosX;
    private int ImagePosY;
    #endregion
    /// <summary>
    /// Posicion donde va iniciar a rendereizar la cantidad.
    /// </summary>
    #region CantStartPosition
    int CantX;
    int CantY;
    #endregion
    /// <summary>
    /// Flecha que se muestra si hay mas elementos hacia abajo
    /// </summary>
    private GameObject NextArrow;
    /// <summary>
    /// Flecha que se muestra si hay mas elementos hacia arriba 
    /// </summary>
    private GameObject PrevArrow;
    /// <summary>
    /// Prefab del elemento que se coloca como item
    /// </summary>
    private GameObject Item;
    /// <summary>
    /// Prefab de la imagen del item
    /// </summary>
    private GameObject ItemImage;
    /// <summary>
    /// Prefab del elemento que se coloca como cantidad
    /// </summary>
    private GameObject ItemCant;
    
    /// <summary>
    /// La posicion siguiente de la imagen que se va mostrar en el menu.
    /// </summary>
    private Vector3 PositionImage;
    /// <summary>
    /// La posicion siguiente del texto de cantidad que se va mostrar en el menu.
    /// </summary>
    private Vector3 PositionCant;
    /// <summary>
    /// Panel del cual los elementos van hacer hijo.
    /// </summary>
    private GameObject Panel;
    /// <summary>
    /// Posicion en x en donde inicia el selector.
    /// </summary>
    private int ArrowX;
    /// <summary>
    /// Cantidad de elementos en la list
    /// </summary>
    private int ElementCant = 0;
    /// <summary>
    /// Font de la letra
    /// </summary>
    private Font font;
    /// <summary>
    /// lista de texto que se muestran 
    /// </summary>
    private List<GameObject> listOption;
    private List<GameObject> listQuantity;
    /// <summary>
    /// Incializa los valores de la clase
    /// </summary>
    /// <param name="position">posicion donde va iniciar el texto</param>
    /// <param name="positionImage">posicion donde va iniciar la imagen</param>
    /// <param name="difY">Delta de y de cada imagen y texto</param>
    /// <param name="difX">Delta de x de cada imagen y texto</param>
    /// <param name="items">Lista de items que se van a mostrar</param>
    /// <param name="maxItem">Cantidad maxima de la lista</param>
    /// <param name="arrow">Selector del menu</param>
    /// <param name="nextArrow">imagen que se muestra cuando hay mas elementos hacia abajo</param>
    /// <param name="prevArrow">Imagen que se muestra cuando hay mas elementos hacia arriba</param>
    /// <param name="panel">Panel padre de la lista</param>
    public override void Init(Vector3 position, Vector3 positionImage,Vector3 positionCant, int difY, int difX, int maxItem, GameObject arrow, GameObject nextArrow, GameObject prevArrow,GameObject panel)
    {

        Options = new List<KeyValuePair<GameObject, GameObject>>();
        font = (Font)Resources.Load("fonts/Vecna");
        for (int i = 1 ; i <= maxItem; i++) {
            GameObject child = panel.transform.FindChild("Item"+i).gameObject;
            GameObject child2 = panel.transform.FindChild("Quantity" + i).gameObject;
            Options.Add(new KeyValuePair<GameObject, GameObject>(child, child2));
        }
        PositionImage = positionImage;
        Position = position;
        PositionCant = positionCant;
        DifY = difY;
        Panel = panel;
        MaxItem = maxItem;
        Arrow = arrow;
        NextArrow = nextArrow;
        PrevArrow = prevArrow;
        NextArrow.SetActive(false);
        PrevArrow.SetActive(false);
        Arrow.SetActive(false);
        Item = Resources.Load("Menus/MenuItems") as GameObject;
        ItemImage = Resources.Load("Menus/MenuImage") as GameObject;
        PosX = (int)position.x;
        PosY = (int)position.y;
        ImagePosX = (int)positionImage.x;
        ImagePosY = (int)positionImage.y;
        CantX = (int)positionCant.x;
        CantY = (int)positionCant.y;
        ImagePosition = positionImage;
        ArrowX = (int)Arrow.transform.localPosition.x;
    }
    /// <summary>
    /// Crea un nuevo objeto texto 
    /// </summary>
    /// <returns></returns>
    private GameObject NewText()
    {
        GameObject Obj = new GameObject();
        Text x = Obj.AddComponent<Text>();
        x.font = font;
        x.fontSize = 20;
        x.supportRichText = true;
        x.horizontalOverflow = HorizontalWrapMode.Overflow;
        x.verticalOverflow = VerticalWrapMode.Overflow;
        Obj.AddComponent<MenuOptionAction>();
        ContentSizeFitter csf = Obj.AddComponent<ContentSizeFitter>();
        csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize; 

        return Obj;
    }
    /// <summary>
    /// asigna la lista de elementos
    /// </summary>
    /// <param name="newItems">Lista de elementos</param>
    public void setList(List<Tuple<T,int>> newItems) {
        Ability = null;
        Items = newItems;
    }
    public void setList(List<U> newAbility) {
        Items = null;
        Ability = newAbility;
    }
    /// <summary>
    /// Cambia la lista de elementos y la muestra
    /// </summary>
    /// <param name="newItems">Nueva lista de elementos</param>
    public void ChangeList(List<Tuple<T,int>> newItems) {
        Ability = null;
        Items = newItems;
        DisplayList();
    }
    /// <summary>
    /// Cambia la lista de elementos y la muestra
    /// </summary>
    /// <param name="newItems">Nueva lista de elementos</param>
    public void ChangeList(List<U> newAbility)
    {
        Items = null;
        Ability = newAbility;
        DisplayList();
    }

   /// <summary>
    /// Oculta el menu.
    /// </summary>
    public void HideList() {
        NextArrow.SetActive(false);
        PrevArrow.SetActive(false);
        Arrow.SetActive(false);
        DestroyOption();

    }
    /// <summary>
    /// Muestra el menu
    /// </summary>
    public void DisplayList() {
        DestroyOption();
        if (Items != null && Items.Count == 0)
        {
            Arrow.SetActive(false);
            NextArrow.SetActive(false);
            return;
        }
        else if (Ability != null && Ability.Count == 0)
        {
            Arrow.SetActive(false);
            NextArrow.SetActive(false);
            return;
        }
        Arrow.SetActive(true);
        int cant = 0;
        if (Items != null)
        {
            foreach (Tuple<T,int> i in Items)
            {
                if (cant < MaxItem)
                {
                    NewItem(cant,cant);
                    cant++;
                }
                else
                {
                    break;
                }
            }
            if (Items.Count > MaxItem)
                NextArrow.SetActive(true);
            ElementCant = Items.Count;
        }
        else {
            foreach (U i in Ability)
            {
                if (cant < MaxItem)
                {
                    NewItem(cant,cant);
                    cant++;
                }
                else
                {
                    break;
                }
            }
            if (Ability.Count > MaxItem)
                NextArrow.SetActive(true);
            ElementCant = Ability.Count;
        }
    }

    private void DestroyOption()
    {
        foreach (var i in Options)
        {
            i.Key.GetComponent<Text>().text = "";
            i.Value.GetComponent<Text>().text = "";

        }
    }
    /// <summary>
    /// Funciona para navegar el menu
    /// </summary>
    public override void update()
    {
        if (!Arrow.activeSelf) return;
        if (ProxyInput.GetInstance().BUp())
        {
            Options[selected].Key.GetComponent<MenuOption>().UnSelect();
            return;
        }
        else if (ProxyInput.GetInstance().AUp())
        {
            Options[selected].Key.GetComponent<MenuOption>().OnSelect();
        }
        

        else if (ProxyInput.GetInstance().DownUp())
        {
            if (selected + ScrollCant < ElementCant - 1)
            {
                 NextArrow.SetActive(true);
            }
            if (selected == MaxItem - 1 && (selected + ScrollCant) < (ElementCant-1))
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                ScrollCant++;
                NewListNext();
                if (Items != null)
                    Options[selected].Key.GetComponent<MenuOption>().On(Items[selected + ScrollCant].First);
                else
                    Options[selected].Key.GetComponent<MenuOption>().On(Ability[selected + ScrollCant]);
            }
            else if (selected < Options.Count-1)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected++;
                if (Items != null)
                    Options[selected].Key.GetComponent<MenuOption>().On(Items[selected + ScrollCant].First);
                else
                    Options[selected].Key.GetComponent<MenuOption>().On(Ability[selected + ScrollCant]);
            }
            else {
                NextArrow.SetActive(false);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].Key.gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().UpUp())
        {
            if (ScrollCant > 0)
                PrevArrow.SetActive(true);
            if ( ScrollCant > 0 && selected == 0)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                ScrollCant--;
                NewListPrev();
                if (Items != null)
                    Options[selected].Key.GetComponent<MenuOption>().On(Items[selected + ScrollCant].First);
                else
                    Options[selected].Key.GetComponent<MenuOption>().On(Ability[selected + ScrollCant]);
            }
            else if (selected > 0)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected--;
                if (Items != null)
                    Options[selected].Key.GetComponent<MenuOption>().On(Items[selected + ScrollCant].First);
                else
                    Options[selected].Key.GetComponent<MenuOption>().On(Ability[selected + ScrollCant]);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].Value.gameObject.transform.position.y);
        }
        
        delay = 0;

    }
    /// <summary>
    /// Agrega un elemento nuevo a la lista que se esta mostrando
    /// </summary>
    /// <param name="i">indice del elemento en la lista </param>
    /// <returns> Retorna el elemento que se creo en la scene</returns>
    private void NewItem(int i,int position){
        if (Items != null)
        {
            Options[position].Key.GetComponent<Text>().text = Items[i].First.ItemName;
            Options[position].Value.GetComponent<Text>().text = Items[i].Second.ToString();
        }
        else {
            Options[position].Key.GetComponent<Text>().text = Ability[i].Ability;
            Options[position].Value.GetComponent<Text>().text = "";
        
        }
    }
    /// <summary>
    /// Agrega el elemento siguiente a la lista que se esta mostrando y elimina el primer elemento que se esta mostrando
    /// </summary>
    private void NewListNext()
    {
        if (selected + ScrollCant <= ElementCant - 1)
        {
            NewItem((MaxItem-1) + ScrollCant,MaxItem-1);
            PrevArrow.SetActive(true);
        }
        else
        {
            NextArrow.SetActive(false);
        }
    }
    /// <summary>
    /// Agrega un elemento en la primera posicion de la lista que se esta mostrando y elimina el ultimo elemento.
    /// </summary>
    private void NewListPrev() {
        NewItem(ScrollCant,0);
        NextArrow.SetActive(true);
        if (ScrollCant == 0)
            PrevArrow.SetActive(false);
    }
    /// <summary>
    /// Restaura los valores de las propiedades a como fueron inicializadas
    /// </summary>
    public void Reset()
    {
        Position = new Vector3(PosX, PosY);
        ImagePosition = new Vector3(ImagePosX, ImagePosY);
        PositionCant = new Vector3(CantX, CantY);
        Arrow.transform.localPosition = new Vector3(ArrowX, ImagePosY);
        DestroyOption();
        DisplayList();
        PrevArrow.SetActive(false);
    }
    /// <summary>
    /// Selecciona el primer elemento de la lista
    /// </summary>
    public void OnFirst()
    {
        if (Options.Count > 0)
        {
            if (Items != null)
                Options[0].Key.GetComponent<MenuOption>().On(Items[0].First);
            else
                Options[0].Key.GetComponent<MenuOption>().On(Ability[0]);
        }
        
    }

}
