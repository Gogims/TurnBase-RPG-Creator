using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 
/// </summary>
public abstract class AbstractNavigator : MonoBehaviour {
    /// <summary>
    /// Indice de la opcion seleccionada.
    /// </summary>
    protected int selected = 0;
    /// <summary>
    /// Delay para el cambio de una opcion a otra.
    /// </summary>
    protected int delay;
    /// <summary>
    /// Imagen que se va mostrar como selector del menu.
    /// </summary>
    protected GameObject Arrow;
    public virtual void update() { }
    public virtual void Init(GameObject arrow, List<GameObject> options) { }
    public virtual void Init(Vector3 position, Vector3 positionImage,Vector3 positionCant, int difY, int difX, int maxItem, GameObject arrow, GameObject nextArrow, GameObject prevArrow, GameObject panel) { }
}
