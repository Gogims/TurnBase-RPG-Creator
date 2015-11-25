using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Menus : MonoBehaviour {
    public Menus() {
        Data = new AbstractMenu();
    }
    public AbstractMenu Data;
    public GameObject MenuName;
    public GameObject BackgroundImage;
    private List<GameObject> Options;
    private int delay;
    public void Awake() {
        MenuName.GetComponent<Text>().text = Data.Name;
      //BackgroundImage.GetComponent<Image>().sprite = Data.Image;
        GameObject menusObj = GameObject.Find("Canvas");
        Options = new List<GameObject>();
        for (int i = 0; i < menusObj.transform.childCount; i++)
        {
            GameObject obj = menusObj.transform.GetChild(i).gameObject;
            if (obj.name.Contains("Select"))
            {
                Options.Add(obj);
            }
        }
    }
    void OnGUI() {
        if (delay < 15){
            delay++;
            return;
        }
        if (ProxyInput.GetInstance().Down())
        {
            if (Data.selected == Options.Count -1)
                Data.selected = 0 ;
            else
                Data.selected++;
            Data.Arrow.transform.position = new Vector3(Data.Arrow.transform.position.x, Options[Data.selected].gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().Up()) 
        {
            if (Data.selected == 0)
                Data.selected = Options.Count - 1;
            else
                Data.selected--;
            Data.Arrow.transform.position = new Vector3(Data.Arrow.transform.position.x, Options[Data.selected].gameObject.transform.position.y);
        }
        else if (ProxyInput.GetInstance().A())
        {
            Options[Data.selected].GetComponent<MenuOption>().OnSelect();
        }
        delay = 0;
    }
}

[Serializable]
public class AbstractMenu {
    
    public int selected = 0;
    public GameObject Arrow;
    //public Sprite Image;
    public string Name = "Age Of Empire";
}