using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Clase que contiene los tipos de objetos que pueden existir en el mapa
/// </summary>
public class ObjectTypes
{
    /// <summary>
    /// Contiene todos los tipos y su descripcion.
    /// </summary>
	private static Dictionary<string,string> types = new Dictionary<string, string>();
	
	/// <summary>
	/// Inicializa la clase.
	/// </summary>
	private static void Init(){
		types.Add ("Wall", @"An object of this type will be use to represent 
a wall in the map, it will collide with other 
objects.");
		types.Add ("Tiles", @"An object of this type will be use to represent tiles 
of a map, it won't collide with other objects.");
		types.Add ("Pick Up", @"An object of this type will be use to represent 
a pick up item on the map, it will collide with 
other objects and it will be destroy on collision.");
		types.Add ("Obstacle", @"An object of this type will be use to represent 
Obstacles on the map, it will collide with other 
objects and it can be destroy.
");
		types.Add ("Item", @"An object of this type will be use to represent 
items on the inventory.");
		types.Add ("Weapon", @"An object of this type will be use to represent 
Weapons on the game.");
		types.Add ("Armor", @"An object of this type will be use to represent 
Armors on the game.");
		types.Add ("Character", @"An object of this type will be use to represent 
a character (enemy,npc,player).");
        types.Add("Background", @"An object of this type will be use to represent 
the background of a battle.");
	}
    /// <summary>
    /// Retorna en un arreglo de string todos los tipos.
    /// </summary>
    /// <returns></returns>
	static public string [] GetTypes(){
		if ( types.Count == 0) 
		{
			Init();
		}
		List<string> returVal = new List<string>(); 
		foreach (string i in types.Keys) {
			returVal.Add(i);
		}
		return returVal.ToArray ();
	}
    /// <summary>
    /// Retorna la descripcion de un tipo dado su llave.
    /// </summary>
    /// <param name="key">Llave del tipo</param>
    /// <returns>Retorna la descripcion de la llave pasada</returns>
	static public string GetDescription(string key){
		if ( types.Count == 0) 
		{
			Init();
		}
		return types [key];
	}

}

