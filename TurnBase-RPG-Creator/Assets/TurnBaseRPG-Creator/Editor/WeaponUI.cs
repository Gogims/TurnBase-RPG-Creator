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

    protected override void InitErrors()
    {
        // Insertando las validaciones de propiedades
        err.InsertPropertyError("Name", element.name.Length, "The Name field can't be empty");
        err.InsertPropertyError("Agility", element.Stats.Agility, "The Agility field can't be negative");
        err.InsertPropertyError("Attack", element.Stats.Attack, "The Attack field can't be negative");
        err.InsertPropertyError("Luck", element.Stats.Luck, "The Luck field can't be negative");
        err.InsertPropertyError("Magic", element.Stats.Magic, "The Magic field can't be negative");
        err.InsertPropertyError("MagicDefense", element.Stats.MagicDefense, "The Magic Defense field can't be negative");
        err.InsertPropertyError("MaxHP", element.Stats.MaxHP, "The MaxHP field can't be negative");
        err.InsertPropertyError("MaxMP", element.Stats.MaxMP, "The MaxMP field can't be negative");
        err.InsertPropertyError("Price", element.Price, "The Price field can't be negative");
        //err.InsertPropertyError("Sprite", SpriteName.Length, "The Image field can't be empty");

        // Insertando las condiciones de las propiedades
        err.InsertCondition("Name", 0, ErrorCondition.Greater, LogicalOperators.None);
        err.InsertCondition("Agility", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("Attack", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("Luck", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("Magic", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("MagicDefense", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("MaxHP", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("MaxMP", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        err.InsertCondition("Price", 0, ErrorCondition.GreaterOrEqual, LogicalOperators.None);
        //err.InsertCondition("Sprite", 0, ErrorCondition.Greater, LogicalOperators.None);
    }

    protected override void UpdateValidations()
    {
        err.UpdateValue("Name", element.Name.Length);
        err.UpdateValue("Agility", element.Stats.Agility);
        err.UpdateValue("Attack", element.Stats.Attack);
        err.UpdateValue("Luck", element.Stats.Agility);
        err.UpdateValue("Magic", element.Stats.Agility);
        err.UpdateValue("MagicDefense", element.Stats.Agility);
        err.UpdateValue("MaxHP", element.Stats.Agility);
        err.UpdateValue("MaxMP", element.Stats.Agility);
        err.UpdateValue("Price", element.Price);
        //err.UpdateValue("Sprite", SpriteName.Length);
    }

    void OnGUI()
    {
        RenderLeftSide();        

        // Basic Settings
        GUILayout.BeginArea(new Rect(300, 0, 600, 180), "Basic Settings", EditorStyles.helpBox);
        GUILayout.Space(15);
        err.ShowErrorsLayout();

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
        element.Stats.Attack = EditorGUILayout.IntField("Attack: ", element.Stats.Attack);
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
