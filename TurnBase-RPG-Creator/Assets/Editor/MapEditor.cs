using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class MapEditor {

	static GameObject floorObject;
	static Object DarkFloor; 
	static Object LightFloor;
	static MapEditor () {
		SceneView.onSceneGUIDelegate += OnSceneEvents;
		DarkFloor =  AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Floors/Dark.prefab", typeof(GameObject));
		LightFloor =  AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Floors/Light.prefab", typeof(GameObject));
	}
	/// <summary>
	/// Funcion que se llama cada vez que ocurre algun evento en la escena actual.
	/// </summary>
	/// <param name="sceneview">Escena.</param>
	static void OnSceneEvents(SceneView sceneview)
	{

		Event e = Event.current;
		if (Selection.activeGameObject != null) {

			ChangeSelectedObject (Selection.activeGameObject);
		} if (EventType.MouseDown == e.type && e.button == 0 && floorObject != null) {
			DropObject ();
		} if (EventType.KeyDown == e.type ) {
			DeleteObject();
		}

	}
	static void DeleteObject()
	{
		foreach (GameObject i in Selection.gameObjects) {
			GameObject temp = new GameObject();
			if ( (i.transform.position.x+i.transform.position.y)%2 == 0) 
				 temp = (GameObject)DarkFloor ;
			else 
				temp = (GameObject)LightFloor;
			temp.transform.position = i.transform.position;
			MapWindow.DestroyImmediate (i);
			MapWindow.Instantiate (temp, temp.transform.position, Quaternion.identity);
			
			MapWindow.DestroyImmediate (GameObject.Find("New Game Object"));
		}
	}
	/// <summary>
	///  Cambia la seleccion del objeto que se va pintar cuando se le de click a un objeto en la scene
	/// </summary>
	/// <param name="Selected">objeto seleccionado</param>
	static void ChangeSelectedObject(GameObject Selected){
		if (Selected.tag == "Floor" && !Selected.activeInHierarchy) {
			floorObject = Selected;
		}

	}
	/// <summary>
	/// Pinta el objeto seleccionado en la scene.
	/// </summary>
	static void DropObject()
	{

		foreach (GameObject i in Selection.gameObjects) {
			GameObject temp = floorObject;
			temp.transform.position = i.transform.position;
			MapWindow.DestroyImmediate (i);
			MapWindow.Instantiate (temp, temp.transform.position, Quaternion.identity);
			
			MapWindow.DestroyImmediate (GameObject.Find("New Game Object"));

		}
		                          
	}
}
