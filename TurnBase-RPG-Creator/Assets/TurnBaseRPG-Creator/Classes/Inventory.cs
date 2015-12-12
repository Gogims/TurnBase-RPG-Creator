using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// Inventorio del jugador
/// </summary>
[Serializable]
public class Inventory {
    /// <summary>
    /// Constructor del inventorio
    /// </summary>
    public Inventory () {
        Armors = new Dictionary<string,Tuple<AbstractArmor,int>>();
        Usables = new Dictionary<string, Tuple<AbstractUsable, int>>();
        Weapons = new Dictionary<string, Tuple<AbstractWeapon, int>>();

    }
    /// <summary>
    /// Lista de armor
    /// </summary>
    public Dictionary<string, Tuple<AbstractArmor, int>> Armors;
    /// <summary>
    /// Lista de Usables
    /// </summary>
    public Dictionary<string,Tuple<AbstractUsable,int>> Usables;
    /// <summary>
    /// Lista de weapons
    /// </summary>
    public Dictionary<string, Tuple<AbstractWeapon, int>> Weapons;
    /// <summary>
    /// Retorna una lista de armor dado un tipo
    /// </summary>
    /// <param name="type"> Tipo de armor</param>
    /// <returns>lista de armors</returns>
    public List<Tuple<AbstractArmor, int>> TypeArmor(AbstractArmor.ArmorType type)
    {
        List<Tuple<AbstractArmor, int>> returnList = new List<Tuple<AbstractArmor, int>>();
        foreach (string key in Armors.Keys) {
            AbstractArmor i = Armors[key].First;
            int cant = Armors[key].Second;
            if (i.Type == type)
                returnList.Add(new Tuple<AbstractArmor,int>(i,cant));
        }
        return returnList;
    }
    /// <summary>
    /// Retorna el lista de armas y su cantidad en el inventorio
    /// </summary>
    /// <returns></returns>
    public List<Tuple<AbstractWeapon, int>> GetWeapons() {
        List<Tuple<AbstractWeapon, int>> returnList = new List<Tuple<AbstractWeapon, int>>();
        foreach (string key in Weapons.Keys)
        {
            AbstractWeapon i = Weapons[key].First;
            int cant = Weapons[key].Second;
            returnList.Add(new Tuple<AbstractWeapon, int>(i, cant));
        }
        return returnList;
    }
    /// <summary>
    /// Retorna la lista de usables y su cantidad
    /// </summary>
    /// <returns></returns>
    public List<Tuple<AbstractUsable, int>> GetUsables()
    {
        List<Tuple<AbstractUsable, int>> returnList = new List<Tuple<AbstractUsable, int>>();
        foreach (string key in Usables.Keys)
        {
            AbstractUsable i = Usables[key].First;
            int cant = Usables[key].Second;
            returnList.Add(new Tuple<AbstractUsable, int>(i, cant));
        }
        return returnList;
    }
    public void DeleteItem(string name)
    {
        if (Armors.ContainsKey(name))
        {
            Armors[name].Second--;
            if (Armors[name].Second <= 0)
                Armors.Remove(name);
        }
        else if (Weapons.ContainsKey(name))
        {
            Weapons[name].Second--;
            if (Weapons[name].Second <= 0)
                Weapons.Remove(name);
        }
        if (Usables.ContainsKey(name))
        {
            Usables[name].Second--;
            if (Usables[name].Second <= 0)
                Usables.Remove(name);
        }
    }
    /// <summary>
    /// Inserta un usable en el inventorio
    /// </summary>
    /// <param name="u">Usable a insertar</param>
    public void InsertUsable(AbstractUsable u) {
        if (Usables.ContainsKey(u.ItemName))
        {
            Usables[u.ItemName].Second++;
        }
        else
        {
            Usables[u.ItemName] = new Tuple<AbstractUsable, int>(u, 1);
        }
    }
    /// <summary>
    /// Inserta una armadura en el inventorio
    /// </summary>
    /// <param name="a">Armadura a insertar</param>
    public void InsertArmor(AbstractArmor a ) {
        if (Armors.ContainsKey(a.ItemName))
        {
            Armors[a.ItemName].Second++;
        }
        else
        {
            Armors[a.ItemName] = new Tuple<AbstractArmor, int>(a, 1);
        }
    }
    /// <summary>
    /// Inserta un arma al inventrio
    /// </summary>
    /// <param name="w">Arma a insertar</param>
    public void InsertWeapon(AbstractWeapon w) {
        if (Weapons.ContainsKey(w.ItemName))
        {
            Weapons[w.ItemName].Second++;
        }
        else
        {
            Weapons[w.ItemName] = new Tuple<AbstractWeapon, int>(w, 1);
        }
    }
}
