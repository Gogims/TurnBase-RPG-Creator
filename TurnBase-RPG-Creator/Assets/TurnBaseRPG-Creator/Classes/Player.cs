using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Actor
{    /// <summary>
    /// Inventorio del actor 
    /// </summary>
    public Inventory Items = new Inventory();
    void OnLevelWasLoaded(int level)
    {
        GameObject.Find("PLAYER").transform.position = GameObject.Find("PLAYER(Clone)").transform.position;
        Destroy(GameObject.Find("PLAYER(Clone)"));

    }
    
    /// <summary>
    /// Listado de las animaciones para caminar hacia abajo
    /// </summary>
    public List<Sprite> downSprites;
    /// <summary>
    /// Listado de las animaciones para caminar hacia la izquierda
    /// </summary>
    public List<Sprite> leftSprites;
    /// <summary>
    /// Listado de las animaciones para caminar hacia arriba
    /// </summary>
    public List<Sprite> upSprites;
    /// <summary>
    /// Listado de las animaciones para caminar hacia la derecha
    /// </summary>
    public List<Sprite> rightSprites;

    /// <summary>
    /// Información del jugador
    /// </summary>
    public AbstractPlayer Data;


    public Player()
    {
        Data = new AbstractPlayer();
    }

    protected override void Start()
    {
        AbstractAbility aux = new AbstractAbility();
        AbstractUsable aux2 = new AbstractUsable();
        AbstractArmor aux3 = new AbstractArmor();
        AbstractArmor aux4 = new AbstractArmor();
        AbstractArmor aux5 = new AbstractArmor();
        AbstractArmor aux6 = new AbstractArmor();
        AbstractArmor aux7 = new AbstractArmor();
        AbstractArmor aux8 = new AbstractArmor();
        AbstractArmor aux9 = new AbstractArmor();
        AbstractArmor aux0 = new AbstractArmor();
        AbstractArmor aux11 = new AbstractArmor();
        AbstractArmor aux12 = new AbstractArmor();
        AbstractArmor aux13 = new AbstractArmor();
        aux2.ItemName = "Potion";
        aux.Ability = "Ice";
        aux.AttackPower = 10;
        aux3.ItemName = "Armor";
        aux3.Type = AbstractArmor.ArmorType.Body;
        Items.InsertArmor(aux3);
        Items.InsertArmor(aux3);
        Items.InsertArmor(aux3);
        Items.InsertArmor(aux3);
        aux4.Type = AbstractArmor.ArmorType.Feet;
        aux4.ItemName = "Armor2";
        Items.InsertArmor(aux4);
        Items.InsertArmor(aux4);
        aux5.Type = AbstractArmor.ArmorType.Helmet;
        aux5.ItemName = "Armor3";
        Items.InsertArmor(aux5);
        aux6.Type = AbstractArmor.ArmorType.Helmet;
        aux6.ItemName = "Armor6";
        Items.InsertArmor(aux6);
        aux7.Type = AbstractArmor.ArmorType.Helmet;
        aux7.ItemName = "Armor67";
        Items.InsertArmor(aux7);
        aux8.Type = AbstractArmor.ArmorType.Necklace;
        aux8.ItemName = "Armor4";
        Items.InsertArmor(aux8);
        aux9.Type = AbstractArmor.ArmorType.Ring;
        aux9.ItemName = "Armor5";
        Items.InsertArmor(aux9);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Items.InsertUsable(aux2);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux); 
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.Job.Abilities.Add(aux);
        Data.InstanceStates();
    }
    /// <summary>
    /// Aplica la experiencia al jugador
    /// </summary>
    /// <param name="XpCant">Cantidad de experiencia a aplicar</param>
    /// <returns>Retorna los atributos del nivel anterior si crece de nivel de lo contrario retorna null</returns>
    public Attribute ApplyXp(int XpCant) {
        Data.XPToLvlUp += XpCant;
        int cantToNextLvl = Data.Job.XP.GetValue(Data.Level);
        bool lvlup = false;
        while(Data.XPToLvlUp >= cantToNextLvl ){
            if (!lvlup) lvlup = true;
            Data.Level++;
            Data.XPToLvlUp = Data.XPToLvlUp - cantToNextLvl;
            cantToNextLvl = Data.Job.XP.GetValue(Data.Level);
        }
        Attribute Stats= null;
        if (lvlup)
        {
            Stats = new Attribute();
            Stats.Agility = Data.Stats.Agility;
            Stats.Attack = Data.Stats.Attack;
            Stats.Defense = Data.Stats.Defense;
            Stats.Luck = Data.Stats.Luck;
            Stats.Magic = Data.Stats.Magic;
            Stats.MagicDefense = Data.Stats.MagicDefense;
            Stats.MaxHP = Data.Stats.MaxHP;
            Stats.MaxMP = Data.Stats.MaxMP;
            Data.Stats.Agility = Data.Job.Agility.GetValue(Data.Level);
            Data.Stats.Attack = Data.Job.Attack.GetValue(Data.Level);
            Data.Stats.Defense = Data.Job.Defense.GetValue(Data.Level);
            Data.Stats.Luck = Data.Job.Luck.GetValue(Data.Level);
            Data.Stats.Magic = Data.Job.MagicAttack.GetValue(Data.Level);
            Data.Stats.MagicDefense = Data.Job.MagicDefense.GetValue(Data.Level);
            Data.Stats.MaxHP = Data.Job.MaxHP.GetValue(Data.Level);
            Data.Stats.MaxMP = Data.Job.MaxMP.GetValue(Data.Level);
            Data.HP = Data.Stats.MaxHP;
            Data.MP = Data.Stats.MaxMP;
        }
        return Stats;
    }
    void Update()
    {
        if (!Constant.start)
        {
            MoveDirection(ProxyInput.GetInstance().Down(), "down", 0);
            MoveDirection(ProxyInput.GetInstance().Left(), "left", 0.34f);
            MoveDirection(ProxyInput.GetInstance().Up(), "up", 0.67f);
            MoveDirection(ProxyInput.GetInstance().Right(), "right", 1);
            PlayerMove();
        }
        if (ProxyInput.GetInstance().Select() && !Constant.start)
        {
            Constant.ActiveMap = GameObject.Find("Map");
            Constant.ActiveMap.SetActive(false);
            Constant.LastSceneLoaded = "StartMenu";
            Application.LoadLevelAdditive("StartMenu");
            Constant.start = true;
        }
       
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        Pickup pickup = coll.gameObject.GetComponent<Pickup>();
        if (pickup != null)
        {
            if (ProxyInput.GetInstance().A())
            {
                if (pickup.Sound != null)
                {
                    Audio audio = new Audio(pickup.name);
                    audio.CreateAudioSource(pickup.Sound);
                    audio.Source.Play();
                    Destroy(audio.gameobject, pickup.Sound.length + 0.5f);
                }

                if (pickup.ItemArmor.ItemName != "")
                {
                    AbstractArmor armor = new AbstractArmor();
                    armor = pickup.ItemArmor;
                    this.Items.InsertArmor(armor);
                }
                if (pickup.ItemWeapon.ItemName != "")
                {
                    this.Items.InsertWeapon(pickup.ItemWeapon);
                }
                if (pickup.ItemUsable.ItemName != "")
                {
                    this.Items.InsertUsable(pickup.ItemUsable);
                }
                Destroy(coll.gameObject);
            }
        }

        var obstacle = coll.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            if (ProxyInput.GetInstance().A())
            {
                if (obstacle.Sound != null)
                {
                    Audio audio = new Audio(name);
                    audio.CreateAudioSource(obstacle.Sound);
                    audio.Source.Play();
                    Destroy(audio.gameobject, obstacle.Sound.length + 0.5f);
                }

                if (obstacle.Type == Constant.ObstacleType.Switchable)
                {
                    obstacle.Switched = !obstacle.Switched;
                }
                else if (obstacle.Type == Constant.ObstacleType.Destroyable)
                {
                    obstacle.hp -= Data.TotalDamage();

                    if (obstacle.hp <= 0)
                    {
                        Destroy(obstacle.gameObject);
                    }
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "RPG-ENEMY")
        {
            Destroy(coll.gameObject);
            GameObject p = GameObject.FindWithTag("RPG-PLAYER");
            Constant.ActiveMap = GameObject.Find("Map");
            Constant.ActiveMap.SetActive(false);
            DontDestroyOnLoad(p);
            p.SetActive(true);
            Constant.LastSceneLoaded = coll.gameObject.GetComponent<Troop>().Id;
            Application.LoadLevelAdditive(Constant.LastSceneLoaded);
        }
        if (coll.gameObject.tag == "RPG-MAPOBJECT")
        {   
            Door door = coll.gameObject.GetComponent<Door>();

            if (door != null && Constant.checkDoor(door.InMap))
            {
                string path = door.InMap.MapPath;
                GameObject p = GameObject.FindWithTag("RPG-PLAYER");
                p.name = "PLAYER";
                DontDestroyOnLoad(p);
                DontDestroyOnLoad(GameObject.Find("MobileSingleStickControl"));
                Application.LoadLevel(path.Substring(path.LastIndexOf("/")+1).Replace(".unity", ""));
                GameObject.FindWithTag("RPG-PLAYER").transform.position = new Vector3(door.X, door.Y);
            }
        }            
    }

    private bool PlayerMove()
    {
        float horizontal = 0;
        float vertical = 0;

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = ProxyInput.GetInstance().GetHorizontal();
        vertical = ProxyInput.GetInstance().GetVertical();

        if (Move(horizontal, vertical))
        {
            return false;
        }

        return true;
    }
}

[Serializable]
public class AbstractPlayer : AbstractActor
{
    public int XPToLvlUp;
}