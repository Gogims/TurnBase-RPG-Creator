using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

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
    public List<EnemyBattle> Enemies;

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
    public int AreaWidth;

    /// <summary>
    /// Alto que ocupará el área que podrá moverse
    /// </summary>
    public int AreaHeight;

    /// <summary>
    /// La dirección de la escena
    /// </summary>
    public string TroopPath;

    /// <summary>
    /// El comportamiento de movimiento que tendrá la tropa
    /// </summary>
    public Constant.EnemyType Type;

    private float MinX;
    private float MaxX;
    private float MinY;
    private float MaxY;

    public Troop()
    {
        Enemies = new List<EnemyBattle>();
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
        switch (Type)
        {
            case Constant.EnemyType.Follower:

                break;
            case Constant.EnemyType.Random:

                break;
            case Constant.EnemyType.Stationary:
                break;
        }
    }

    /// <summary>
    /// Crea la escena del battlemap
    /// </summary>
    public void CreateTroopScene()
    {
        EditorApplication.NewScene(); // Crea una scene nueva.
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 6.95f; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)        
        Camera.main.rect = new Rect(0, 0, 1, 1.4f);

        CreateBackground("Bottom", BackgroundBottom, 0);
        CreateBackground("Top", BackgroundTop, 1);

        string returnPath = "Assets/Resources/BattleMaps/" + Id + ".unity";
        TroopPath = Directory.GetCurrentDirectory() + '\\' + returnPath.Replace('/', '\\');
        EditorApplication.SaveScene(returnPath);// Guarda la scene.
    }

    private GameObject CreateBackground(string name, Sprite background, int OrderLayer)
    {
        GameObject gobj = new GameObject(name);
        SpriteRenderer renderer = gobj.AddComponent<SpriteRenderer>();
        renderer.sprite = background;
        renderer.sortingLayerName = "Background";
        renderer.sortingOrder = OrderLayer;

        return gobj;
    }
}

[Serializable]
public class EnemyBattle
{
    /// <summary>
    /// Enemigo del battle map
    /// </summary>
    public AbstractEnemy Enemy;

    /// <summary>
    /// Position donde se presenta el enemigo
    /// </summary>
    public Rect EnemyPosition;    

    public EnemyBattle()
    {
        Enemy = new AbstractEnemy();
        EnemyPosition = new Rect();
    }
}