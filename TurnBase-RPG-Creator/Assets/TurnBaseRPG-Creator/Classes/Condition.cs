using UnityEngine;
using System.Collections;

/// <summary>
///  Condiciones de errores 
/// </summary>
public enum ErrorCondition {
    /// <summary>
    /// Representa la pregunta mayor que
    /// </summary>
    Greater = 1,
    /// <summary>
    /// Representa la pregunta mayor o igual que
    /// </summary>
    GreaterOrEqual,
    /// <summary>
    /// Representa la pregunta menor que
    /// </summary>
    Less,
    /// <summary>
    /// Representa la pregunta menor o igual que
    /// </summary>
    LessOrEqual,
    /// <summary>
    /// Representa la pregunta de igual que
    /// </summary>
    Equal,
    /// <summary>
    /// Representa la pregunta diferente que.
    /// </summary>
    Different
}
