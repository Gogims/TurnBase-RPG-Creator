using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Usable : Item
{
    /// <summary>
    /// Si se gasta el item luego de utilizar
    /// </summary>
    public bool Consumeable;
    /// <summary>
    /// A cuantas aliados/enemigos afecta el item
    /// </summary>
    public Constant.AOE AreaOfEffect;
    /// <summary>
    /// Cuando está disponible el item para usar
    /// </summary>
    public Constant.ItemAvailable Available;
    /// <summary>
    /// Es un item clave?
    /// </summary>
    public bool KeyItem;
    /// <summary>
    /// Porcentaje para que el item sea exitoso al utilizar
    /// </summary>
    public float HitRate;
    /// <summary>
    /// Defense= Remueve Efecto. Offense = Agrega Efecto
    /// </summary>
    public Constant.OffenseDefense StateType;
    /// <summary>
    /// Listado de efectos que remueve/agrega al objetivo
    /// </summary>
    public List<AbstractState> States;
}