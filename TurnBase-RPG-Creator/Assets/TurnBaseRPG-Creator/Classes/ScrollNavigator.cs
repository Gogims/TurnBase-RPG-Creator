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
    private List<KeyValuePair<GameObject,Tuple<GameObject,GameObject>>> Options;
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
        
        font = (Font)Resources.Load("fonts/Vecna");
        Options = new List<KeyValuePair<GameObject,Tuple<GameObject,GameObject>>>();
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
    /// Destruye la lista de elementos que se estan mostrando
    /// </summary>
    private void DestroyOption()
    {
        foreach (KeyValuePair<GameObject,Tuple<GameObject,GameObject>> i in Options) {
            Destroy(i.Key);
            Destroy(i.Value.First);
            Destroy(i.Value.Second);
        }
        Options.Clear();

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
                    Options.Add(NewItem(cant));
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
                    Options.Add(NewItem(cant));
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


        Reorder();
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
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].Value.First.gameObject.transform.position.y);
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
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].Value.First.gameObject.transform.position.y);
        }
        
        delay = 0;

    }
    /// <summary>
    /// Agrega un elemento nuevo a la lista que se esta mostrando
    /// </summary>
    /// <param name="i">indice del elemento en la lista </param>
    /// <returns> Retorna el elemento que se creo en la scene</returns>
    private KeyValuePair<GameObject,Tuple<GameObject,GameObject>> NewItem(int i){
        GameObject Cant = NewText();
        GameObject Text = NewText();
        if (Items != null)
        {
            Cant.GetComponent<Text>().text = Items[i].Second.ToString();
            Text.GetComponent<Text>().text = Items[i].First.ItemName;
            ItemImage.GetComponent<Image>().sprite = Items[i].First.Image;

        }
        else {
            Cant.GetComponent<Text>().text = "";
            Text.GetComponent<Text>().text = Ability[i].Ability;
            ItemImage.GetComponent<Image>().sprite = Ability[i].Image;        
        }
        GameObject ay = Instantiate(ItemImage);
        Text.transform.parent = Panel.transform;
        Text.transform.localPosition = Position;
        Text.transform.localScale = new Vector3(1, 1);
        Constant.SetAnchorPoint(Text);
        ay.transform.SetParent(Panel.transform);
        Cant.transform.SetParent(Panel.transform);
        Cant.transform.localPosition = PositionCant;
        Cant.transform.localScale = new Vector3(1, 1);
        Constant.SetAnchorPoint(Cant);
        ay.transform.parent = Panel.transform;
        ay.transform.localPosition = ImagePosition;
        ay.transform.localScale = new Vector3(1, 1);
        //Constant.SetAnchorPoint(ay);
        
        
        KeyValuePair<GameObject, Tuple<GameObject, GameObject>> aux = new KeyValuePair<GameObject, Tuple<GameObject, GameObject>>(Text, new Tuple<GameObject, GameObject>(ay, Cant));
        return aux;
    }
    /// <summary>
    /// Agrega el elemento siguiente a la lista que se esta mostrando y elimina el primer elemento que se esta mostrando
    /// </summary>
    private void NewListNext()
    {
        if (selected + ScrollCant <= ElementCant - 1)
        {
            Destroy(Options[0].Key);
            Destroy(Options[0].Value.First);
            Destroy(Options[0].Value.Second);
            Options.RemoveAt(0);
            Options.Add(NewItem((MaxItem-1) + ScrollCant));
            PrevArrow.SetActive(true);
        }
        else
        {
            NextArrow.SetActive(false);
        }
        Reorder();
    }
    /// <summary>
    /// Agrega un elemento en la primera posicion de la lista que se esta mostrando y elimina el ultimo elemento.
    /// </summary>
    private void NewListPrev() {
        Destroy(Options[Options.Count - 1].Key);
        Destroy(Options[Options.Count - 1].Value.First);
        Destroy(Options[Options.Count - 1].Value.Second);
        Options.RemoveAt(Options.Count - 1);
        Options.Insert(0,NewItem(ScrollCant));
        NextArrow.SetActive(true);
        if (ScrollCant == 0)
            PrevArrow.SetActive(false);
        Reorder();
    }
    /// <summary>
    /// Reordena la lista que se esta mostrando
    /// </summary>
    private void Reorder(){
        Position = new Vector3(PosX, PosY);
        ImagePosition = new Vector3(ImagePosX, ImagePosY);
        PositionCant = new Vector3(CantX, CantY);
        foreach (KeyValuePair<GameObject,Tuple<GameObject,GameObject>> i in Options) {
            i.Key.transform.localPosition = Position;
            i.Value.First.transform.localPosition = ImagePosition;
            i.Value.Second.transform.localPosition = PositionCant;
            ImagePosition.x += DifX;
            ImagePosition.y += DifY;
            Position.x += DifX;
            Position.y += DifY;
            PositionCant.x += DifX;
            PositionCant.y += DifY;
        }
            
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
