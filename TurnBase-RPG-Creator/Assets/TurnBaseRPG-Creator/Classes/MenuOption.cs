using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour {
    public MenuOption()
    {
        Data = new AbstractMenuOption();
    }
    public AbstractMenuOption Data;
    public GameObject Item;
    public void Awake() {
        Item.GetComponent<Text>().text = Data.Text;
    }
    public void OnSelect()
    {
        Application.LoadLevel(Data.SceneName);
    }
}
[Serializable]
public class AbstractMenuOption
{
    public string SceneName;
    public string Text;
}
