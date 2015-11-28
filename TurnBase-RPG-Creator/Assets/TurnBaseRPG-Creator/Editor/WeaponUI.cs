using UnityEngine;
using UnityEditor;

public class WeaponUI : CRUD<Weapon>
{
    AbstractWeapon SelectedWeapon;

    public WeaponUI() : base("Weapon", new Rect(0, 0, 300, 400)) { }

    public void Initialize(ref AbstractWeapon weapon)
    {
        SelectedWeapon = weapon;
        Init();        
    }

    protected override void InitErrors()
    {
        // Insertando las validaciones de propiedades
        err.InsertPropertyError("Name", element.name.Length, "The Name field can't be empty");
        err.InsertPropertyError("Agility", element.Data.Stats.Agility, "The Agility field can't be negative");
        err.InsertPropertyError("Attack", element.Data.Stats.Attack, "The Attack field can't be negative");
        err.InsertPropertyError("Luck", element.Data.Stats.Luck, "The Luck field can't be negative");
        err.InsertPropertyError("Magic", element.Data.Stats.Magic, "The Magic field can't be negative");
        err.InsertPropertyError("MagicDefense", element.Data.Stats.MagicDefense, "The Magic Defense field can't be negative");
        err.InsertPropertyError("MaxHP", element.Data.Stats.MaxHP, "The MaxHP field can't be negative");
        err.InsertPropertyError("MaxMP", element.Data.Stats.MaxMP, "The MaxMP field can't be negative");
        err.InsertPropertyError("Price", element.Data.Price, "The Price field can't be negative");
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
        err.UpdateValue("Agility", element.Data.Stats.Agility);
        err.UpdateValue("Attack", element.Data.Stats.Attack);
        err.UpdateValue("Luck", element.Data.Stats.Agility);
        err.UpdateValue("Magic", element.Data.Stats.Agility);
        err.UpdateValue("MagicDefense", element.Data.Stats.Agility);
        err.UpdateValue("MaxHP", element.Data.Stats.Agility);
        err.UpdateValue("MaxMP", element.Data.Stats.Agility);
        err.UpdateValue("Price", element.Data.Price);
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
        element.Data.ItemName = element.Name = EditorGUILayout.TextField("Name: ", element.Name);
        element.Data.Description = EditorGUILayout.TextField("Description: ", element.Data.Description);
        element.Data.Type = (AbstractWeapon.WeaponType)EditorGUILayout.EnumPopup("Weapon Type: ", element.Data.Type);
        element.Data.Price = EditorGUILayout.IntField("Price: ", element.Data.Price);

        if (GUI.Button(new Rect(0, 100, 400, 20), "Select Sprite"))
        {
            EditorGUIUtility.ShowObjectPicker<Sprite>(null, false, "Weapon", 1);
        }

        if (element.Icon != null)
        {
            GUI.DrawTextureWithTexCoords(new Rect(400, 100, Constant.SpriteWidth, Constant.SpriteHeight), element.Icon.texture, Constant.GetTextureCoordinate(element.Icon));
        }

        AddObject();

        GUILayout.EndArea();

        //Attributes stats
        GUILayout.BeginArea(new Rect(300, 180, 600, 160), "Attributes", EditorStyles.helpBox);
        GUILayout.Space(15);

        element.Data.Stats.Agility = EditorGUILayout.IntField("Agility: ", element.Data.Stats.Agility);
        element.Data.Stats.Attack = EditorGUILayout.IntField("Attack: ", element.Data.Stats.Attack);
        element.Data.Stats.Luck = EditorGUILayout.IntField("Luck: ", element.Data.Stats.Luck);
        element.Data.Stats.Magic = EditorGUILayout.IntField("Magic: ", element.Data.Stats.Magic);
        element.Data.Stats.MagicDefense = EditorGUILayout.IntField("MagicDefense: ", element.Data.Stats.MagicDefense);
        element.Data.Stats.MaxHP = EditorGUILayout.IntField("MaxHP: ", element.Data.Stats.MaxHP);
        element.Data.Stats.MaxMP = EditorGUILayout.IntField("MaxMP: ", element.Data.Stats.MaxMP);

        GUILayout.EndArea();

        // State
        GUILayout.BeginArea(new Rect(300, 340, 600, 40), "State", EditorStyles.helpBox);
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        element.Data.PercentageState = EditorGUILayout.Slider("Apply State(%)", element.Data.PercentageState, 0, 100);
        GUILayout.TextField(element.Data.State.State);
        if (GUILayout.Button("Select State"))
        {
            var window = EditorWindow.GetWindow<StateUI>();
            window.Selected = true;
            window.Initialize(ref element.Data.State);
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

    protected override void Create()
    {
        element.Data.Image = element.Icon;
        base.Create();
    }

    override protected void AssignElement()
    {
        SelectedWeapon.ItemName = element.Name;
        SelectedWeapon.Description = element.Data.Description;
        SelectedWeapon.HitRate = element.Data.HitRate;
        SelectedWeapon.Image = element.Data.Image;
        SelectedWeapon.MinLevel = element.Data.MinLevel;
        SelectedWeapon.NumberHit = element.Data.NumberHit;
        SelectedWeapon.PercentageState = element.Data.PercentageState;
        SelectedWeapon.Price = element.Data.Price;
        SelectedWeapon.State = element.Data.State;
        SelectedWeapon.Stats = element.Data.Stats;
        SelectedWeapon.Type = element.Data.Type;
    }
}
