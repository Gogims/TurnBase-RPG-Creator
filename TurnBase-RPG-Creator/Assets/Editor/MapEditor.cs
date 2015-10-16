using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class MapEditor {

	/// <summary>
	/// Objeto seleccionado.
	/// </summary>
	public static GameObject selectedObject;
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
		//Revisa si el objeto seleccionado es nulo.
		if (Selection.activeGameObject != null) {
			ChangeSelectedObject (Selection.activeGameObject);
		}// si el click izquierdo es precionado y el objeto seleccionado es diferente de nulo inserta un objeto al mapa. 
		if (EventType.MouseDown == e.type && e.button == 0 && selectedObject != null) {
			DropObject ();
		} // Si la tecla del  es precionada borra los objetos seleccionados. 
		if (EventType.KeyDown == e.type ) {
			DeleteObject();
		}// En caso de que hagan drag and drop.
		if (EventType.DragExited == e.type) {
			DeleteSelectedObject(Selection.activeGameObject);
		}

	}
	/// <summary>
	/// Borra el objeto que este seleccionado.
	/// </summary>
	/// <param name="Selected">Selected.</param>
	static void DeleteSelectedObject(GameObject Selected)
	{
		MapWindow.DestroyImmediate (Selected);
	}
	/// <summary>
	/// Borra los objeto seleccionados.
	/// </summary>
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
			selectedObject = Selected;
			GameEngine.inspectorRpg.Focus();
		}

	}
	/// <summary>
	/// Pinta el objeto seleccionado en la scene.
	/// </summary>
	static void DropObject()
	{

		foreach (GameObject i in Selection.gameObjects) {
			GameObject temp = selectedObject;
			temp.transform.position = i.transform.position;
			MapWindow.DestroyImmediate (i);
			MapWindow.Instantiate (temp, temp.transform.position, Quaternion.identity);
			
			MapWindow.DestroyImmediate (GameObject.Find("New Game Object"));

		}
		                          
	}
}
