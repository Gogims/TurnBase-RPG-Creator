using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MapEditor {

	/// <summary>
	/// Objeto seleccionado.
	/// </summary>
	public static GameObject selectedObject;
    private static GameObject AreaTroopSelector;
	static Object DarkFloor;
	static Object LightFloor;
    static bool deleting = false;
	static MapEditor () {
	
		SceneView.onSceneGUIDelegate += OnSceneEvents;
		DarkFloor =  AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile1.prefab", typeof(GameObject));
        LightFloor = AssetDatabase.LoadAssetAtPath(@"Assets/Resources/Tile/DefaultTile2.prefab", typeof(GameObject));
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
		}
        
        // si el click izquierdo es precionado y el objeto seleccionado es diferente de nulo inserta un objeto al mapa. 
        if (EventType.MouseUp == e.type && e.button == 0 && selectedObject != null && (selectedObject.tag == "RPG-MAPOBJECT" || selectedObject.tag == "RPG-PLAYER" || selectedObject.tag == "RPG-ENEMY"))
        {
             DropObject();
		} 
        
        // Si la tecla del es precionada borra los objetos seleccionados. 
        if (EventType.KeyDown == e.type && !deleting &&  e.keyCode == KeyCode.Delete)
        {
            deleting = true;
            DeleteObject();
            deleting = false;
		}
        
        // En caso de que hagan drag and drop.
		if (EventType.DragExited == e.type) {
			DeleteSelectedObject(Selection.activeGameObject);
		}
        if (EventType.MouseDown == e.type && e.button == 1 )
        {
            Selection.activeGameObject = null;
            selectedObject = null;
            GameEngine.inspectorRpg.Focus();
        }
	}

	/// <summary>
	/// Borra el objeto que este seleccionado.
	/// </summary>
	/// <param name="Selected">Selected.</param>
	static void DeleteSelectedObject(GameObject Selected)
	{
        if (Selected.tag == "MainCamera") return;
		    MapUI.DestroyImmediate (Selected,true);
	}
	/// <summary>
	/// Borra los objeto seleccionados.
	/// </summary>
	static void DeleteObject()
	{
		foreach (GameObject i in Selection.gameObjects) {
            if (i == null) continue;
            if (i.name.Contains("Main Camera") )
            {
                var camera = i;
                GameObject.Instantiate(camera);
                continue;
            }

            SpriteRenderer si = i.GetComponent<SpriteRenderer>();
            if (si.sortingLayerName == Constant.LAYER_TILE)
            {

                GameObject temp = new GameObject();
                if ((i.transform.position.x + i.transform.position.y) % 2 == 0)
                    temp = (GameObject)DarkFloor;
                else
                    temp = (GameObject)LightFloor;
                Sprite aux = temp.GetComponent<SpriteRenderer>().sprite;
                temp.transform.position = i.transform.position;
                temp.transform.localScale = new Vector3(ProjectSettings.pixelPerUnit / aux.rect.width, ProjectSettings.pixelPerUnit / aux.rect.height);
                MapUI.Instantiate(temp, temp.transform.position, Quaternion.identity);
                MapUI.DestroyImmediate(GameObject.Find("New Game Object"), true);
            }
			MapUI.DestroyImmediate (i,true);
			
		}
	}
	/// <summary>
	///  Cambia la seleccion del objeto que se va pintar cuando se le de click a un objeto en la escena
	/// </summary>
	/// <param name="Selected">objeto seleccionado</param>
	static void ChangeSelectedObject(GameObject Selected){
        if (Selected != selectedObject)
        {
            if (AreaTroopSelector != null)
            {
                Object.DestroyImmediate(AreaTroopSelector);
            }

            if (Selected.tag == "RPG-MAPOBJECT" && !Selected.activeInHierarchy)
            {
                selectedObject = Selected;
                GameEngine.inspectorRpg.Focus();
            }

            if (Selected.tag == "RPG-ENEMY" && Selected.activeInHierarchy)
            {
                selectedObject = Selected;
                var SceneWindow = SceneView.lastActiveSceneView;
                GameEngine.inspectorRpg.Focus();
                SceneWindow.Focus();

                if (RPGInspectorUI.SelectionMode)
                {
                    AreaTroopSelector = new GameObject("Selector");
                    var sprite = AreaTroopSelector.AddComponent<SpriteRenderer>();
                    sprite.sprite = Resources.Load<Sprite>("Selector");
                    sprite.sortingLayerName = "Selector";
                    AreaTroopSelector.transform.position = Selected.gameObject.GetComponent<Troop>().transform.position;
                }
            }
        }        

        if (AreaTroopSelector != null && selectedObject.gameObject.GetComponent<Troop>() != null)
        {
            Troop troop = selectedObject.GetComponent<Troop>();
            var selector = AreaTroopSelector.GetComponent<SpriteRenderer>();
            float EnemyAreaWidth = troop.AreaWidth < 0 ? 0 : 3.125f * troop.AreaWidth * 2;
            float EnemyAreaHeight = troop.AreaHeight < 0 ? 0 : 3.125f * troop.AreaHeight * 2;

            selector.transform.localScale = new Vector3(EnemyAreaWidth+3.125f, EnemyAreaHeight + 3.125f, 1);

        }

    }
	/// <summary>
	/// Pinta el objeto seleccionado en la scene.
	/// </summary>
	static void DropObject()
	{
        GameObject temp = selectedObject;
        SpriteRenderer selectSprite = temp.GetComponent<SpriteRenderer>();
		foreach (GameObject i in Selection.gameObjects) {
            if (i == null) continue;
            SpriteRenderer iSprite = i.GetComponent<SpriteRenderer>();
            if (i.name == "Main Camera" || iSprite.sprite.name == selectSprite.sprite.name) continue;
            if (selectSprite.sortingLayerName == Constant.LAYER_TILE)
            {
                if (i.transform.parent == null && i.tag != "RPG-PLAYER")
                {
                    temp.transform.position = i.transform.position;
                    MapUI.DestroyImmediate(i, true);
                }
                else if (i.tag != "RPG-PLAYER")
                {
                    temp.transform.position = i.transform.parent.gameObject.transform.position;
                    MapUI.DestroyImmediate(i.transform.parent.gameObject, true);
                }
                temp.transform.localScale = new Vector3(ProjectSettings.pixelPerUnit / selectSprite.sprite.rect.width, ProjectSettings.pixelPerUnit / selectSprite.sprite.rect.height);
                MapUI.Instantiate(temp, temp.transform.position, Quaternion.identity);
                
            }
            else if (temp.tag == "RPG-PLAYER")
            {
                if (GameObject.Find("PLAYER(Clone)") != null)
                    continue;

                temp.name = "PLAYER";
                temp.transform.position = i.transform.position;
                temp.transform.localScale = new Vector3(ProjectSettings.pixelPerUnit / selectSprite.sprite.rect.width, ProjectSettings.pixelPerUnit / selectSprite.sprite.rect.height);
                MapUI.Instantiate(temp, temp.transform.position, Quaternion.identity);
            }
            else
            {
                GameObject aux = null;
                if (i.transform.parent == null && i.transform.childCount != 0)
                {
                    if (i.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite.name == selectSprite.sprite.name)
                    {
                        continue;
                    }

                    aux = i;
                    MapUI.DestroyImmediate(i.transform.GetChild(0).gameObject);
                }
                else if (i.transform.parent != null)
                {
                    aux = i.transform.parent.gameObject;
                    MapUI.DestroyImmediate(i);
                }
                else
                {
                    aux = i;
                }
                var inst = MapUI.Instantiate(temp, new Vector3(0, 0), Quaternion.identity);
                temp = inst as GameObject;
                temp.transform.parent = aux.transform;
                temp.transform.localPosition = new Vector3(0, 0);
                var sprite = aux.GetComponent<SpriteRenderer>().sprite;
                var sprite2 = temp.GetComponent<SpriteRenderer>().sprite;
                temp.transform.localScale = new Vector3(sprite.rect.width / sprite2.rect.width, sprite.rect.height / sprite2.rect.height);

            }
			
			MapUI.DestroyImmediate (GameObject.Find("New Game Object"),true);

		}
        Selection.activeGameObject = null;		                          
	}
}
