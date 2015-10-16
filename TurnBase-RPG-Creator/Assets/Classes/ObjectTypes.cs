using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectTypes
{
	private static Dictionary<string,string> types = new Dictionary<string, string>();
	
	
	private static void Init(){
		types.Add ("Wall", @"Specify a wall,an object of this type will collide 
with other objects.");
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
	}
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
	static public string GetDescription(string key){
		if ( types.Count == 0) 
		{
			Init();
		}
		return types [key];
	}

}

