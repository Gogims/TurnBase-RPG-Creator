using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : RPGElement
{
    Dictionary<Enemy, Rect> Enemies;
    Player Player;
    enum BattleStateMachine
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    };

    BattleStateMachine BattleState;

    public BattleManager(List<EnemyBattle> enemies, AbstractPlayer player)
    {
        foreach (var enemy in enemies)
        {
            Enemy e = new Enemy();
            e.Data = enemy.Enemy;
            Enemies.Add(e, enemy.EnemyPosition);
        }

        Player = new Player();
        Player.Data = player;
    }

    public void Start()
    {
        // orden de ataque de los enemigos
        Enemies = Enemies.OrderByDescending(x => x.Key.Data.Stats.Agility).ToDictionary(x => x.Key, x => x.Value);

        if (Player.Data.Stats.Agility >= Enemies.First().Key.Data.Stats.Agility)
        {
            BattleState = BattleStateMachine.PLAYERTURN;
        }
        else
        {
            BattleState = BattleStateMachine.ENEMYTURN;
        }
    }

    public void Update()
    {
        switch (BattleState)
        {
            case BattleStateMachine.START:
                break;
            case BattleStateMachine.PLAYERTURN:
                break;
            case BattleStateMachine.ENEMYTURN:
                break;
            case BattleStateMachine.WIN:
                break;
            case BattleStateMachine.LOSE:
                break;
        }
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
        int MagicDamage = Attacker.Stats.Magic + boost;
        MagicDamage += CheckArmor(Attacker.Helmet).Magic;
        MagicDamage += CheckArmor(Attacker.Body).Magic;
        MagicDamage += CheckArmor(Attacker.Feet).Magic;
        MagicDamage += CheckArmor(Attacker.Ring).Magic;
        MagicDamage += CheckArmor(Attacker.Necklace).Magic;

        // Calculando la defensa
        int MagicDefense = Defender.Stats.MagicDefense;
        MagicDefense += CheckArmor(Defender.Helmet).MagicDefense;
        MagicDefense += CheckArmor(Defender.Body).MagicDefense;
        MagicDefense += CheckArmor(Defender.Feet).MagicDefense;
        MagicDefense += CheckArmor(Defender.Ring).MagicDefense;
        MagicDefense += CheckArmor(Defender.Necklace).MagicDefense;

        int damage = MagicDamage - MagicDefense;

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

        int AttackDamage = Attacker.Stats.Attack + boost;
        AttackDamage += CheckArmor(Attacker.Helmet).Attack;
        AttackDamage += CheckArmor(Attacker.Body).Attack;
        AttackDamage += CheckArmor(Attacker.Feet).Attack;
        AttackDamage += CheckArmor(Attacker.Ring).Attack;
        AttackDamage += CheckArmor(Attacker.Necklace).Attack;

        if (Attacker.MainHand != null)
        {
            ApplyState(Attacker.MainHand.State, Attacker.MainHand.PercentageState, ref Defender);
        }

        AttackDamage *= damageMultiplier;

        // Calculando la defensa
        int defense = Defender.Stats.Defense;
        defense += CheckArmor(Defender.Helmet, ref Attacker).Defense;
        defense += CheckArmor(Defender.Body, ref Attacker).Defense;
        defense += CheckArmor(Defender.Feet, ref Attacker).Defense;
        defense += CheckArmor(Defender.Ring, ref Attacker).Defense;
        defense += CheckArmor(Defender.Necklace, ref Attacker).Defense;

        // Calculando el daño total
        int damage = AttackDamage - defense;

        if (damage > 0)
        {
            Defender.HP -= damage;

            //Check if dead (todo)
        }
    }

    private Attribute CheckArmor(AbstractArmor armorPiece)
    {
        if (armorPiece == null) return new Attribute();

        return armorPiece.Stats;
    }

    private Attribute CheckArmor(AbstractArmor armorPiece, ref AbstractActor Attacker)
    {
        ApplyState(armorPiece.State, armorPiece.PercentageState, ref Attacker);

        return CheckArmor(armorPiece);
    }    

    private void ApplyState(AbstractState state, float? ApplyRate, ref AbstractActor inflicted)
    {
        if (state.State != string.Empty && inflicted != null)
        {
            if (!ApplyRate.HasValue || ApplyRate.Value >= UnityEngine.Random.Range(1, 100))
            {
                inflicted.States.Add(state);
            }
        }
    }

    private void RemoveState(AbstractState state, float ApplyRate, ref AbstractActor inflicted)
    {
        if (state.State != string.Empty && inflicted != null)
        {
            if (ApplyRate >= UnityEngine.Random.Range(1, 100))
            {
                foreach (var item in inflicted.States)
                {
                    // Deberia ser por el ID
                    if (item.State == state.State)
                    {
                        inflicted.States.Remove(item);
                        break;
                    }
                }
            }
        }
    }
}
