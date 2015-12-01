using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

public class State: RPGElement
{
    public AbstractState Data;

    public State()
    {
        Data = new AbstractState();
    }
}

[Serializable]
public class AbstractState
{
    /// <summary>
    /// ID del estado
    /// </summary>
    public string id;    
    /// <summary>
    /// Prioridad del estado (100 = más alto)
    /// </summary>
    public int Priority;
    /// <summary>
    /// Nombre del estado
    /// </summary>
    public string State = string.Empty;
    /// <summary>
    /// Acción que realiza el estado
    /// </summary>
    public Constant.ActionType Behavior;
    /// <summary>
    /// Valor fijo que realizará el estado
    /// </summary>
    public int FixedValue;
    /// <summary>
    /// Tipo de ataque que realizará el estado
    /// </summary>
    public Constant.AttackType AttackType;
    /// <summary>
    /// Cuanto prociento de daño del tipo de ataque elegido realizará el estado
    /// </summary>
    public float ActionRate;
    /// <summary>
    /// Se remueve el efecto despues del combate?
    /// </summary>
    public bool RemoveBattleEnd;    
    /// <summary>
    /// Mensaje que mostrará cuando el estado se quede
    /// </summary>
    public string MessageRemains = string.Empty;
    /// <summary>
    /// Mensaje que mostrará cuando te recuperes del estado
    /// </summary>
    public string MessageRecovery = string.Empty;
    /// <summary>
    /// Mensaje que mostrará cuando se le aplique al personaje el estado
    /// </summary>
    public string MessageActor = string.Empty;
    /// <summary>
    /// Mensaje que mostrará cuando se le aplique al enemigo el estado
    /// </summary>
    public string MessageEnemy = string.Empty;
    /// <summary>
    /// Se elimina el estado al recibir daño?
    /// </summary>
    public bool RemoveByDamage;
    /// <summary>
    /// Cual es la probabilidad de que se elimine el estado al recibir el daño (0-100)
    /// </summary>
    public float PercentRemoveByDamage;
    /// <summary>
    /// true = se remueve por una cantidad de turnos
    /// </summary>
    public bool RemoveByTurn;
    /// <summary>
    /// Total de turnos para ser eliminado el estado
    /// </summary>
    public int TurnTotal;
    /// <summary>
    /// Contador de turnos pasados con el estado
    /// </summary>
    public int Turn;
    /// <summary>
    /// Probabilidad de aplicar la restricción del estado
    /// </summary>
    public float RestrictionRate;
    /// <summary>
    /// El estado se ha eliminado? Read-only
    /// </summary>
    public bool Removed {
        get { return _removed; }
    }
    /// <summary>
    /// Tipo de estado
    /// </summary>
    public Constant.DamageHeal Type;    

    /// <summary>
    /// Deja de aplicar el estado al actor (se remueve)
    /// </summary>
    private bool _removed = false;    
}