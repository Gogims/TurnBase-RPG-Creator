using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOptionAction : MenuOption {
    public override void OnSelect()
    {
        GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").GetComponent<Menus>().Select();
    }
    public override void On(string name)
    {
        GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").GetComponent<Menus>().On(name);
    }
    public override void On(Item selected)
    {
        GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").GetComponent<Menus>().On(selected);
        
    }
    public override void On(AbstractAbility p)
    {
        GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").GetComponent<Menus>().On(p);
    }

}
