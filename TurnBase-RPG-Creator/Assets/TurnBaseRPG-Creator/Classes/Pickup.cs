using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Representa un item en el mapa.
/// </summary>
public class Pickup : MapObject
{
    
    public Pickup() {
        ItemUsable = new AbstractUsable();
        ItemArmor = new AbstractArmor();
        ItemWeapon = new AbstractWeapon();
    }
    /// <summary>
    /// items que da el objeto.
    /// </summary>
    #region Items
    public AbstractUsable ItemUsable;
    public AbstractArmor ItemArmor;
    public AbstractWeapon ItemWeapon;
    #endregion
}
