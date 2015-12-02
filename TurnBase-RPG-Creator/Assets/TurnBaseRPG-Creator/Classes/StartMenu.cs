using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenu : Menus {
    public GameObject PlayerName;
    public GameObject PlayerLvl;
    public GameObject PlayerHP;
    public GameObject PlayerMP;
    public GameObject PlayerJob;
    public GameObject PlayerGold;
    public GameObject PlayerFace;
    public GameObject PlayerBody;
    public GameObject HPSlider;
    public GameObject MPSlider;
    public void Start() {
       PlayerName.GetComponent<Text>().text =  GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Name;
       PlayerLvl.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Level.ToString();
       PlayerHP.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.HP.ToString() + "/" + GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MaxHP.ToString();
       PlayerMP.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.MP.ToString() + "/" + GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MaxMP.ToString();
       PlayerFace.GetComponent<Image>().sprite = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Icon;
       PlayerBody.GetComponent<Image>().sprite = GameObject.FindWithTag("RPG-PLAYER").GetComponent<SpriteRenderer>().sprite;
       HPSlider.GetComponent<Slider>().value = (float)GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.HP / (float)GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MaxHP;
       MPSlider.GetComponent<Slider>().value = (float)GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.MP / (float)GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Stats.MaxMP;
       PlayerJob.GetComponent<Text>().text = GameObject.FindWithTag("RPG-PLAYER").GetComponent<Player>().Data.Job.JobName;
    }
    public void Update() {
        Menu.update();
    }
}
