using UnityEngine;
using System.Collections;

public class RpgObject  {
	Texture2D texture;
	public string Tag { set; get;}
	public string name { set; get; }
	public int type { set; get; } 	
	public  static string[] ObjectTypes = {"Tiles","Wall"};	

}
