using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BattleManager : RPGElement
{
    List<GameObject> Enemies;
    int GainXp = 0;
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
    enum Actions
    { 
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
        LOSE,
        RUN,
        LVLUP,
        ENDBATTLE
    };
    BattleStateMachine BattleState;
    private bool SELECTIONMODE;
    private int enemySelect;
    private GameObject CanvasMessage;
    string Message = string.Empty;
    private bool destroy;
    private bool working = false;
    private bool lvlup;

    public BattleManager(List<GameObject> enemies, Player player)
    {
        Enemies = enemies;
        Player = player;
    }

    void Start()
    {
        working = false;
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
         setCanvasMessage(GameObject.Find("BattleMap").transform.FindChild("CanvasMessage").gameObject);   
    }

    private void setCanvasMessage(GameObject canvasMessage)
    {
        Vector2 worldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);
        canvasMessage.transform.position = new Vector3(-worldScreen.x / 2 + canvasMessage.GetComponent<RectTransform>().rect.width / 2, -worldScreen.y / 2 + canvasMessage.GetComponent<RectTransform>().rect.height / 2);
        CanvasMessage = canvasMessage;
        CanvasMessage.SetActive(false);
    }

    private void setCanvasBar(GameObject canvasBar)
    {
        Vector2 worldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);
        canvasBar.transform.position = new Vector3(-worldScreen.x / 2 + canvasBar.GetComponent<RectTransform>().rect.width / 2, worldScreen.y / 2 - canvasBar.GetComponent<RectTransform>().rect.height / 2);
        Hp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("HP Slider").gameObject.GetComponent<Slider>();
        Mp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("MP Slider").gameObject.GetComponent<Slider>();
        CantHp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("HPCant").gameObject.GetComponent<Text>();
        CantMp = canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("MPCant").gameObject.GetComponent<Text>();
        canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("LevelVal").gameObject.GetComponent<Text>().text = Player.Data.Level.ToString();
        canvasBar.transform.FindChild("Panel").gameObject.transform.FindChild("PlayerName").gameObject.GetComponent<Text>().text = Player.Data.ActorName;
        UpdateBars();
    }

#if UNITY_EDITOR
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
        UnityEngine.Object [] BattlePrefabs = Resources.LoadAll("BattleMap",typeof(GameObject));
        foreach (var i in BattlePrefabs)
        {
            GameObject x = Instantiate(i as GameObject);
            x.name = i.name;
            x.transform.SetParent(battlemap.transform);
        }
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
        Constant.AddSceneToBuild("Assets/Resources/BattleMap/" + troop.Id + ".unity");
    }
#endif

    IEnumerator ShowMessage()
    {
        BattleMenu.SetActive(false);
        CanvasMessage.SetActive(true);
        CanvasMessage.transform.FindChild("Panel").gameObject.transform.FindChild("MessageText").gameObject.GetComponent<Text>().text = Message;
        selector.SetActive(false);
        working = true;
        yield return new WaitForSeconds(2);
        switch (BattleState)
        {
            case BattleStateMachine.PLAYERTURN:
                if (destroy)
                {
                    GainXp += Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data.RewardExperience;
                    Destroy(Enemies[enemySelect]);
                    Enemies.RemoveAt(enemySelect);
                    destroy = false;
                }
                
                BattleState = BattleStateMachine.ENEMYTURN;
                SELECTIONMODE = false;
                UpdateBars();
                break;
            case BattleStateMachine.ENEMYTURN:

                if (enemySelect+1 == Enemies.Count)
                {
                    BattleState = BattleStateMachine.PLAYERTURN;
                    BattleMenu.SetActive(true);
                    enemySelect = 0;
                }
                else
                    enemySelect++;
                SELECTIONMODE = false;
                UpdateBars();
                break;
            case BattleStateMachine.WIN:
                if (lvlup)
                    BattleState = BattleStateMachine.LVLUP;
                else
                    BattleState = BattleStateMachine.ENDBATTLE;
                break;
            case BattleStateMachine.LOSE:
                BattleState = BattleStateMachine.ENDBATTLE;
                break;
            case BattleStateMachine.RUN:
                BattleState = BattleStateMachine.ENDBATTLE;
                break;
            case BattleStateMachine.LVLUP:
                BattleState = BattleStateMachine.ENDBATTLE;
                break;
            default:
                break;
        }
        Message = "";
        CanvasMessage.SetActive(false);
        working = false;    
    }

    void Update()
    {
        if (!working)
        {
            int damage = 0;
            if (Enemies.Count <= 0)
                BattleState = BattleStateMachine.WIN;
            if (Player.Data.HP <= 0)
                BattleState = BattleStateMachine.LOSE;
            switch (BattleState)
            {
                case BattleStateMachine.PLAYERTURN:
                    if (SELECTIONMODE)
                    {
                        if (ProxyInput.GetInstance().Up())
                        {
                            if (enemySelect + 1 >= Enemies.Count) break;
                            enemySelect++;
                            PositionSelector(enemySelect);
                        }
                        else if (ProxyInput.GetInstance().Down())
                        {
                            if (enemySelect - 1 < 0) break;
                            enemySelect--;
                            PositionSelector(enemySelect);
                        }
                        else if (ProxyInput.GetInstance().A() || (UsableToUse != null && UsableToUse.AreaOfEffect == Constant.AOE.Self))
                        {

                            switch (ActionSelected)
                            {
                                case Actions.Attack:
                                    AbstractActor attacker = Player.Data;
                                    AbstractActor defender = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;
                                    damage = NormalFight(ref attacker, ref defender, 0);
                                    Message += "You have Attack! "+ defender.ActorName + " has recive " + damage + " damage.";
                                    break;
                                case Actions.Ability:
                                    AbstractActor attacker2 = Player.Data;
                                    AbstractActor defender2 = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;
                                    damage = AbilityFight(ref attacker2, ref defender2, AbilityToUse);
                                    Message += "You have use " + AbilityToUse.Ability + "! " + defender2.ActorName + " has recive " + damage + " damage.";
                                    break;
                                case Actions.Usable:
                                    AbstractActor actor = null;
                                    if (UsableToUse.AreaOfEffect == Constant.AOE.OneEnemy)
                                    {
                                        actor = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;

                                    }
                                    else
                                    {
                                        actor = Player.Data;
                                    }
                                    Message = UsableToUse.ItemName + " has been use on " + actor.ActorName + "! ";
                                    damage = ItemFigth(ref actor);
                                    break;
                                default:
                                    break;
                            }
                            
                            StartCoroutine(ShowMessage());

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
                    
                    AbstractAbility ability = Enemies[enemySelect].GetComponent<Enemy>().AttackSelected();
                    if (ability == null)
                    {
                        AbstractActor defender = Player.Data;
                        AbstractActor attacker = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;
                        damage = NormalFight(ref attacker, ref defender, 0);
                        Message += attacker.ActorName+" "+enemySelect+1+" has attack! " + damage.ToString() + " Damage dealt ";
                    }
                    else {
                        AbstractActor defender = Player.Data;
                        AbstractActor attacker = Enemies[enemySelect].GetComponent<Enemy>().BattleEnemy.Data;
                        damage = AbilityFight(ref attacker, ref defender, ability);
                        Message += attacker.ActorName +" "+enemySelect+1+ " has use " + ability.Ability + "! " + damage.ToString() + " Damage dealt ";
                    }
                    StartCoroutine(ShowMessage());
                    break;
                case BattleStateMachine.WIN:
                    ////TODO!!!!
                    List<AbstractUsable> items = GetRewards();//TODO
                    string sitems = "";
                    foreach (var i in items){
                        sitems += ", "+i.ItemName ;
                    }
                    int xp = getXP();//TTODOOOO
                    lvlup = applyXP();//TODO
                    Message = "You have earn "+getXP()+" XP, "+sitems;
                    StartCoroutine(ShowMessage());
                    break;
                case BattleStateMachine.LOSE:
                    Message = "You have lost the battle!";
                    StartCoroutine(ShowMessage());
                    break;
                case BattleStateMachine.LVLUP:
                    Message = "You have Reach Level " + Player.Data.Level;
                    showStats();//TODO
                    StartCoroutine(ShowMessage());
                    break;
                case BattleStateMachine.ENDBATTLE:
                    Destroy(GameObject.Find("BattleMap"));
                    Destroy(GameObject.Find("BattleMenu"));
                    Constant.ActiveMap.SetActive(true);
                    break;
            }
        }
    }

    private void showStats()
    {
        throw new NotImplementedException();
    }

    private bool applyXP()
    {
        foreach (var enemy in Enemies)
        {
            ;
        }
        return true;
    }

    private List<AbstractUsable> GetRewards()
    {
        return new List<AbstractUsable>();
    }

    private int getXP()
    {
        return 0;
    }
    void UpdateBars() {
        Hp.value = (float)((float)Player.Data.HP / (float)Player.Data.Stats.MaxHP);
        Mp.value = (float)((float)Player.Data.MP / (float)Player.Data.Stats.MaxMP);
        CantHp.text = Player.Data.HP.ToString() + "/" + Player.Data.Stats.MaxHP.ToString();
        CantMp.text = Player.Data.MP.ToString() + "/" + Player.Data.Stats.MaxMP.ToString();
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
                if (UsableToUse.Power < 0)
                    Message += "Damage Dealt " + -1 * UsableToUse.Power;
                else
                    Message += "HP Restored " +UsableToUse.Power;
                return UsableToUse.Power;
            case Constant.Attribute.MP:
                actor.MP += UsableToUse.Power;
                if (actor.MP > actor.Stats.MaxHP)
                    actor.MP = actor.Stats.MaxMP;
                if (actor.MP < actor.Stats.MaxHP)
                    actor.MP = 0;
                if (UsableToUse.Power < 0)
                    Message += "MP Dealt " + -1 * UsableToUse.Power;
                else
                    Message += "MP Restored " + UsableToUse.Power;
                break;
            case Constant.Attribute.Attack:
                actor.Stats.Attack += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "Attack decrease " + -1 * UsableToUse.Power;
                else
                    Message += "Attack raise " + UsableToUse.Power;
                break;
            case Constant.Attribute.Defense:
                actor.Stats.Defense += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "Defense decrease " + -1 * UsableToUse.Power;
                else
                    Message += "Defense raise " + UsableToUse.Power;
                break;
            case Constant.Attribute.Magic:
                actor.Stats.Magic += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "Magic decrease " + -1 * UsableToUse.Power;
                else
                    Message += "Magic raise " + UsableToUse.Power;
                break;
            case Constant.Attribute.MagicDefense:
                actor.Stats.MagicDefense += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "Magic Defense decrease " + -1 * UsableToUse.Power;
                else
                    Message += "Magic Defense raise" + UsableToUse.Power;
                break;
            case Constant.Attribute.Agility:
                actor.Stats.Agility += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "Agility decrease " + -1 * UsableToUse.Power;
                else
                    Message += "Agility raise " + UsableToUse.Power;
                break;
            case Constant.Attribute.Luck:
                actor.Stats.Luck += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "Luck decrease " + -1 * UsableToUse.Power;
                else
                    Message += "Luck raise " + UsableToUse.Power;
                break;
            case Constant.Attribute.MaxHP:
                actor.Stats.MaxHP += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "MaxHP decrease " + -1 * UsableToUse.Power;
                else
                    Message += "MaxHP raise " + UsableToUse.Power;
                break;
            case Constant.Attribute.MaxMP:
                actor.Stats.MaxMP += UsableToUse.Power;
                if (UsableToUse.Power < 0)
                    Message += "MaxMP decrease " + -1 * UsableToUse.Power;
                else
                    Message += "MaxMP raise " + UsableToUse.Power;
                break;
            default:
                break;
        }

        return 0;
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
                destroy = true;
            }
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
            Message += "Is a Critical Attack!";
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
                destroy = true;
            }
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
        Message = "You have run from the  battle!";
        BattleState = BattleStateMachine.RUN;
        StartCoroutine(ShowMessage());
    }

}
