using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// Prioridad del estado (1 = más alto)
    /// </summary>
    public int Priority;
    /// <summary>
    /// Nombre del estado
    /// </summary>
    public string State = string.Empty;
    /// <summary>
    /// Acción que realiza el estado
    /// </summary>
    public Constant.ActionType ActionRestriction;
    /// <summary>
    /// Valor fijo que realizará el estado
    /// </summary>
    public int Constant;
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
    /// Se elimina el estado al caminar?
    /// </summary>
    public bool RemoveByWalking;
    /// <summary>
    /// Cuantos pasos necesita realizar para que se elimine el estado
    /// </summary>
    public int NumberOfSteps;
    /// <summary>
    /// Cuantos pasos se necesita para que se active el estado
    /// </summary>
    public int ActiveOnSteps;
    /// <summary>
    /// Contador de pasos
    /// </summary>
    public int Steps;
    /// <summary>
    /// Se elimina el estado al recibir daño?
    /// </summary>
    public bool RemoveByDamage;
    /// <summary>
    /// Cual es la probabilidad de que se elimine el estado al recibir el daño (0-1)
    /// </summary>
    public float PercentRemoveByDamage;
    /// <summary>
    /// 0=none, 1=Inicio del Turno, 2= Al final del turno
    /// </summary>
    public Constant.TurnTiming AutoRemovalTiming;
    /// <summary>
    /// Rango de turnos que tomará para remover el estado (rango inferior)
    /// </summary>
    public int DurationStartTurn;
    /// <summary>
    /// Rango de turnos que tomará para remover el estado (rango superior)
    /// </summary>
    public int DurationEndTurn;
    /// <summary>
    /// Contador de turnos pasados con el estado
    /// </summary>
    public int Turn;
    /// <summary>
    /// Probabilidad de atacar a tu aliado
    /// </summary>
    public float PercentageAttackAlly;
    /// <summary>
    /// El estado se ha eliminado? Read-only
    /// </summary>
    public bool Removed {
        get { return _removed; }
    }
    /// <summary>
    /// Momento en el que el estado se activa
    /// </summary>
    public Constant.TriggerTurnType TriggerTurn;
    /// <summary>
    /// Tipo de estado
    /// </summary>
    public Constant.DamageHeal Type;

    /// <summary>
    /// Deja de aplicar el estado al actor (se remueve)
    /// </summary>
    private bool _removed;  
}