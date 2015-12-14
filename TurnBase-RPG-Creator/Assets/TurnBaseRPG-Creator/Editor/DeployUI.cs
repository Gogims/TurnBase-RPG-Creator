﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class DeployUI : EditorWindow {
    Object [] Maps;
    private Vector2 scrollPosition;
    private string MapName = string.Empty;
    private GameObject NewGame;
    public void Init() {
        Maps = Resources.LoadAll("Maps", typeof(GameObject));
        NewGame  =  Resources.Load<GameObject>("Menus/FirstScene");
        GameObject aux2 = NewGame as GameObject;
        foreach (Object i in Maps)
        {   
            GameObject aux = i as GameObject;
            if (aux.GetComponent<Map>().Id == aux2.GetComponent<MenuOptionScen>().SceneName)
            {
                MapName = aux.GetComponent<Map>().Name;
                break;
            }

        }
        
    }
    void OnGUI()
    {
        RenderLeft();

        Rect LeftSide = new Rect(this.position.width * 0.5f, 0, this.position.width * 0.5f, this.position.height - 20);
        GUILayout.BeginArea(LeftSide, string.Empty, EditorStyles.helpBox);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mapa:",GUILayout.Width(50));
        GUI.enabled = false;
        MapName = GUILayout.TextField(MapName);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        Texture Logo = Resources.Load<Texture>("LogoPUCMM");
        GUI.DrawTexture(new Rect(LeftSide.width - Logo.width, LeftSide.height, Logo.width, Logo.height),
                        Logo);
    }

    private void RenderLeft()
    {
        GUILayout.BeginArea(new Rect(0, 0, (float)(this.position.width * 0.5), this.position.height - 20), string.Empty, EditorStyles.helpBox);
        GUILayout.BeginVertical();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DrawObjectList();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    /// <summary>
    /// Dibuja los objetos que estan en el arreglo
    /// </summary>
    void DrawObjectList()
    {
        int x = 10;
        int y = 10;
        foreach (var obj in Maps)
        {

            GameObject temp = (GameObject)obj;
            if (temp.tag == "RPG-CORE") continue;
            RPGElement ob = temp.GetComponent<RPGElement>();
            Rect position = new Rect(x, y, 44, 44);
            if (ob.Icon != null)
            {
                GUI.DrawTextureWithTexCoords(position, ob.Icon.texture, Constant.GetTextureCoordinate(ob.Icon));
            }
            if (ob.Name.Length > 6)
                GUI.Label(new Rect(x, y + 54, 44, 20), ob.Name.Substring(0, 4) + "...");
            else
                GUI.Label(new Rect(x, y + 54, 44, 20), ob.Name);
            if (GUI.Button(position, "", new GUIStyle()))
            {

                MapName = temp.GetComponent<Map>().Name;
                GameObject i = Instantiate(NewGame);
                i.GetComponent<MenuOptionScen>().SceneName = temp.GetComponent<Map>().Id;
                i.name = "FirstScene";
                PrefabUtility.CreatePrefab("Assets/Resources/Menus/"+i.name+".prefab", i);
                NewGame = Resources.Load<GameObject>("Menus/FirstScene");
                DestroyImmediate(i);
            }
            if (x + 84 + 64 < this.position.width)
                x += 84;
            else
            {
                GUILayout.Label("", GUILayout.Height(44 + 25), GUILayout.Width(x + 25));
                y += 64;
                x = 10;
            }
        }
    }

    
}
