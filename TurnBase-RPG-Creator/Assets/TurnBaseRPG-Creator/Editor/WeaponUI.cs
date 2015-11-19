using UnityEngine;
using UnityEditor;

public class WeaponUI : CRUD<Weapon>
{
    public WeaponUI() : base("Weapon", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref Weapon w)
    {
        AssignedElement = w;
        Init();        
    }

    void OnGUI()
    {
        RenderLeftSide();

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 180), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUI.enabled = !Selected;
        element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Description = EditorGUILayout.TextField("Description: ", element.Description);
        element.Type = (Weapon.WeaponType)EditorGUILayout.EnumPopup("Weapon Type: ", element.Type);
        element.Price = EditorGUILayout.IntField("Price: ", element.Price);

        if (GUI.Button(new Rect(0, 100, 400, 20), "Select Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, null, 1);
        }

        if (element.Image != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 100, Constant.SpriteWidth, Constant.SpriteHeight), element.Image.texture, Constant.GetTextureCoordinate(element.Image));
        }

        AddObject();

        GUILayout.EndArea();

        //Attributes stats
        GUILayout.BeginArea(new Rect(300, 180, 600, 160), "Attributes", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.Stats.Agility);
        element.Stats.Defense = EditorGUILayout.IntField("Attack: ", element.Stats.Attack);
        element.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.Stats.Luck);
        element.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.Stats.Magic);
        element.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.Stats.MagicDefense);
        element.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.Stats.MaxHP);
        element.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.Stats.MaxMP);

        GUILayout.EndArea();

        // State
        GUILayout.BeginArea(new Rect(300, 340, 600, 40), "State", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.PercentageState = EditorGUILayout.Slider("Apply State(%)", element.PercentageState, 0, 100);
        GUILayout.TextField(element.State.State);
        if (GUILayout.Button("Select State"))
        {
            var window = EditorWindow.GetWindow<StateUI>();
            window.Selected = true;
            window.Initialize(ref element.State);
            window.Show();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        if (Selected)
        {
            GUI.enabled = !Creating;
            SelectButton = GUI.Button(new Rect(300, 380, 100, 20), "Select");
            GUI.enabled = true;
        }
        else
        {
            GUI.enabled = true;
            SaveButton = GUI.Button(new Rect(300, 380, 100, 20), "Save");
            GUI.enabled = !Creating;
            DeleteButton = GUI.Button(new Rect(400, 380, 100, 20), "Delete");
            GUI.enabled = true;
        }
    } 

    override protected void AssignElement()
    {
        AssignedElement.Name = element.Name;
        AssignedElement.Description = element.Description;
        AssignedElement.Type = element.Type;
        AssignedElement.HitRate = element.HitRate;
        AssignedElement.NumberHit = element.NumberHit;
        AssignedElement.Stats = element.Stats;
        AssignedElement.Id = element.Id;
        AssignedElement.Image = element.Image;
        AssignedElement.State = element.State;
    }
}
