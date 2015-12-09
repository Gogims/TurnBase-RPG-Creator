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
				file = System.IO.File.ReadAllBytes(value);
			}
		}
	}

	private byte[] file;
	private string _path = string.Empty;

	public byte[] File{
		get{
			return file;
		}
	}

	public FileBrowser (){
	}

	public FileBrowser (string _path)
	{
		path = _path;
	}
}