using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Representa un Mapa en RPG.
/// </summary>
public class Map : RPGElement{
    public int Heigth;
    /// <summary>
    /// Representa el ancho del mapa.
    /// </summary>
    public int Width;
	/// <summary>
	/// Crea un mapa dado su ancho, alto y nombre.
	/// </summary>
	public string CreateMap(){
		EditorApplication.NewScene(); // Crea una scene nueva.
		Camera.main.orthographicSize = 5; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)
		Camera.main.transform.localPosition = new Vector3((float)(Width-1)/2,(float)(Heigth-1)/2,-10); // Posiciona la camara en el centro del mapa
		Camera.main.backgroundColor = Color.black;
        UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
        UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
		for (int i =0; i < Width;i++){
			for(int j = 0; j < Heigth;j++){
				Vector2 position = new Vector2(i,j);
				GameObject clone = new GameObject();
				
				if ( ( i+j ) % 2 == 0 ) 
					clone = EditorWindow.Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
				else 
					clone = EditorWindow.Instantiate(prefab2, new Vector3(i,j,0f), Quaternion.identity) as GameObject;
				clone.tag = "RPG-CORE";
				EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
			}
		}
        GameObject settings = new GameObject("Settings");
        settings.AddComponent<Map>();
        Map x = settings.GetComponent<Map>();
        x.Name = this.Name;
        x.Width = this.Width;
        x.Heigth = this.Heigth;
        x.Icon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TurnBaseRPG-Creator/RPG-Sprites/MapIcon.png");
        string returnPath = "Assets/Maps/" + Guid.NewGuid() + ".unity";
		EditorApplication.SaveScene("Assets/Maps/"+Guid.NewGuid()+".unity");// Guarda la scene.
        return Directory.GetCurrentDirectory() + '\\' + returnPath.Replace('/', '\\');
	}
    /// <summary>
    /// Actualiza un mapa dado su path
    /// </summary>
    /// <param name="Path">Path del mapa (scene)</param>
    public void updateMap(string Path)
    {
        GameObject obj = GameObject.Find("Settings");
        Map aux = obj.GetComponent<Map>();

        GameObject [] List1 = GameObject.FindGameObjectsWithTag("RPG-MAPOBJECT");
        GameObject[] List2 = GameObject.FindGameObjectsWithTag("RPG-CORE");
        if (this.Width < aux.Width || this.Heigth < aux.Heigth)
        {
            if (this.Width < aux.Width)
                aux.Width = this.Width;
            if (this.Heigth < aux.Heigth)
                aux.Heigth = this.Heigth;
            foreach (var i in List1)
            {
                Transform position = i.GetComponent<Transform>();
                if ((position.localPosition.x > this.Width - 1) || position.localPosition.y > this.Heigth - 1)
                    DestroyImmediate(i);
            }
            foreach (var i in List2)
            {
                Transform position = i.GetComponent<Transform>();
                if (position.localPosition.x > this.Width - 1 || position.localPosition.y > this.Heigth - 1)
                    DestroyImmediate(i);
            }
        }

        if (this.Width > aux.Width)
        {
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            int x = aux.Width;
            for (int i = x; i < this.Width; i++)
                for (int j = 0; j < aux.Heigth; j++)
                {
                    Vector2 position = new Vector2(i, j);
                    GameObject clone = new GameObject();

                    if ((i + j) % 2 == 0)
                        clone = EditorWindow.Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
                    else
                        clone = EditorWindow.Instantiate(prefab2, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    clone.tag = "RPG-CORE";
                    EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
                }
            aux.Width = this.Width;
        }
        if (this.Heigth > aux.Heigth)
        {
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            int y = aux.Heigth;
            for (int i = 0; i < aux.Width; i++)
                for (int j = y; j < this.Heigth; j++)
                {
                    Vector2 position = new Vector2(i, j);
                    GameObject clone = new GameObject();

                    if ((i + j) % 2 == 0)
                        clone = EditorWindow.Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
                    else
                        clone = EditorWindow.Instantiate(prefab2, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                    clone.tag = "RPG-CORE";
                    EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
                }
        }
        if (this.Heigth > aux.Heigth && this.Width > aux.Width)
        {

            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            Vector2 position = new Vector2(this.Heigth - 1, this.Width - 1);
            GameObject clone = new GameObject();

            if ((this.Width - 1 + this.Heigth - 1) % 2 == 0)
                clone = EditorWindow.Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
            else
                clone = EditorWindow.Instantiate(prefab2, position, Quaternion.identity) as GameObject;
            clone.tag = "RPG-CORE";
            EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
        }
        EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
        aux.Name = this.Name;
        aux.Width = this.Width;
        aux.Heigth = this.Heigth;
        EditorApplication.SaveScene();


    }
}
