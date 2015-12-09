using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Actor : RPGElement
{
    private const float speed =1f;

    protected virtual void Start()
    {

    }

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    protected bool Move(float xDir, float yDir)
    {
        if (xDir == 0 && yDir == 0) return false;

        Vector2 movement = new Vector2();

        movement.x = xDir * Time.deltaTime * speed;
        movement.y = yDir * Time.deltaTime * speed;

        transform.Translate(movement);

        return true;
    }

    protected float NextPosition(float dir)
    {
        return dir * Time.deltaTime * speed;
    }

    protected void MoveDirection(bool input, string direction, float idle)
    {
        var animations = GetComponent<Animator>();

        if (input)
        {
            animations.SetBool(direction, true);
            animations.SetFloat("Idle", idle);
        }
        else
        {
            animations.SetBool(direction, false);
        }
    }
}

[Serializable]
public class AbstractActor
{
    /// <summary>
    /// Atributos actuales del actor
    /// </summary>
    public Attribute Stats;
    /// <summary>
    /// Nivel actual del actor
    /// </summary>
    public int Level;
    /// <summary>
    /// Breve descripcion del actor
    /// </summary>
    public string Description;
    /// <summary>
    /// Cantidad de vida disponible por el actor
    /// </summary>
    public int HP;
    /// <summary>
    /// Cantidad de mana disponible por el actor
    /// </summary>
    public int MP;
    /// <summary>
    /// Imagen del actor
    /// </summary>
    public Sprite Image;
    /// <summary>
    /// Arma del actor
    /// </summary>
    public AbstractWeapon MainHand;
    /// <summary>
    /// Cabeza del actor
    /// </summary>
    public AbstractArmor Helmet;
    /// <summary>
    /// Pecho y piernas del actor
    /// </summary>
    public AbstractArmor Body;
    /// <summary>
    /// Zapatos del actor
    /// </summary>
    public AbstractArmor Feet;
    /// <summary>
    /// Anillo del actor
    /// </summary>
    public AbstractArmor Ring;
    /// <summary>
    /// Collar del actor
    /// </summary>
    public AbstractArmor Necklace;
    /// <summary>
    /// Clase del actor
    /// </summary>
    public AbstractJob Job;
    /// <summary>
    /// Nombre del actor
    /// </summary>
    public string ActorName = string.Empty;
    /// <summary>
    /// Si su turno en el modo combate terminó
    /// </summary>
    public bool TurnEnded;


    /// <summary>
    /// Listado de estados que posee el actor actualmente
    /// </summary>
    private Dictionary<string, Dictionary<string, AbstractState>> ApplyStates;
    /// <summary>
    /// Listado del momento en que el estado se elimina
    /// </summary>
    private Dictionary<string, Dictionary<string, AbstractState>> RemoveStates;

    public AbstractActor()
    {
        Stats = new Attribute();
        MainHand = new AbstractWeapon();
        Helmet = new AbstractArmor();
        Body = new AbstractArmor();
        Feet = new AbstractArmor();
        Ring = new AbstractArmor();
        Necklace = new AbstractArmor();
        Job = new AbstractJob();
    }    

    public int TotalDamage()
    {
        int AttackDamage = Stats.Attack;

        AttackDamage += MainHand.Stats.Attack;
        AttackDamage += CheckArmor(Helmet).Attack;
        AttackDamage += CheckArmor(Body).Attack;
        AttackDamage += CheckArmor(Feet).Attack;
        AttackDamage += CheckArmor(Ring).Attack;
        AttackDamage += CheckArmor(Necklace).Attack;

        return AttackDamage;
    }

    public int TotalDefense()
    {
        int Defense = Stats.Defense;

        Defense += MainHand.Stats.Defense;
        Defense += CheckArmor(Helmet).Defense;
        Defense += CheckArmor(Body).Defense;
        Defense += CheckArmor(Feet).Defense;
        Defense += CheckArmor(Ring).Defense;
        Defense += CheckArmor(Necklace).Defense;

        return Defense;
    }

    public int TotalMagicDefense()
    {
        int MagicDefense = Stats.MagicDefense;

        MagicDefense += MainHand.Stats.MagicDefense;
        MagicDefense += CheckArmor(Helmet).MagicDefense;
        MagicDefense += CheckArmor(Body).MagicDefense;
        MagicDefense += CheckArmor(Feet).MagicDefense;
        MagicDefense += CheckArmor(Ring).MagicDefense;
        MagicDefense += CheckArmor(Necklace).MagicDefense;

        return MagicDefense;
    }

    public int TotalMagicDamage()
    {
        int MagicDamage = Stats.Magic;

        MagicDamage += MainHand.Stats.Magic;
        MagicDamage += CheckArmor(Helmet).Magic;
        MagicDamage += CheckArmor(Body).Magic;
        MagicDamage += CheckArmor(Feet).Magic;
        MagicDamage += CheckArmor(Ring).Magic;
        MagicDamage += CheckArmor(Necklace).Magic;

        return MagicDamage;
    }

    /// <summary>
    /// Instancia los hash de estados en caso de que no hayan sido instanceados
    /// </summary>
    public void InstanceStates()
    {
        if (ApplyStates == null)
        {
            ApplyStates = new Dictionary<string, Dictionary<string, AbstractState>>();
            ApplyStates.Add("OnStart", new Dictionary<string, AbstractState>());
            ApplyStates.Add("OnAction", new Dictionary<string, AbstractState>()); 
        }

        if (RemoveStates == null)
        {
            RemoveStates = new Dictionary<string, Dictionary<string, AbstractState>>();
            RemoveStates.Add("OnDamage", new Dictionary<string, AbstractState>());
            RemoveStates.Add("OnBattleEnd", new Dictionary<string, AbstractState>());
            RemoveStates.Add("OnTurn", new Dictionary<string, AbstractState>()); 
        }
    }

    /// <summary>
    /// Agrega un estado en la estructura de aplicar estado
    /// </summary>
    /// <param name="state">Estado a agregar</param>
    public void AddState(AbstractState state)
    {
        if (state.Behavior == Constant.ActionType.None && !ApplyStates["OnStart"].ContainsKey(state.id))
        {
            ApplyStates["OnStart"].Add(state.id, state);
            ApplyStates["OnStart"] = OrderDictionary(ApplyStates["OnStart"]);
        }
        else if (state.Behavior != Constant.ActionType.None && !ApplyStates["OnAction"].ContainsKey(state.id))
        {
            ApplyStates["OnAction"].Add(state.id, state);
            ApplyStates["OnAction"] = OrderDictionary(ApplyStates["OnAction"]);
        }

        if (state.RemoveBattleEnd && !RemoveStates["OnBattleEnd"].ContainsKey(state.id))
        {
            RemoveStates["OnBattleEnd"].Add(state.id, state);
            RemoveStates["OnBattleEnd"] = OrderDictionary(RemoveStates["OnBattleEnd"]);
        }
        if (state.RemoveByDamage && !RemoveStates["OnDamage"].ContainsKey(state.id))
        {
            RemoveStates["OnDamage"].Add(state.id, state);
            RemoveStates["OnDamage"] = OrderDictionary(RemoveStates["OnDamage"]);
        }
        if (state.RemoveByTurn && !RemoveStates["OnTurn"].ContainsKey(state.id))
        {
            RemoveStates["OnTurn"].Add(state.id, state);
            RemoveStates["OnTurn"] = OrderDictionary(RemoveStates["OnTurn"]);
        }
    }

    /// <summary>
    /// Remueve el estado de la estructura a aplicar
    /// </summary>
    /// <param name="state">Estado a remover</param>
    public void RemoveState(AbstractState state)
    {
        if (state.Behavior == Constant.ActionType.None && ApplyStates["OnStart"].ContainsKey(state.id))
        {
            ApplyStates["OnStart"].Remove(state.id);
        }
        else if (state.Behavior != Constant.ActionType.None && !ApplyStates["OnAction"].ContainsKey(state.id))
        {
            ApplyStates["OnAction"].Remove(state.id);
        }

        if (state.RemoveBattleEnd && !RemoveStates["OnBattleEnd"].ContainsKey(state.id))
        {
            RemoveStates["OnBattleEnd"].Remove(state.id);
        }
        if (state.RemoveByDamage && !RemoveStates["OnDamage"].ContainsKey(state.id))
        {
            RemoveStates["OnDamage"].Remove(state.id);
        }
        if (state.RemoveByTurn && !RemoveStates["OnTurn"].ContainsKey(state.id))
        {
            RemoveStates["OnTurn"].Remove(state.id);
        }
    }

    /// <summary>
    /// Busca todos los estados que se disparan al recibir daño e intenta de removerlos si pasan la condicional
    /// </summary>
    public void RemoveOnDamageState()
    {
        if (RemoveStates["OnDamage"].Count == 0) return;

        foreach (var state in RemoveStates["OnDamage"].Values)
        {
            if (state.PercentRemoveByDamage >= UnityEngine.Random.Range(0, 100))
            {
                RemoveState(state);
            }
        }
    }

    /// <summary>
    /// Busca todos los estados que se disparan al iniciar un turno e intenta de removerlos si pasan la condicional
    /// </summary>
    public void RemoveOnTurnState()
    {
        if (RemoveStates["OnTurn"].Count == 0) return;

        foreach (var state in RemoveStates["OnTurn"].Values)
        {
            state.Turn++;

            if (state.Turn >= state.TurnTotal)
            {
                RemoveState(state);
            }
        }
    }

    /// <summary>
    /// Busca todos los estados que se disparan al final del combate y los remueve
    /// </summary>
    public void RemoveOnBattleEnd()
    {
        if (RemoveStates["OnBattleEnd"].Count == 0) return;

        foreach (var state in RemoveStates["OnBattleEnd"].Values)
        {
            RemoveState(state);
        }
    }

    /// <summary>
    /// Ordena el diccionary a partir de la prioridad
    /// </summary>
    /// <param name="states">Hash de estados</param>
    /// <returns>Hash de estado ordenado</returns>
    private Dictionary<string, AbstractState> OrderDictionary(Dictionary<string, AbstractState> states)
    {
        return states.OrderByDescending(x => x.Value.Priority).ToDictionary(x => x.Key, x => x.Value);
    }

    private Attribute CheckArmor(AbstractArmor armorPiece)
    {
        if (armorPiece == null) return new Attribute();

        return armorPiece.Stats;
    }

    private int ApplyStateDamage(AbstractState state)
    {
        int HealDamage = state.Type == Constant.DamageHeal.Damage ? -1 : 1;
        float value = 0f;

        value += state.FixedValue;

        if (state.ActionRate != 0)
        {
            if (state.AttackType == Constant.AttackType.MagicAttack)
            {
                value += TotalMagicDamage() * (state.ActionRate / 100);
            }
            else
            {
                value += TotalDamage() * (state.ActionRate / 100);
            }
        }

        return (int)value * HealDamage;
    }
}