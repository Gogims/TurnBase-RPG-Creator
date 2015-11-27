using UnityEngine;
using System.Collections;

public class MenuOptionScen : MenuOption {
    public string SceneName;
    public override void OnSelect()
    {
        Application.LoadLevel(SceneName);
    }

}
