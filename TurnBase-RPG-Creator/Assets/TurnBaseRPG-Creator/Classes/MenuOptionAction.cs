using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOptionAction : MenuOption {
    public override void OnSelect()
    {
        GameObject.Find("Canvas").GetComponent<Menus>().Select();
    }
    public override void UnSelect() {
        GameObject.Find("Canvas").GetComponent<Menus>().unSelect();
    }
    public override void On(string name)
    {
        GameObject.Find("Canvas").GetComponent<Menus>().On(name);
    }
    public override void On(Item selected)
    {
        GameObject.Find("Canvas").GetComponent<Menus>().On(selected);
        
    }
    public override void On(AbstractAbility p)
    {
        GameObject.Find("Canvas").GetComponent<Menus>().On(p);
    }

}
