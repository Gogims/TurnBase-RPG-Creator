using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{    /// <summary>
    /// Inventorio del actor 
    /// </summary>
    public Inventory Items = new Inventory();
    void OnLevelWasLoaded(int level)
    {
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
        Data.InstanceStates();
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
                        Destroy(obstacle);
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
        horizontal = (Input.GetAxisRaw("Horizontal"));
        vertical = (Input.GetAxisRaw("Vertical"));

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
    
}