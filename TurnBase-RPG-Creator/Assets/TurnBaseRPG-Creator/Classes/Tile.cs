using UnityEngine;
using System.Collections;
/// <summary>
/// Esta clase representa una loseta en el juego 
/// </summary>
public class Tile: MapObject
{
    /// <summary>
    /// Tipo de tile 
    /// </summary>
    public Constant.TileType Type;

    /// <summary>
    /// Gameobject que se "removerá" cuando se presione el tile
    /// </summary>
    public GameObject Removable;

    /// <summary>
    /// Si el tile se encuentra presionado por un objeto
    /// </summary>
    private bool Pressed = true;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "RPG-PLAYER" || (coll.gameObject.tag == "RPG-MAPOBJECT" && coll.gameObject.GetComponent<Obstacle>() != null))
        {
            Pressed = false;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "RPG-PLAYER" || (coll.gameObject.tag == "RPG-MAPOBJECT" && coll.gameObject.GetComponent<Obstacle>() != null))
        {
            Pressed = true;
        }
    }

    void Update()
    {
        if (Removable != null)
        {
            Removable.SetActive(Pressed);
        }
    }

}
