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
    public string State = "";
    /// <summary>
    /// Acción que realiza el estado
    /// </summary>
    public ActionType ActionRestriction;
    /// <summary>
    /// Daño fijo que realizará el estado
    /// </summary>
    public int ConstantDamage;
    /// <summary>
    /// Cuanto prociento de daño de tu ataque realizará el estado
    /// </summary>
    public float PercentageDamage;
    /// <summary>
    /// Se remueve el efecto despues del combate?
    /// </summary>
    public bool RemoveBattleEnd;    
    /// <summary>
    /// Mensaje que mostrará cuando el estado se quede
    /// </summary>
    public string MessageRemains;
    /// <summary>
    /// Mensaje que mostrará cuando te recuperes del estado
    /// </summary>
    public string MessageRecovery;
    /// <summary>
    /// Mensaje que mostrará cuando se le aplique al personaje el estado
    /// </summary>
    public string MessageActor;
    /// <summary>
    /// Mensaje que mostrará cuando se le aplique al enemigo el estado
    /// </summary>
    public string MessageEnemy;
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
    public RemovalTiming AutoRemovalTiming;
    /// <summary>
    /// Rango de turnos que tomará para remover el estado (rango inferior)
    /// </summary>
    public int DurationStartTurn;
    /// <summary>
    /// Rango de turnos que tomará para remover el estado (rango superior)
    /// </summary>
    public int DurationEndTurn;
    /// <summary>
    /// Probabilidad de atacar a tu aliado
    /// </summary>
    public float PercentageAttackAlly;

    public enum RemovalTiming
    {
        None,
        InicioTurno,
        FinalTurno        
    }

    public enum ActionType
    {
       AttackEnemy,
       AttackEnemyOrAlly,
       AttackAlly,
       SelfDamage,
       UnableToAct
    };
}