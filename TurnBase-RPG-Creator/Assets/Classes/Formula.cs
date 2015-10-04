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
    /// 
    /// </summary>
    public int Acceleration = 30;

    /// <summary>
    /// Notifica si uno de los valores que comprenden la formula a cambiado, para actualizarlos
    /// </summary>
    public bool updated;

    public const int MinValue = 10;

    public const int MaxValue = 50;

    private Dictionary<int, int> Growth;

    public Formula()
    {
        Growth = new Dictionary<int, int>(99);

        for (int i = 1; i <= 99; i++)
        {
            Growth.Add(i, GetLevelValue(i));
        }
    }

    public int GetValue(int Level)
    {
        return Growth[Level];
    }

    public IEnumerable<int> GenerateCurve()
    {
        foreach (var item in Growth)
        {
            yield return item.Value;
        }
    }

    public void UpdateValue()
    {
        for (int i = 1; i <= Growth.Count; i++)
        {
            Growth[i] = GetLevelValue(i);
        }
    }    

    private int GetLevelValue(int Level)
    {
        int previouslevel = 0;

        if (Level > 1)
            previouslevel = Growth[Level - 1];

        return previouslevel + BaseValue + (Level * ExtraValue) ^ (1 + (Acceleration / 100));
    }
}
