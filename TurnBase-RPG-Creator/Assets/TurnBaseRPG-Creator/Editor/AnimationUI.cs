using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Rotorz.ReorderableList;

public class AnimationUI : EditorWindow
{
    List<Sprite> spriteList;
    string SpriteName;
    private Vector2 ScrollPosition;

    public AnimationUI() { }

    public void Init(ref List<Sprite> sprites, string nameSprite)
    {
        spriteList = sprites;
        SpriteName = nameSprite;
    }

    void OnGUI()
    {    
        GUILayout.BeginArea(new Rect(0, 0, this.position.width, this.position.height-20));

        if (GUILayout.Button("Add " + SpriteName))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Character", 1);
        }

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

        if (spriteList.Count == 0)
        {
            ReorderableListGUI.ListField(spriteList, DrawSprite, ReorderableListFlags.HideAddButton);
        }
        else
        {
            ReorderableListGUI.ListField(spriteList, DrawSprite, spriteList[0].textureRect.height, ReorderableListFlags.HideAddButton);
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        if (GUI.Button(new Rect(position.width - 100, position.height - 20, 100, 20), "Close"))
        {
            this.Close();
        }

        AddSprite();
    }    

    Sprite DrawSprite(Rect position, Sprite itemValue)
    {
        GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, itemValue.textureRect.width, itemValue.textureRect.height), itemValue.texture,
                                new Rect(itemValue.textureRect.x / itemValue.texture.width,
                                        itemValue.textureRect.y / itemValue.texture.height,
                                        itemValue.textureRect.width / itemValue.texture.width,
                                        itemValue.textureRect.height / itemValue.texture.height)
                                        );
        return itemValue;
    }    

    private void AddSprite()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated" && Event.current.type == EventType.ExecuteCommand)
        {
            Sprite s = new Sprite();
            s = (Sprite)EditorGUIUtility.GetObjectPickerObject();
            spriteList.Add(s);
            Repaint();
        }        
    }
}