﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollMenu<T>  : MonoBehaviour
    where T: Item
{
    /// <summary>
    /// Cantidad de elementos que se han agregado despues que se hizo el scroll.
    /// </summary>
    private int ScrollCant = 0;
    /// <summary>
    /// La opcion seleccionada
    /// </summary>
    private int selected = 0;
    /// <summary>
    /// Opciones que aparecen en el menu Con su imagen.
    /// </summary>
    public List<KeyValuePair<GameObject,GameObject>> Options;
    /// <summary>
    /// Lista de item que va ir mostrando
    /// </summary>
    public List<GameObject> Items;
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
    private GameObject Arrow;
    private GameObject NextArrow;
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
    /// La posicion siguiente de la imagen que se va mostrar en el menu.
    /// </summary>
    private Vector3 PositionImage;
    /// <summary>
    /// Panel del cual los elementos van hacer hijo.
    /// </summary>
    private GameObject Panel;
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
    public void Init(Vector3 position, Vector3 positionImage, int difY, int difX, List<GameObject> items,int maxItem, GameObject arrow, GameObject nextArrow, GameObject prevArrow,GameObject panel)
    {
        PositionImage = positionImage;
        Position = position;
        DifY = difY;
        Panel = panel;
        Items =items;
        MaxItem = maxItem;
        Arrow = arrow;
        NextArrow = nextArrow;
        PrevArrow = prevArrow;
        NextArrow.SetActive(false);
        PrevArrow.SetActive(false);
        Item = Resources.Load("Menus/MenuItems") as GameObject;
        ItemImage = Resources.Load("Menus/MenuItemsImage") as GameObject;
        PosX = (int)position.x;
        PosY = (int)position.y;
        ImagePosX = (int)positionImage.x;
        ImagePosY = (int)positionImage.y;
        ImagePosition = positionImage;
        int cant = 0;
        foreach (GameObject i in  items)
        {
            if (cant < MaxItem)
            {
                Options.Add(NewItem(cant));

            }
            else {
                break;
            }
        }
        if (Options.Count > MaxItem)
            NextArrow.SetActive(true);
        
    }
    public void update()
    {
        if (ProxyInput.GetInstance().B())
        {
            Options[selected].Key.GetComponent<MenuOption>().UnSelect();
            return;
        }
        if (ProxyInput.GetInstance().Down())
        {
            if (selected + ScrollCant < Items.Count - 1)
            {
                 NextArrow.SetActive(true);
            }
            if (selected == MaxItem - 1 && (selected + ScrollCant )< (Items.Count - 1))
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                ScrollCant++;
                NewListNext();
         
                Options[selected].Key.GetComponent<MenuOption>().On(Options[selected].Key.GetComponent<Text>().text);
            }
            else if (selected < Options.Count-1)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected++;
                Options[selected].Key.GetComponent<MenuOption>().On(Options[selected].Key.GetComponent<Text>().text);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].Key.gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().Up())
        {
            if (ScrollCant > 0)
                PrevArrow.SetActive(true);
            if ( ScrollCant > 0 && selected == 0)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                ScrollCant--;
                NewListPrev();
                Options[selected].Key.GetComponent<MenuOption>().On(Options[selected].Key.GetComponent<Text>().text);
            }
            else if (selected > 0)
            {
                //Options[selected].GetComponent<MenuOption>().Off(Options[selected].GetComponent<Text>().text);
                selected--;
                Options[selected].Key.GetComponent<MenuOption>().On(Options[selected].Key.GetComponent<Text>().text);
            }
            Arrow.transform.position = new Vector3(Arrow.transform.position.x, Options[selected].Key.gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().A())
        {
            Options[selected].Key.GetComponent<MenuOption>().OnSelect();
        }
    }
    private KeyValuePair<GameObject,GameObject> NewItem(int i){
        Item.GetComponent<Text>().text = Items[i].GetComponent<T>().ItemName;
        ItemImage.GetComponent<Image>().sprite = Items[i].GetComponent<T>().Image;
        Item.transform.parent = Panel.transform;
        ItemImage.transform.parent = Panel.transform;
        Item.transform.localPosition = Position;
        ItemImage.transform.localPosition = ImagePosition;
        KeyValuePair<GameObject, GameObject> aux = new KeyValuePair<GameObject, GameObject>(Instantiate(Item), Instantiate(ItemImage));
        return aux;
    }
    private void NewListNext()
    {
        if (MaxItem + ScrollCant < Items.Count - 1)
        {
            Destroy(Options[0].Key);
            Destroy(Options[0].Value);
            Options.RemoveAt(0);
            Options.Add(NewItem(MaxItem + ScrollCant));
        }
        else
        {
            NextArrow.SetActive(false);
        }
        Reorder();
    }
    private void NewListPrev() {
        Destroy(Options[Options.Count - 1].Key);
        Destroy(Options[Options.Count - 1].Value);
        Options.RemoveAt(Options.Count - 1);
        Options.Insert(0,NewItem(ScrollCant));
        if (ScrollCant == 0)
            PrevArrow.SetActive(false);
        Reorder();
    }
    private void Reorder(){
        Position = new Vector3(PosX, PosY);
        ImagePosition = new Vector3(ImagePosX, ImagePosY);
        foreach (KeyValuePair<GameObject,GameObject> i in Options) {
            i.Key.transform.localPosition = Position;
            i.Value.transform.localPosition = ImagePosition;
            ImagePosition.x += DifX;
            ImagePosition.y += DifY;
            Position.x += DifX;
            Position.y += DifY;
        }
            
    }
}