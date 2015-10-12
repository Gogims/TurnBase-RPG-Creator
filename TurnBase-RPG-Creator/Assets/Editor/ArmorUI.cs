using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ArmorUI : CRUD
{
    const int width = 96;
    const int height = 96;
    Armor Armor;
    ListBox listArmors;
    int prevId = -1;    

    public ArmorUI() 
        : base(@"\Assets\Resources\Armor\")
    {

    }

    public void Init()
    {
        Armor = new Armor(width, height);        
        ListObjects = (Resources.LoadAll("Armor", typeof(GameObject)));
        Creating = true;
        listArmors = new ListBox(new Rect(0, 0, 300, 380), new Rect(0, 0, 285, 400), false, true);
        spritename = "";

        for (int i = 0; i < ListObjects.Length; i++)
        {
            elementObject = (GameObject)Instantiate(ListObjects[i]);
            Armor temp = elementObject.GetComponent<Armor>();
            listArmors.AddItem(temp.Name, temp.Id);
            DestroyImmediate(elementObject);
        }

        GetId();
    }

    void OnGUI()
    {
        //Left side area
        GUILayout.BeginArea(new Rect(0, 0, 300, 400), "Armors", EditorStyles.helpBox);
        GUILayout.Space(10);

        if (listArmors.ReDraw())
        {
            UpdateListBox();
        }

        CreateButton = GUI.Button(new Rect(0, 380, 100, 20), "Create");

        GUILayout.EndArea();

        //Right side area        
        GUILayout.BeginArea(new Rect(300, 0, 600, 400), "Basic Settings", EditorStyles.helpBox);

        GUILayout.Space(10);

        Armor.Name = EditorGUILayout.TextField("Name", Armor.Name);
        Armor.Description = EditorGUILayout.TextField("Description", Armor.Description);

        Armor.ArmorType = EditorGUILayout.Popup(Armor.ArmorType, Armor.ArmorTypes());               

        //Attributes stats
        GUILayout.Label("Attributes", EditorStyles.boldLabel);

        Armor.Stats.Agility = EditorGUILayout.IntField("Agility: ", Armor.Stats.Agility);
        Armor.Stats.Defense = EditorGUILayout.IntField("Defense: ", Armor.Stats.Defense);
        Armor.Stats.Luck = EditorGUILayout.IntField("Luck: ", Armor.Stats.Luck);
        Armor.Stats.Magic = EditorGUILayout.IntField("Magic: ", Armor.Stats.Magic);
        Armor.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", Armor.Stats.MagicDefense);
        Armor.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", Armor.Stats.MaxHP);
        Armor.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", Armor.Stats.MaxMP);

        // Text field to upload image
        GUI.enabled = false;
        GUI.TextField(new Rect(0, 240, 300, 20), spritename);
        GUI.enabled = true;

        // Button to upload image
        if (GUI.Button(new Rect(300, 240, 100, 20), "Select Sprite"))
        {
            ObjectSelectorWrapper.ShowSelector(typeof(Sprite));
        }

        if (Armor.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 240, Armor.Image.textureRect.width, Armor.Image.textureRect.height), Armor.Image.texture, Armor.GetTextureCoordinate()); 
        }

        SaveButton = GUI.Button(new Rect(0, 380, 100, 20), "Save");
        GUI.enabled = !Creating;
        DeleteButton = GUI.Button(new Rect(100, 380, 100, 20), "Delete");
        GUI.enabled = true;
               

        GUILayout.EndArea();
    }

    void UpdateListBox()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject);
        }

        elementObject = (GameObject)Instantiate(ListObjects[listArmors.GetSelectedIndex()]);
        Armor = elementObject.GetComponent<Armor>();

        prevId = listArmors.GetSelectedID();
        Creating = false;
    }

    void Update()
    {
        //Si se crea una nueva arma
        if (CreateButton)
        {
            Armor = new Armor(width, height);
            Creating = true;
        }

        //Funcionamineto de guardado
        if (SaveButton)
        {
            SaveArmor();
            Armor = new Armor(width, height);
        }

        //Si se elimina un arma
        if (DeleteButton)
        {
            Delete();
            ListObjects = (Resources.LoadAll("Armor", typeof(GameObject)));
            Armor = new Armor(width, height);
        }        

        //Seleccionar el Sprite
        if (ObjectSelectorWrapper.isVisible)
        {
            Armor.Image = ObjectSelectorWrapper.GetSelectedObject<Sprite>();

            if (Armor.Image != null)
            {
                spritename = Armor.Image.name;
            }
        }
    }

    public void SaveArmor()
    {
        if (Creating)
        {
            Create();
        }
        else
        {
            Edit();
        }

        ListObjects = (Resources.LoadAll("Armor", typeof(GameObject)));
    }

    public override void Create()
    {
        elementObject = new GameObject("Armor");
        Armor wcomponent = elementObject.AddComponent<Armor>();

        AssignArmor(ref wcomponent);

        Id++;
        wcomponent.Id = Id;
        CreatePrefab(ref elementObject, wcomponent);
        listArmors.AddItem(Armor.Name, Armor.Id);
        Creating = false;
        SetId();
    }

    public override void Edit()
    {
        Armor wcomponent = elementObject.GetComponent<Armor>();
        AssignArmor(ref wcomponent);
        CreatePrefab(ref elementObject, wcomponent);

        listArmors.ChangeName(listArmors.GetSelectedIndex(), Armor.Name);
    }

    public override void Delete()
    {
        if (elementObject != null)
        {
            DestroyImmediate(elementObject, true);
            AssetDatabase.DeleteAsset("Assets/Resources/Armor/" + Armor.Id + ".prefab");
            listArmors.RemoveItemIndex(listArmors.GetSelectedIndex());
        }
    }

    private void AssignArmor(ref Armor wcomponent)
    {
        wcomponent.Name = Armor.Name;
        wcomponent.Description = Armor.Description;
        wcomponent.Stats = Armor.Stats;
        wcomponent.Id = Armor.Id;
        wcomponent.Image = Armor.Image;
    }

    private void CreatePrefab(ref GameObject ArmorObject, Armor wcomponent)
    {
        PrefabUtility.CreatePrefab("Assets/Resources/Armor/" + wcomponent.Id + ".prefab", wcomponent.gameObject);
        DestroyImmediate(ArmorObject);
    }
}
