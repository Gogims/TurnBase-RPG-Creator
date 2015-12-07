using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

public class BattleManager : RPGElement
{
    List<Enemy> Enemies;
    AbstractPlayer Player;
    /// <summary>
    /// Actor, True = player
    /// </summary>
    List<Tuple<AbstractActor, bool>> ActorsOrdered;

    enum BattleStateMachine
    {
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    };

    BattleStateMachine BattleState;

    public BattleManager(List<Enemy> enemies, AbstractPlayer player)
    {
        Enemies = enemies;
        Player = player;
    }

    void Start()
    {
        //ActorsOrdered = OrderActors(Player, Enemies);
        ResizeSpriteToScreen(GameObject.Find("Top"));
        ResizeSpriteToScreen(GameObject.Find("Bottom"));

        foreach (var enemy in GameObject.FindGameObjectsWithTag("RPG-ENEMY"))
        {            

        }

    }

    /// <summary>
    /// Crea la escena del battlemap
    /// </summary>
    /// <param name="troop">Tropa que contiene el battlemap</param>
    public static void CreateTroopScene(Troop troop)
    {
        EditorApplication.NewScene(); // Crea una scene nueva.
        Camera.main.orthographic = true;
        Camera.main.backgroundColor = Color.black;
        Camera.main.gameObject.AddComponent<MainCamera>();
        Camera.main.orthographicSize = 6.95f; // Ajusta el tamaño de la camara ( la cantidad de espacio que va enfocar)        
        Camera.main.rect = new Rect(0, 0, 1, 1.4f);
        Camera.main.orthographicSize = 222.12f;

        new GameObject("BattleManager").AddComponent<BattleManager>();

        Vector2 backgroundSize = CreateBackground("Bottom", troop.BackgroundBottom, 0);
        CreateBackground("Top", troop.BackgroundTop, 1);
        foreach (var enemy in troop.Enemies)
        {
            GameObject gobj = new GameObject(enemy.Data.ActorName);
            gobj.tag = "RPG-ENEMY";
            var enemyScene = gobj.AddComponent<Enemy>();
            enemyScene.BattleEnemy = enemy;

            var sprite = gobj.AddComponent<SpriteRenderer>();
            sprite.sprite = enemy.Data.Image;
            sprite.sortingLayerName = "Actors";
        }

        EditorApplication.SaveScene("Assets/Resources/BattleMap/" + troop.Id + ".unity", true);// Guarda la scene.
    }

    void Update()
    {
        //switch (BattleState)
        //{
        //    case BattleStateMachine.PLAYERTURN:
        //        Player.RemoveOnTurnState();
        //        break;
        //    case BattleStateMachine.ENEMYTURN:
        //        break;
        //    case BattleStateMachine.WIN:
        //        break;
        //    case BattleStateMachine.LOSE:
        //        break;
        //}
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


    private void AbilityFight(ref AbstractActor Attacker, ref AbstractActor Defender, AbstractAbility ability)
    {
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
            MagicFight(ref Attacker, ref Defender, ability.AttackPower);
        }
        else
        {
            NormalFight(ref Attacker, ref Defender, ability.AttackPower);
        }        
    }

    private void MagicFight(ref AbstractActor Attacker, ref AbstractActor Defender, int boost)
    {
        // Calculando el Ataque
        int MagicDamage = Attacker.TotalMagicDamage() + boost;

        // Calculando la defensa
        int MagicDefense = Defender.TotalMagicDefense();

        int damage = MagicDamage - MagicDefense;

        Defender.RemoveOnDamageState();

        if (damage > 0)
        {
            Defender.HP -= damage;

            //Check if dead (todo)
        }
    }

    private void NormalFight(ref AbstractActor Attacker, ref AbstractActor Defender, int boost)
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

        Defender.RemoveOnDamageState();

        // Calculando el daño total
        int damage = AttackDamage - defense;

        if (damage > 0)
        {
            Defender.HP -= damage;

            //Check if dead (todo)
        }
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

    private static Vector2 CreateBackground(string name, Sprite background, int OrderLayer)
    {
        GameObject gobj = new GameObject(name);
        SpriteRenderer renderer = gobj.AddComponent<SpriteRenderer>();
        renderer.sprite = background;
        renderer.sortingLayerName = "Background";
        renderer.sortingOrder = OrderLayer;
        gobj.transform.localScale = new Vector2(1.5455f, 1);

        return new Vector2(gobj.transform.localScale.x* renderer.sprite.textureRect.width, gobj.transform.localScale.y * renderer.sprite.textureRect.height);
    }

    internal void UseItem(AbstractUsable UsableSelected)
    {
        throw new NotImplementedException();
    }

    internal void Attack()
    {
        throw new NotImplementedException();
    }

    internal void Run()
    {
        throw new NotImplementedException();
    }

    internal void UseAbility(AbstractAbility AbilitySelected)
    {
        throw new NotImplementedException();
    }
}
