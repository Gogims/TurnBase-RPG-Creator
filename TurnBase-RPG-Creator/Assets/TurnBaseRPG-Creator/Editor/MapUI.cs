using UnityEngine;
using System.Collections;
using UnityEditor;

public class MapUI : EditorWindow {

	Map map;
    ErrorHandler err;
    int Heigth = 10;
    int Width = 10;
    int name = 8;
	public void Init () {
		map = new Map ();

		map.Heigth = 10;
		map.Width=10;
        map.Name = "new_map1";
        err = new ErrorHandler();
        err.InsertPropertyError("Heigth", map.Heigth, "The Heigth has to be greater than 10 and less than 20");
        err.InsertPropertyError("Width", map.Width, "The Width has to be greater than 10 and less than 20");
        err.InsertPropertyError("Name", map.Name.Length, "The length of the name hast o be grater than 5");
        err.InsertCondition("Heigth", Constatnt.MIN_MAP_HEIGTH, ErrorCondition.GreaterOrEqual, LogicalCondition.AND);
        err.InsertCondition("Heigth", Constatnt.MAX_MAP_HEIGTH, ErrorCondition.LessOrEqual, LogicalCondition.None);
        err.InsertCondition("Width", Constatnt.MIN_MAP_WIDTH, ErrorCondition.GreaterOrEqual, LogicalCondition.AND);
        err.InsertCondition("Width", Constatnt.MAX_MAP_WIDTH, ErrorCondition.LessOrEqual, LogicalCondition.None);
        err.InsertCondition("Name", 5, ErrorCondition.GreaterOrEqual, LogicalCondition.None);
       
	}
	void OnGUI () {
		if (map == null || err == null)
			this.Init ();
        err.ShowErrors();      
		GUILayout.Label ("Settings", EditorStyles.boldLabel);
		map.Name = EditorGUILayout.TextField ("Map Name",map.Name);
		map.Width = EditorGUILayout.IntField ("Width", map.Width);
		map.Heigth = EditorGUILayout.IntField ("Heigth", map.Heigth);
        UpdateValidationVal();
		if (GUILayout.Button ("Create Map") && !err.CheckErrors()) {
			map.CreateMap();
		}
	}
	void onDestroy(){
		this.Close ();
	}
    void UpdateValidationVal()
    {
        err.UpdateValue("Heigth", map.Heigth);
        err.UpdateValue("Width", map.Width);
        err.UpdateValue("Name", map.Name.Length);
    }
}
