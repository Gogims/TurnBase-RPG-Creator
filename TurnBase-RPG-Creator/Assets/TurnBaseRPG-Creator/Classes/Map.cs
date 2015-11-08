using System.Collections.Generic;
/// <summary>
/// Map.
/// </summary>
using UnityEditor;
using UnityEngine;


public class Map{
	public int Heigth {get;set;}

	public int Width {get;set;}

	public string Name {get;set;}

	/// <summary>
	/// Crea un mapa dado su ancho, alto y nombre.
	/// </summary>
	public void CreateMap(){
		EditorApplication.NewScene(); // Crea una scene nueva.
		Camera.main.orthographicSize = 5; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)
		Camera.main.transform.localPosition = new Vector3((float)(Width-1)/2,(float)(Heigth-1)/2,-10); // Posiciona la camara en el centro del mapa
		Camera.main.backgroundColor = Color.black;
		Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Floors/Dark.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
		Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Floors/Light.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
		for (int i =0; i < Width;i++){
			for(int j = 0; j < Heigth;j++){
				Vector2 position = new Vector2(i,j);
				GameObject clone = new GameObject();
				
				if ( ( i+j ) % 2 == 0 ) 
					clone = EditorWindow.Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
				else 
					clone = EditorWindow.Instantiate(prefab2, new Vector3(i,j,0f), Quaternion.identity) as GameObject;
				clone.tag = "RPG-CREATOR";
				EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
			}
		}
		EditorApplication.SaveScene("Assets/Maps/"+Name+".unity");// Guarda la scene.
	}
}
