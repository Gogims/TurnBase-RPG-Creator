using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

public class BattleManager : RPGElement
{
    List<GameObject> Enemies;
    Player Player;
    public GameObject BattleMenu;
    /// <summary>
    /// Selector para seleccionar el enemigo
    /// </summary>
    GameObject selector;
    /// <summary>
    /// habilidad seleccionada en el battleMenu
    /// </summary>
    AbstractAbility AbilityToUse;
    /// <summary>
    /// Item seleccionado en el battleMenu
    /// </summary>
    AbstractUsable UsableToUse;
    /// <summary>
    /// Actor, True = player
    /// </summary>
    List<Tuple<AbstractActor, bool>> ActorsOrdered;
    Slider Hp;
    Slider Mp;
    Text CantHp;
    Text CantMp;
    enum Actions { 
        Attack,
        Ability,
        Usable
    }
    Actions ActionSelected;
    enum BattleStateMachine
    {
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    };
    BattleStateMachine BattleState;
    private bool SELECTIONMODE;
    private int enemySelect;

    public BattleManager(List<GameObject> enemies, Player player)
    {
        Enemies = enemies;
        Player = player;
    }
    void Start()
    {
        Constant.LastSceneLoaded = "BattleMenu";
        Application.LoadLevelAdditive("BattleMenu");
        //ActorsOrdered = OrderActors(Player, Enemies);
        ResizeSpriteToScreen(GameObject.Find("Top"));
        ResizeSpriteToScreen(GameObject.Find("Bottom"));
        selector = GameObject.Find("BattleMap").transform.FindChild("Selector").gameObject;
        Vector2 worldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);
       
        selector.SetActive(false);
        Enemies = new List<GameObject>();
        Player = GameObject.FindWithTag("RPG-PLAYER").gameObject.GetComponent<Player>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("RPG-ENEMY"))
        {
            Enemies.Add(enemy);
        }
        
         GameObject canvasBar = GameObject.Find("BattleMap").transform.FindChild("CanvasBars").gameObject;
         setCanvasBar(canvasBar);
         

    }

    private void setCanvasBar(GameObject canvasBar)
    {
        Vector2 worldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);
        canvasBar.transform.position = new Vector3(-worldScreen.x / 2 + canvasBar.GetComponent<RectTransform>().rect.width / 2, worldScreen.y / 2 - canvasBar.GetComponent<RectTransform>().rect.height / 2);
        Hp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("HPSlider").gameObject.GetComponent<Slider>();
        Mp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("MPSlider").gameObject.GetComponent<Slider>();
        CantHp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("HPCant").gameObject.GetComponent<Text>();
        CantMp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("MPCant").gameObject.GetComponent<Text>();
        canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("LevelVal").gameObject.GetComponent<Text>().text = Player.Data.Level.ToString();
        canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("PlayerName").gameObject.GetComponent<Text>().text = Player.Data.ActorName;
        UpdateBars();
    }

    /// <summary>
    /// Crea la escena del battlemap
    /// </summary>
    /// <param name="troop">Tropa que contiene el battlemap</param>
    public static void CreateTroopScene(Troop troop)
    {
        EditorApplication.NewScene(); // Crea una scene nueva.
        var battlemap = new GameObject("BattleMap");
        Camera.main.transform.parent = battlemap.transform;
        Camera.main.orthographic = true;
        Camera.main.backgroundColor = Color.black;
        Camera.main.gameObject.AddComponent<MainCamera>();
        Camera.main.orthographicSize = 6.95f; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)        
        Camera.main.rect = new Rect(0, 0, 1, 1.4f);
        Camera.main.orthographicSize = 222.12f;        

        var bm = new GameObject("BattleManager").AddComponent<BattleManager>();
        bm.transform.parent = battlemap.transform;

        Vector2 backgroundSize = CreateBackground("Bottom", troop.BackgroundBottom, 0, battlemap);
        CreateBackground("Top", troop.BackgroundTop, 1, battlemap);
        foreach (var enemy in troop.Enemies)
        {
            GameObject gobj = new GameObject(enemy.Data.ActorName);
            gobj.tag = "RPG-ENEMY";
            var enemyScene = gobj.AddComponent<Enemy>();
            enemyScene.BattleEnemy = enemy;

            var sprite = gobj.AddComponent<SpriteRenderer>();
            sprite.sprite = enemy.Data.Image;
            sprite.sortingLayerName = "Actors";
            gobj.transform.parent = battlemap.transform;

        }

        Audio audio = new Audio("BackgroundAudio");
        audio.CreateAudioSource(troop.Background);
        audio.gameobject.transform.parent = battlemap.transform;
        EditorApplication.SaveScene("Assets/Resources/BattleMap/" + troop.Id + ".unity", true);// Guarda la scene.
    }

    void Update()
    {
        if (Enemies.Count <= 0)
            BattleState = BattleStateMachine.WIN;
        if (Player.Data.HP <= 0)
            BattleState = BattleStateMachine.LOSE;
        switch (BattleState)
        {
            case BattleStateMachine.PLAYERTURN:
                if (SELECTIONMODE)
                {
                    
                    if (ProxyInput.GetInstance().Up()){
                        if (enemySelect + 1 >= Enemies.Count) break;
                        enemySelect++;
                        PositionSelector(enemySelect);
                    }
                    else if (ProxyInput.GetInstance().Down()) {
                        if (enemySelect -1 < 0 ) break;
                        enemySelect--;
                        PositionSelector(enemySelect);
                    }
                    else if (ProxyInput.GetInstance().A() || (UsableToUse != null && UsableToUse.AreaOfEffect == Constant.AOE.Self)){

                    switch (ActionSelected)
                        {
                            case Actions.Attack:
                                 AbstractActor attacker = Player.Data;
                                 AbstractActor defender = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;
                                 int damage = NormalFight(ref attacker, ref defender, 0);
                                 ShowDamage(damage);
                                 BattleState = BattleStateMachine.ENEMYTURN;
                                 break;
                            case Actions.Ability:
                                 AbstractActor attacker2 = Player.Data;
                                 AbstractActor defender2 = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;
                                 int damage2 = AbilityFight(ref attacker2, ref defender2,  AbilityToUse);
                                 ShowDamage(damage2);
                                 BattleState = BattleStateMachine.ENEMYTURN;
                                break;
                            case Actions.Usable:
                                AbstractActor actor =null;
                                int val=0;
                                if (UsableToUse.AreaOfEffect == Constant.AOE.OneEnemy)
                                {
                                    actor = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;

                                }
                                else {
                                    actor = Player.Data;
                                }
                                val = ItemFigth(ref actor);
                                if (UsableToUse.Attribute == Constant.Attribute.HP && UsableToUse.AreaOfEffect == Constant.AOE.OneEnemy)
                                    ShowDamage(val);
                                else if (UsableToUse.Attribute == Constant.Attribute.HP && UsableToUse.AreaOfEffect == Constant.AOE.Self)
                                {
                                    ShowHeal(val);
                                }
                                BattleState = BattleStateMachine.ENEMYTURN;
                                SELECTIONMODE = false;
                                UpdateBars();
                                
                                break;
                            default:
                                break;
                        }
                    }
                    else if (ProxyInput.GetInstance().B())
                    {
                        deactivateSelector();
                        BattleMenu.SetActive(true);
                        SELECTIONMODE = false;
                    }
                }
                //Player.RemoveOnTurnState();
                break;
            case BattleStateMachine.ENEMYTURN:
                BattleMenu.SetActive(true);
                BattleState = BattleStateMachine.PLAYERTURN;
                SELECTIONMODE = false;
                UpdateBars();
                break;
            case BattleStateMachine.WIN:
                Destroy(GameObject.Find("BattleMap"));
                Destroy(GameObject.Find("BattleMenu"));
                Constant.ActiveMap.SetActive(true);
                break;
            case BattleStateMachine.LOSE:
                Destroy(GameObject.Find("BattleMap"));
                Destroy(GameObject.Find("BattleMenu"));
                Constant.ActiveMap.SetActive(true);
                break;
        }
    }
    void UpdateBars() {
        Hp.value = (float)((float)Player.Data.HP / (float)Player.Data.Stats.MaxHP);
        Mp.value = (float)((float)Player.Data.MP / (float)Player.Data.Stats.MaxMP);
        CantHp.text = Player.Data.HP.ToString() + "/" + Player.Data.Stats.MaxHP.ToString();
        CantMp.text = Player.Data.MP.ToString() + "/" + Player.Data.Stats.MaxMP.ToString();
    }
    private void ShowHeal(int val)
    {
        Debug.Log(val);
    }
    /// <summary>
    /// Usa el item seleccionado en el battleMenu
    /// </summary>
    /// <param name="actor">Actor que se le va aplicar el item</param>
    /// <returns>retorna la cantida de vida disminuida o aumentada por el item </returns>
    private int ItemFigth(ref AbstractActor actor)
    {
        Player.Items.DeleteItem(UsableToUse.ItemName);
        switch (UsableToUse.Attribute)
        {
            case Constant.Attribute.None:
                return 0;
            case Constant.Attribute.HP:
                actor.HP += UsableToUse.Power;
                if (actor.HP > actor.Stats.MaxHP)
                    actor.HP = actor.Stats.MaxHP;
                if (actor.HP < actor.Stats.MaxHP)
                    actor.HP = 0;
                return UsableToUse.Power;
            case Constant.Attribute.MP:
                actor.MP += UsableToUse.Power;
                if (actor.MP > actor.Stats.MaxHP)
                    actor.MP = actor.Stats.MaxMP;
                if (actor.MP < actor.Stats.MaxHP)
                    actor.MP = 0;
                break;
            case Constant.Attribute.Attack:
                actor.Stats.Attack += UsableToUse.Power;
                break;
            case Constant.Attribute.Defense:
                actor.Stats.Defense += UsableToUse.Power;
                break;
            case Constant.Attribute.Magic:
                actor.Stats.Magic += UsableToUse.Power;
                break;
            case Constant.Attribute.MagicDefense:
                actor.Stats.MagicDefense += UsableToUse.Power;
                break;
            case Constant.Attribute.Agility:
                actor.Stats.Agility += UsableToUse.Power;
                break;
            case Constant.Attribute.Luck:
                actor.Stats.Luck += UsableToUse.Power;
                break;
            case Constant.Attribute.MaxHP:
                actor.Stats.MaxHP += UsableToUse.Power;
                break;
            case Constant.Attribute.MaxMP:
                actor.Stats.MaxMP += UsableToUse.Power;
                break;
            default:
                break;
        }

        return 0;
    }

    private void ShowDamage(int damage)
    {
        Debug.Log(damage);
    }
    private void ResizeSpriteToScreen(GameObject background)
    {
        var sr = background.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        Vector2 worldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);

        background.transform.localScale = new Vector3(worldScreen.x / width, worldScreen.y / height, 1);

    }

    private List<Tuple<AbstractActor, bool>> OrderActors(AbstractPlayer p, List<BattleEnemy> enemies)
    {
        List<Tuple<AbstractActor, bool>> actors = new List<Tuple<AbstractActor, bool>>();
        enemies = enemies.OrderByDescending(x => x.Data.Stats.Agility).ToList();

        foreach (var enemy in enemies)
        {
            if (p.Stats.Agility >= enemy.Data.Stats.Agility)
            {
                actors.Add(new Tuple<AbstractActor, bool>(p, true));                
            }

            actors.Add(new Tuple<AbstractActor, bool>(enemy.Data, false));
        }    

        return actors;
    }


    private int AbilityFight(ref AbstractActor Attacker, ref AbstractActor Defender, AbstractAbility ability)
    {
        Attacker.MP -= ability.MPCost;
        if (ability.State.State != string.Empty)
        {
            if (ability.Type == Constant.OffenseDefense.Offensive)
            {
                ApplyState(ability.State, ability.ApplyStatePorcentage, ref Defender);
            }
            else
            {
                RemoveState(ability.State, ability.ApplyStatePorcentage, ref Defender);
            }
        }

        if (ability.AttackType == Constant.AttackType.MagicAttack)
        {
           return  MagicFight(ref Attacker, ref Defender, ability.AttackPower);
        }
        else
        {
            return NormalFight(ref Attacker, ref Defender, ability.AttackPower);
        }        
    }

    private int MagicFight(ref AbstractActor Attacker, ref AbstractActor Defender, int boost)
    {
        
        // Calculando el Ataque
        int MagicDamage = Attacker.TotalMagicDamage() + boost;
        
        // Calculando la defensa
        int MagicDefense = Defender.TotalMagicDefense();

        int damage = MagicDamage - MagicDefense;
        
        //Defender.RemoveOnDamageState();
        

        if (damage > 0)
        {
            Defender.HP -= damage;
            if (Defender.HP <= 0 )
            {
                Destroy(Enemies[enemySelect]);
                Enemies.RemoveAt(enemySelect);
            }
            //Check if dead (todo)
        }
        return damage;
    }

    private int NormalFight(ref AbstractActor Attacker, ref AbstractActor Defender, int boost)
    {
        int damageMultiplier = 1;

        // Revisando si realiza un ataque crítico
        if ((Attacker.Stats.Luck / Attacker.Level) >= UnityEngine.Random.Range(1, 100))
        {
            damageMultiplier = 2;
        }

        // Calculando el ataque

        int AttackDamage = Attacker.TotalDamage();

        if (Attacker.MainHand != null)
        {
            ApplyState(Attacker.MainHand.State, Attacker.MainHand.PercentageState, ref Defender);
        }

        AttackDamage *= damageMultiplier;

        // Calculando la defensa
        int defense = Defender.Stats.Defense;
        ApplyState(Defender.Helmet.State, Defender.Helmet.PercentageState, ref Attacker);
        ApplyState(Defender.Body.State, Defender.Body.PercentageState, ref Attacker);
        ApplyState(Defender.Feet.State, Defender.Feet.PercentageState, ref Attacker);
        ApplyState(Defender.Ring.State, Defender.Ring.PercentageState, ref Attacker);
        ApplyState(Defender.Necklace.State, Defender.Necklace.PercentageState, ref Attacker);

        //Defender.RemoveOnDamageState();

        // Calculando el daño total
        int damage = AttackDamage - defense;

        if (damage > 0)
        {
            Defender.HP -= damage;
            if (Defender.HP < 0)
            {
                Destroy(Enemies[enemySelect]);
                Enemies.RemoveAt(enemySelect);
            }
            //Check if dead (todo)
        }
        return damage;
    }     

    /// <summary>
    /// Intenta de aplicar un estado a partir de una probabilidad
    /// </summary>
    /// <param name="state">Estado ha aplicar</param>
    /// <param name="ApplyRate">Probabilidad de aplicar</param>
    /// <param name="inflicted">Actor que se intenta de aplicar el estado</param>
    private void ApplyState(AbstractState state, float? ApplyRate, ref AbstractActor inflicted)
    {
        if (state.State != string.Empty && inflicted != null)
        {
            if (!ApplyRate.HasValue || ApplyRate.Value >= UnityEngine.Random.Range(1, 100))
            {
                inflicted.AddState(state);
            }
        }
    }

    /// <summary>
    /// Intenta de remover un estado a partir de una probabilidad
    /// </summary>
    /// <param name="state">Estado ha remover</param>
    /// <param name="ApplyRate">Probabilidad de remover</param>
    /// <param name="inflicted">Actor que se intenta de remover el estado</param>
    private void RemoveState(AbstractState state, float ApplyRate, ref AbstractActor inflicted)
    {
        if (state.State != string.Empty && inflicted != null)
        {
            if (ApplyRate >= UnityEngine.Random.Range(1, 100))
            {
                //foreach (var item in inflicted.States)
                //{
                //    // Deberia ser por el ID
                //    //if (item.State == state.State)
                //    //{
                //    //    inflicted.States.Remove(item);
                //    //    break;
                //    //}
                //}
                inflicted.RemoveState(state);
            }
        }
    }

    private static Vector2 CreateBackground(string name, Sprite background, int OrderLayer, GameObject father)
    {
        GameObject gobj = new GameObject(name);
        SpriteRenderer renderer = gobj.AddComponent<SpriteRenderer>();
        renderer.sprite = background;
        renderer.sortingLayerName = "Background";
        renderer.sortingOrder = OrderLayer;
        gobj.transform.localScale = new Vector2(1.5455f, 1);
        gobj.transform.parent = father.transform;

        return new Vector2(gobj.transform.localScale.x* renderer.sprite.textureRect.width, gobj.transform.localScale.y * renderer.sprite.textureRect.height);
    }
    private void PositionSelector(int index){
        selector.transform.position = new Vector3(Enemies[index].transform.position.x, Enemies[index].transform.position.y + Enemies[index].GetComponent<SpriteRenderer>().sprite.rect.height);
    }
    private void ActiveSelector() {
        BattleMenu.SetActive(false);
        selector.SetActive(true);
        PositionSelector(0);
        enemySelect = 0;
    }
    private void deactivateSelector() {
        selector.SetActive(false);
        PositionSelector(0);
        enemySelect = 0;
    }
    public void UseItem(AbstractUsable UsableSelected)
    {
        ActionSelected = Actions.Usable;
        SELECTIONMODE = true;
        UsableToUse = UsableSelected;
        if (UsableToUse.AreaOfEffect == Constant.AOE.OneEnemy)
            ActiveSelector();
       
    }

    public void Attack()
    {
        ActionSelected = Actions.Attack;
        ActiveSelector();
        SELECTIONMODE = true;
    }
    public void UseAbility(AbstractAbility AbilitySelected)
    {
        ActionSelected = Actions.Ability;
        SELECTIONMODE = true;
        ActiveSelector();
        AbilityToUse = AbilitySelected;
    }

    public void Run()
    {
        Destroy(GameObject.Find("BattleMap"));
        Destroy(GameObject.Find("BattleMenu"));
        Constant.ActiveMap.SetActive(true);
    }

}
