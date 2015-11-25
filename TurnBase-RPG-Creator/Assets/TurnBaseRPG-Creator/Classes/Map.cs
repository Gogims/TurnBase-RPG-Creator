using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Representa un Mapa en RPG.
/// </summary>
public class Map : RPGElement{
    public Map() {
        Data = new AbstractMap();
    }
    public AbstractMap Data;
	/// <summary>
	/// Crea un mapa dado su ancho, alto y nombre.
	/// </summary>
	public void CreateMap(){
		EditorApplication.NewScene(); // Crea una scene nueva.
		Camera.main.orthographicSize = 5; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)
        Camera.main.transform.localPosition = new Vector3((float)(Data.Width - 1) / 2, (float)(Data.Heigth - 1) / 2, -10); // Posiciona la camara en el centro del mapa
		Camera.main.backgroundColor = Color.black;
        UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
        UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
        for (int i = 0; i < Data.Width; i++)
        {
            for (int j = 0; j < Data.Heigth; j++)
            {
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
        x.Name = x.Data.Name = this.Data.Name;
        x.Data.Width = this.Data.Width;
        x.Data.Heigth = this.Data.Heigth;
        x.Icon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TurnBaseRPG-Creator/RPG-Sprites/MapIcon.png");
        x.Id = Guid.NewGuid().ToString();
        string returnPath = "Assets/Resources/Maps/" + Guid.NewGuid() + ".unity";
        x.Data.MapPath = Directory.GetCurrentDirectory() + '\\' + returnPath.Replace('/', '\\');
        PrefabUtility.CreatePrefab("Assets/Resources/Maps/" + x.Id + ".prefab", x.gameObject);
        DestroyImmediate(settings);
        EditorApplication.SaveScene(returnPath);// Guarda la scene.
	}
    /// <summary>
    /// Actualiza un mapa dado su path
    /// </summary>
    /// <param name="Path">Path del mapa (scene)</param>
    public void updateMap(Map aux)
    {

        GameObject [] List1 = GameObject.FindGameObjectsWithTag("RPG-MAPOBJECT");
        GameObject[] List2 = GameObject.FindGameObjectsWithTag("RPG-CORE");

        if (this.Data.Width < aux.Data.Width || this.Data.Heigth < aux.Data.Heigth)
        {
            if (this.Data.Width < aux.Data.Width)
                aux.Data.Width = this.Data.Width;
            if (this.Data.Heigth < aux.Data.Heigth)
                aux.Data.Heigth = this.Data.Heigth;
            foreach (var i in List1)
            {
                Transform position = i.GetComponent<Transform>();
                if ((position.localPosition.x > this.Data.Width - 1) || position.localPosition.y > this.Data.Heigth - 1)
                    DestroyImmediate(i);
            }
            foreach (var i in List2)
            {
                Transform position = i.GetComponent<Transform>();
                if (position.localPosition.x > this.Data.Width - 1 || position.localPosition.y > this.Data.Heigth - 1)
                    DestroyImmediate(i);
            }
        }

        if (this.Data.Width > aux.Data.Width)
        {
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            int x = aux.Data.Width;
            for (int i = x; i < this.Data.Width; i++)
                for (int j = 0; j < aux.Data.Heigth; j++)
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
            aux.Data.Width = this.Data.Width;
        }
        if (this.Data.Heigth > aux.Data.Heigth)
        {
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            int y = aux.Data.Heigth;
            for (int i = 0; i < aux.Data.Width; i++)
                for (int j = y; j < this.Data.Heigth; j++)
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
        if (this.Data.Heigth > aux.Data.Heigth && this.Data.Width > aux.Data.Width)
        {

            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
            Vector2 position = new Vector2(this.Data.Heigth - 1, this.Data.Width - 1);
            GameObject clone = new GameObject();

            if ((this.Data.Width - 1 + this.Data.Heigth - 1) % 2 == 0)
                clone = EditorWindow.Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
            else
                clone = EditorWindow.Instantiate(prefab2, position, Quaternion.identity) as GameObject;
            clone.tag = "RPG-CORE";
            EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
        }
        EditorWindow.DestroyImmediate(GameObject.Find("New Game Object"));
        Camera.main.orthographicSize = 5; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)
        Camera.main.transform.localPosition = new Vector3((float)(this.Data.Width - 1) / 2, (float)(this.Data.Heigth - 1) / 2, -10); // Posiciona la camara en el centro del mapa

        EditorApplication.SaveScene();


    }
}
[Serializable]
public class AbstractMap
{
    /// <summary>
    /// Almacena el path de la scene.
    /// </summary>
    public string MapPath = string.Empty;
    /// <summary>
    /// Almacena la altura del mapa
    /// </summary>
    public int Heigth;
    /// <summary>
    /// Almacena el ancho del mapa.
    /// </summary>
    public int Width;
    /// <summary>
    /// Nombre del mapa.
    /// </summary>
    public string Name = string.Empty;
}
