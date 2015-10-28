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
		map.Name = "new_map1";
	}
	void OnGUI () {
		if (map == null)
			this.Init ();
		GUILayout.Label ("Settings", EditorStyles.boldLabel);
		if (map.Name.Length == 0 )
			map.Name = EditorGUILayout.TextField ("Map Name","new_map1");
		else 
			map.Name = EditorGUILayout.TextField ("Map Name",map.Name);

		if (map.Width > Constatnt.MAX_MAP_WIDTH)
			map.Width = EditorGUILayout.IntField ("Width",Constatnt.MAX_MAP_WIDTH);
		else if (map.Width < Constatnt.MIN_MAP_WIDTH )
			map.Width = EditorGUILayout.IntField ("Heigth",Constatnt.MIN_MAP_WIDTH );
		else
			map.Width = EditorGUILayout.IntField ("Width", map.Width);

		if ( map.Heigth > Constatnt.MAX_MAP_HEIGTH) 
			map.Heigth = EditorGUILayout.IntField ("Heigth", Constatnt.MAX_MAP_HEIGTH);
		else if ( map.Heigth < Constatnt.MIN_MAP_HEIGTH) 
			map.Heigth = EditorGUILayout.IntField ("Heigth", Constatnt.MIN_MAP_HEIGTH);
		else 
			map.Heigth = EditorGUILayout.IntField ("Heigth", map.Heigth);

		if (GUILayout.Button ("Create Map")) {
			map.CreateMap();
		}
	}
	void onDestroy(){
		this.Close ();
	}
}
