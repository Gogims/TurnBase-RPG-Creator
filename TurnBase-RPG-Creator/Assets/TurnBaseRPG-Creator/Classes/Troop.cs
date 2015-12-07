using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
[Serializable]
public class Troop : Actor
{
    /// <summary>
    /// El fondo de arriba del combate
    /// </summary>
    public Sprite BackgroundTop;

    /// <summary>
    /// El fondo de abajo del combate
    /// </summary>
    public Sprite BackgroundBottom;

    /// <summary>
    /// Lista de enemigos de la batalla
    /// </summary>
    public List<BattleEnemy> Enemies;

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
    /// Ancho que ocupará el área que podrá moverse
    /// </summary>
    public int AreaWidth
    {
        get
        {
            return _width;
        }
        set
        {
            if (value != _width)
            {
                Changed = true;
            }

            _width = value;
        }
    }
    
    /// <summary>
    /// Alto que ocupará el área que podrá moverse
    /// </summary>
    public int AreaHeight
    {
        get
        {
            return _height;
        }
        set
        {
            if (value != _height)
            {
                Changed = true;
            }

            _height = value;
        }
    }

    /// <summary>
    /// El comportamiento de movimiento que tendrá la tropa
    /// </summary>
    public Constant.EnemyType Type;

    /// <summary>
    /// Si el área cambió
    /// </summary>
    public bool Changed;

    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;

    private float MinX;
    private float MaxX;
    private float MinY;
    private float MaxY;
    private bool axis;
    private int count;
    private float direction;    

    public Troop()
    {        
        Enemies = new List<BattleEnemy>();
        downSprites = new List<Sprite>();
        leftSprites = new List<Sprite>();
        upSprites = new List<Sprite>();
        rightSprites = new List<Sprite>();
    }

    /// <summary>
    /// Al iniciar el juego con tropas en la escena 
    /// </summary>
    protected override void Start()    
    {
        MinX = transform.position.x - AreaWidth;
        MinY = transform.position.y - AreaHeight;
        MaxX = transform.position.x + AreaWidth;
        MaxY = transform.position.y + AreaHeight;
    }

    void Update()
    {
        count++;
        bool down = false;
        bool left = false;
        bool up = false;
        bool right = false;

        switch (Type)
        {
            case Constant.EnemyType.Follower:

                break;
            case Constant.EnemyType.Random:
                if (count > 20)
                {
                    axis = UnityEngine.Random.Range(1, 3) % 2 == 0;
                    direction = UnityEngine.Random.Range(1, 3) % 2 == 0 ? -1 : 1;
                    count = 0; 
                }

                if (CanMove())
                {
                    if (axis)
                    {
                        if (direction == -1)
                            left = true;
                        else
                            right = true;

                        Move(direction, 0);
                    }                        
                    else
                    {
                        if (direction == -1)
                            down = true;
                        else
                            up = true;

                        Move(0, direction);
                    }
                        
                }
                
                break;
            case Constant.EnemyType.Stationary:
                break;
        }

        MoveDirection(down, "down", 0);
        MoveDirection(left, "left", 0.34f);
        MoveDirection(up, "up", 0.67f);
        MoveDirection(right, "right", 1);
    }  

    private bool CanMove()
    {
        float futurePositionX = NextPosition(direction) + transform.position.x;
        float futurePositionY = NextPosition(direction) + transform.position.y;

        if (futurePositionX <= MaxX && futurePositionX >= MinX &&
            futurePositionY <= MaxY && futurePositionY >= MinY)
        {
            return true;
        }

        return false;
    }
}