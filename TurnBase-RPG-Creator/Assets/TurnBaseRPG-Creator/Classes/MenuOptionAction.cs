using UnityEngine;
using System.Collections;

public class MenuOptionAction : MenuOption {
    public override void OnSelect()
    {
        GameObject.Find("Canvas").GetComponent<EquipmentMenu>().Select();
    }
    public override void UnSelect() {
        GameObject.Find("Canvas").GetComponent<EquipmentMenu>().unSelect();
    }
    public override void On(string name)
    {
        GameObject.Find("Canvas").GetComponent<EquipmentMenu>().Load(name);
    }

}
