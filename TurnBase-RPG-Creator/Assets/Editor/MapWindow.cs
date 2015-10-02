using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapWindow : EditorWindow {

	Map map; 
	// Add menu named "My Window" to the Window menu
	public void Init () {
		map = new Map ();
		map.Heigth = 10;
		map.Width=10;
		map.Name = "new_mapa1";
	}
	void OnGUI () {
		if (map == null)
			this.Init ();
		GUILayout.Label ("Ajustes Basicos", EditorStyles.boldLabel);
		map.Name = EditorGUILayout.TextField ("Nombre Del Mapa",map.Name);
		map.Width = EditorGUILayout.IntField ("Ancho", map.Width);
		map.Heigth = EditorGUILayout.IntField ("Alto", map.Heigth);
		if (GUILayout.Button ("Crear Mapa")) {
			CreateMap();
		}
	}
	/// <summary>
	/// Crea un mapa dado su ancho, alto y nombre.
	/// </summary>
	void CreateMap(){
		EditorApplication.NewScene(); // Crea una scene nueva.
		Camera.main.orthographicSize = 5; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)
		Camera.main.transform.localPosition = new Vector3((float)(map.Width-1)/2,(float)(map.Heigth-1)/2,-10); // Posiciona la camara en el centro del mapa
		Camera.main.backgroundColor = Color.black;
		Object prefab = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Floors/Floor1.prefab", typeof(GameObject)); // Carga el prefab que esta por defecto para crear el mapa
		for (int i =0; i < map.Width;i++){
			for(int j = 0; j < map.Heigth;j++){
				Vector2 position = new Vector2(i,j);
				GameObject clone =  Instantiate(prefab, position, Quaternion.identity) as GameObject; // Agrega un objeto nuevo a la scene.
				clone.transform.localScale= new Vector2((float)3.049475,(float)3.070369); // Ajusta el scale del objeto.
			}
		}
		EditorApplication.SaveScene("Assets/Maps/"+map.Name+".unity");// Guarda la scene.
	}
	void onDestroy(){
		this.Close ();
	}
}
