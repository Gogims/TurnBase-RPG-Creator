using UnityEngine;
using UnityEditor;
using System.Linq;

public class PlayerUI: CRUD
{
    Texture2D txt;
    Sprite[] sprites;

    public PlayerUI() : base(@"\Assets\Resources\Player\") { }

    public void Init()
    {

    }

    void OnGUI()
	{
        //Down Sprite
        GUILayout.BeginArea(new Rect(0, 0, 500, 300));                
        if (GUILayout.Button("Down Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);            
        }
        GUILayout.EndArea();

        //Left Sprite
        GUILayout.BeginArea(new Rect(0, 0, 500, 300));
        if (GUILayout.Button("Left Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 2);
        }
        GUILayout.EndArea();

        //Up Sprite
        GUILayout.BeginArea(new Rect(0, 0, 500, 300));
        if (GUILayout.Button("Up Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 3);
        }
        GUILayout.EndArea();

        //Right Sprite
        GUILayout.BeginArea(new Rect(0, 0, 500, 300));
        if (GUILayout.Button("Right Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 4);
        }
        GUILayout.EndArea();

        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            if (EditorGUIUtility.GetObjectPickerControlID() == 1)
            {
                
            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 2)
            {

            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 3)
            {

            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 4)
            {

            }

            txt = (Texture2D)EditorGUIUtility.GetObjectPickerObject();
            string spritesheet = AssetDatabase.GetAssetPath(txt);
            sprites = AssetDatabase.LoadAllAssetsAtPath(spritesheet).OfType<Sprite>().ToArray();

            if (sprites != null && sprites.Length > 0)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    GUI.DrawTextureWithTexCoords(new Rect(i * 100, 280, sprites[i].textureRect.width, sprites[i].textureRect.height), sprites[i].texture,
                        new Rect(sprites[i].textureRect.x / sprites[i].texture.width,
                            sprites[i].textureRect.y / sprites[i].texture.height,
                            sprites[i].textureRect.width / sprites[i].texture.width,
                            sprites[i].textureRect.height / sprites[i].texture.height)
                            );
                }
            }
        }        
    }

    void Update()
    {
        //Seleccionar el Sprite
        if (txt != null)
        {
            
        }
    }
}