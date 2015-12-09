using UnityEngine;
using System.Collections;

public class MenuOptionScen : MenuOption {
    public string SceneName;
    public override void OnSelect()
    {
        Constant.LastSceneLoaded = SceneName;

        if (Application.loadedLevelName == "MainMenu")
            Application.LoadLevel(SceneName);
        else
        {
            GameObject.Find("StartMenu").transform.FindChild("Canvas").gameObject.GetComponent<Menus>().Select();
            Application.LoadLevelAdditive(SceneName);
            
        }
        
    }

}
