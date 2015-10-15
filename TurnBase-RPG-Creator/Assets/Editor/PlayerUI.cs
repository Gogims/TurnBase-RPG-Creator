using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using Rotorz.ReorderableList;

public class PlayerUI: CRUD
{
    Sprite s;
    List<Sprite> downSprites;
    List<Sprite> leftSprites;
    List<Sprite> upSprites;
    List<Sprite> rightSprites;
    private Vector2 _downScrollPosition;
    private Vector2 _leftScrollPosition;
    private Vector2 _upScrollPosition;
    private Vector2 _rightScrollPosition;

    public PlayerUI() : base(@"\Assets\Resources\Player\") { }

    public void Init()
    {
        downSprites = leftSprites = upSprites = rightSprites = new List<Sprite>();
        s = new Sprite();
    }

    void OnGUI()
	{
        //Down Sprite        
        GUILayout.BeginArea(new Rect(0, 0, this.position.width, this.position.height/4));

            if (GUILayout.Button("Down Sprites"))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
            }

            _downScrollPosition = GUILayout.BeginScrollView(_downScrollPosition);

            if (s == null)
            {
                ReorderableListGUI.ListField(downSprites, DrawSprite, ReorderableListFlags.HideAddButton);
            }
            else
            {
                ReorderableListGUI.ListField(downSprites, DrawSprite, s.textureRect.height, ReorderableListFlags.HideAddButton);
            }
            GUILayout.EndScrollView();

        GUILayout.EndArea();

        //Left Sprite
        GUILayout.BeginArea(new Rect(0, this.position.height / 4, this.position.width, this.position.height / 4));
        if (GUILayout.Button("Left Sprites"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 2);            
        }

        _leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition);

        if (s == null)
        {
            ReorderableListGUI.ListField(leftSprites, DrawSprite, ReorderableListFlags.HideAddButton);
        }
        else
        {
            ReorderableListGUI.ListField(leftSprites, DrawSprite, s.textureRect.height, ReorderableListFlags.HideAddButton);
        }
        GUILayout.EndScrollView();

        GUILayout.EndArea();

        //Up Sprite
        GUILayout.BeginArea(new Rect(0, this.position.height / 2, this.position.width, this.position.height / 4));
            if (GUILayout.Button("Up Sprites"))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 3);
            }

            _upScrollPosition = GUILayout.BeginScrollView(_upScrollPosition);

            if (s == null)
            {
                ReorderableListGUI.ListField(upSprites, DrawSprite, ReorderableListFlags.HideAddButton);
            }
            else
            {
                ReorderableListGUI.ListField(upSprites, DrawSprite, s.textureRect.height, ReorderableListFlags.HideAddButton);
            }
            GUILayout.EndScrollView();
        GUILayout.EndArea();

        //Right Sprite
        GUILayout.BeginArea(new Rect(0, this.position.height * 3 / 4, this.position.width, this.position.height / 4));
            if (GUILayout.Button("Right Sprites"))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 4);
            }

            _rightScrollPosition = GUILayout.BeginScrollView(_rightScrollPosition);

            if (s == null)
            {
                ReorderableListGUI.ListField(rightSprites, DrawSprite, ReorderableListFlags.HideAddButton);
            }
            else
            {
                ReorderableListGUI.ListField(rightSprites, DrawSprite, s.textureRect.height, ReorderableListFlags.HideAddButton);
            }
            GUILayout.EndScrollView();
        GUILayout.EndArea();

        AddSprite();
    }

    void Update()
    {
        

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

    void AddSprite()
    {
        if (Event.current.commandName == "ObjectSelectorUpdated" && Event.current.type == EventType.ExecuteCommand)
        {
            s = new Sprite();
            s = (Sprite)EditorGUIUtility.GetObjectPickerObject();

            if (EditorGUIUtility.GetObjectPickerControlID() == 1)
            {
                downSprites.Add(s);
            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 2)
            {                
                leftSprites.Add(s);
            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 3)
            {
                upSprites.Add(s);
            }
            else if (EditorGUIUtility.GetObjectPickerControlID() == 4)
            {
                rightSprites.Add(s);
            }
        }
    }
}