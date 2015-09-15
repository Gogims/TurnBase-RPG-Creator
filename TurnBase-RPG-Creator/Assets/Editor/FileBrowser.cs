using System;
using UnityEditor;
using System.IO;

public class FileBrowser
{
	public string path{
		get{
			return _path;
		}
		
		set{
			_path = value;			

			if (value != null && value != "") {
				image = File.ReadAllBytes(value);
			}
		}
	}

	private byte[] image;
	private string _path;

	public byte[] GetImage{
		get{
			return image;
		}
	}

	public FileBrowser (){
	}

	public FileBrowser (string _path, string _text)
	{
		path = _path;
		EditorGUILayout.TextField (_text);

	}
}