using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Formula
{
    
    
    /// <summary>
    /// Valor inicial con el que la formula empezará
    /// </summary>
    public int BaseValue = 30;
    /// <summary>
    /// Simple valor adicional que se le agrega por cada nivel
    /// </summary>
    public int ExtraValue = 20;
    /// <summary>
    /// Valor para hacer crecer los últimos niveles
    /// </summary>
    public int Acceleration = 30;
    /// <summary>
    /// Convierte la aceleración en decimal
    /// </summary>
    private double acc
    {
        get { return (double)Acceleration / 100; }
    }
    [SerializeField]
    private Constant.FormulaType type;

    public int GetValue(int Level)
    {
        int value = 0;
        int previousValue = 0;

        switch (type)
        {
            case Constant.FormulaType.XP:
                if (Level > 1)
                    previousValue = BaseValue + ((Level - 1) * ExtraValue) + (int)(acc * (Level - 1) * ExtraValue);

                value = previousValue + BaseValue + (Level * ExtraValue) + (int)(acc * Level * ExtraValue);
                break;
            default:
                value = BaseValue + (Level * ExtraValue);
                break;
        }

        return value;
    }

#if UNITY_EDITOR

    /// <summary>
    /// Valor mínimo de los sliders de la curva de experiencia
    /// </summary>
    public const int MinValue = 10;
    /// <summary>
    /// Valor máximo de los sliders de la curva de experiencia
    /// </summary>
    public const int MaxValue = 50;
    /// <summary>
    /// Máximo nivel que un personaje puede llegar
    /// </summary>
    public const int MaxLevel = 99;
    /// <summary>
    /// Estructura que contiene el (nivel, cantidad de xp para subir al próximo nivel)
    /// </summary>
    public List<int> Growth;
    /// <summary>
    /// Nombre de la curva (XP, HP, MP...)
    /// </summary>
    private string CurveName;    

    /// <summary>
    /// Constructor que agrega el valor por defecto de los atributos
    /// </summary>
    /// <param name="type">1=Max HP</param>
    public Formula(Constant.FormulaType _type)
    {
        Growth = new List<int>(MaxLevel);
        type = _type;

        if(type == Constant.FormulaType.XP)
            CurveName = "Experience";
        else if (type == Constant.FormulaType.MaxHP)
            CurveName = "Max HP";
        else if (type == Constant.FormulaType.MaxMP)
            CurveName = "Max MP";
        else if (type == Constant.FormulaType.Attack)
            CurveName = "Attack";
        else if (type == Constant.FormulaType.Defense)
            CurveName = "Defense";
        else if (type == Constant.FormulaType.MagicAttack)
            CurveName = "Magic Attack";
        else if (type == Constant.FormulaType.MagicDefense)
            CurveName = "Magic Defense";
        else if (type == Constant.FormulaType.Agility)
            CurveName = "Agility";
        else if (type == Constant.FormulaType.Luck)
            CurveName = "Luck";

        for (int i = 1; i <= MaxLevel; i++)
        {
            Growth.Add(GetStatValue(i));
        }
    }

    /// <summary>
    /// Busca en el diccionario cuanta experiencia se necesita para pasar el próximo nivel
    /// </summary>
    /// <param name="Level">Nivel que se desea acceder</param>
    /// <returns>Cantidad de nivel necesitado para el próximo nivel</returns>
    public int GetListValue(int Level)
    {
        Level--;
        return Growth[Level];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Valores necesitados para pasar al próximo nivel</returns>
    public IEnumerable<int> GenerateCurve()
    {
        foreach (var item in Growth)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Actualiza el diccionario completo a partir del valor base, valor extra y la acceleración (si aplica)
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < Growth.Count; i++)
        {
            Growth[i] = GetStatValue(i);
        }
    }

    /// <summary>
    /// Devuelve el nombre de la curva
    /// </summary>
    /// <returns>Nombre de la curva</returns>
    public string GetName()
    {
        return CurveName;
    }

    /// <summary>
    /// Devuelve el tipo de curva que es
    /// </summary>
    /// <returns>Tipo de curva</returns>
    public Constant.FormulaType GetFormulaType()
    {
        return type;
    }    

    /// <summary>
    /// Fórmula para calcular la cantidad de experiencia necesitada para pasar al próximo nivel, dado un nivel
    /// </summary>
    /// <param name="Level">Nivel del personaje</param>
    /// <returns>Cantidad de experiencia necesitada para pasar al próximo nivel</returns>
    private int GetStatValue(int level)
    {
        int value = 0;
        int previousValue = 0;

        if (type == Constant.FormulaType.XP)
        {
            level++;

            if (level > 1)
                previousValue = Growth[level - 2];

            value = previousValue + BaseValue + (level * ExtraValue) + (int)(acc * level * ExtraValue);
        }
        else
        {
            value = BaseValue + (level * ExtraValue);
        }       

        return value;  
    }
#endif  
}
