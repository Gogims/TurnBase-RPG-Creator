using System;
using System.Collections.Generic;
using UnityEngine;


public class Formula : MonoBehaviour
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
    /// Notifica si uno de los valores que comprenden la formula a cambiado, para actualizarlos
    /// </summary>
    public bool updated;

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
    private Dictionary<int, int> Growth;

    /// <summary>
    /// Convierte la aceleración en decimal
    /// </summary>
    private double acc
    {
        get { return (double)Acceleration / 100; }
    }

    /// <summary>
    /// Constructor que agrega el nivel máximo de items en el diccionario y le asigna un valor por defecto
    /// </summary>
    public Formula()
    {
        Growth = new Dictionary<int, int>(MaxLevel);

        for (int i = 1; i <= MaxLevel; i++)
        {
            Growth.Add(i, GetLevelValue(i));
        }
    }

    /// <summary>
    /// Busca en el diccionario cuanta experiencia se necesita para pasar el próximo nivel
    /// </summary>
    /// <param name="Level">Nivel que se desea acceder</param>
    /// <returns>Cantidad de nivel necesitado para el próximo nivel</returns>
    public int GetValue(int Level)
    {
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
            yield return item.Value;
        }
    }

    /// <summary>
    /// Actualiza el diccionario completo a partir del valor base, valor extra y la acceleración
    /// </summary>
    public void UpdateValue()
    {
        for (int i = 1; i <= Growth.Count; i++)
        {
            Growth[i] = GetLevelValue(i);
        }
    }

    /// <summary>
    /// Fórmula para calcular la cantidad de experiencia necesitada para pasar al próximo nivel, dado un nivel
    /// </summary>
    /// <param name="Level">Nivel del personaje</param>
    /// <returns>Cantidad de experiencia necesitada para pasar al próximo nivel</returns>
    private int GetLevelValue(int Level)
    {
        int previouslevel = 0;

        if (Level > 1)
            previouslevel = Growth[Level - 1];

        return previouslevel + BaseValue + (Level * ExtraValue) + (int)(acc * Level * ExtraValue);
    }
}
