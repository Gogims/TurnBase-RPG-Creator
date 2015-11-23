using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Troop : RPGElement
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
    /// La dirección de la escena
    /// </summary>
    public string TroopPath;

    public Troop()
    {
        Enemies = new List<EnemyBattle>();        
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