using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using Rotorz.ReorderableList;

public class PlayerUI : EditorWindow
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
    private float listHeight;

    public PlayerUI() { }

    public void Init()
    {
        downSprites = new List<Sprite>();
        leftSprites = new List<Sprite>();
        upSprites = new List<Sprite>();
        rightSprites = new List<Sprite>();
    }

    void OnGUI()
    {
        listHeight = position.height / 4;

        //Down Sprite        
        GUILayout.BeginArea(new Rect(0, 0, this.position.width, listHeight));

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
        GUILayout.BeginArea(new Rect(0, listHeight, this.position.width, listHeight));
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
        GUILayout.BeginArea(new Rect(0, 2 * listHeight, this.position.width, listHeight));
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
        GUILayout.BeginArea(new Rect(0, 3 * listHeight, this.position.width, listHeight));
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

        if (GUILayout.Button("Save"))
        {
            ActorAnimation animation = new ActorAnimation();
            List<Sprite> sprites;

            if (downSprites.Count > 0)
            {
                sprites = new List<Sprite>(downSprites);
                sprites.Add(downSprites[0]);
                animation.down = ActorAnimation.ConstructAnimation(sprites, "test", "down", 30, true);
                animation.downIdle = ActorAnimation.ConstructAnimation(downSprites[0], "test", "downIdle", 30, true);
            }

            if (leftSprites.Count > 0)
            {
                sprites = new List<Sprite>(leftSprites);
                sprites.Add(leftSprites[0]);
                animation.left = ActorAnimation.ConstructAnimation(sprites, "test", "left", 30, true);
                animation.leftIdle = ActorAnimation.ConstructAnimation(leftSprites[0], "test", "leftIdle", 30, true);
            }

            if (upSprites.Count > 0)
            {
                sprites = new List<Sprite>(upSprites);
                sprites.Add(upSprites[0]);
                animation.up = ActorAnimation.ConstructAnimation(sprites, "test", "up", 30, true);
                animation.upIdle = ActorAnimation.ConstructAnimation(upSprites[0], "test", "upIdle", 30, true);
            }

            if (rightSprites.Count > 0)
            {
                sprites = new List<Sprite>(rightSprites);
                sprites.Add(rightSprites[0]);
                animation.right = ActorAnimation.ConstructAnimation(sprites, "test", "right", 30, true);
                animation.rightIdle = ActorAnimation.ConstructAnimation(rightSprites[0], "test", "rightIdle", 30, true);
            }

            animation.ConstructAnimationControl("test");
        }
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