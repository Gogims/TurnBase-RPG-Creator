using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : RPGElement
{
    List<EnemyBattle> Enemies;
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

    public BattleManager(List<EnemyBattle> enemies, AbstractPlayer player)
    {
        Enemies = enemies;
        Player = player;
    }

    void Start()
    {
<<<<<<< HEAD
        //foreach (var state in Player.States)
        //{
            
        //}
=======
        ActorsOrdered = OrderActors(Player, Enemies);
>>>>>>> 3a1832d3c2f535711708dba7d9216fd8a23588cc
    }

    void Update()
    {
        switch (BattleState)
        {
            case BattleStateMachine.PLAYERTURN:
                Player.RemoveOnTurnState();
                break;
            case BattleStateMachine.ENEMYTURN:
                break;
            case BattleStateMachine.WIN:
                break;
            case BattleStateMachine.LOSE:
                break;
        }
    }

    private List<Tuple<AbstractActor, bool>> OrderActors(AbstractPlayer p, List<EnemyBattle> enemies)
    {
        List<Tuple<AbstractActor, bool>> actors = new List<Tuple<AbstractActor, bool>>();
        enemies = enemies.OrderByDescending(x => x.Enemy.Stats.Agility).ToList();

        foreach (var enemy in enemies)
        {
            if (p.Stats.Agility >= enemy.Enemy.Stats.Agility)
            {
                actors.Add(new Tuple<AbstractActor, bool>(p, true));                
            }

            actors.Add(new Tuple<AbstractActor, bool>(enemy.Enemy, false));
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
<<<<<<< HEAD
                //foreach (var item in inflicted.States)
                //{
                //    // Deberia ser por el ID
                //    //if (item.State == state.State)
                //    //{
                //    //    inflicted.States.Remove(item);
                //    //    break;
                //    //}
                //}
=======
                inflicted.RemoveState(state);
>>>>>>> 3a1832d3c2f535711708dba7d9216fd8a23588cc
            }
        }
    }
}
