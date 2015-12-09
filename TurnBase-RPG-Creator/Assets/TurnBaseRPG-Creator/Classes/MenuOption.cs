﻿using UnityEngine;
using System.Collections;
using System;


public abstract class MenuOption: MonoBehaviour{
    public virtual void OnSelect()
    {
    }
    public virtual void UnSelect() 
    {
        GameObject.Find(Constant.LastSceneLoaded).transform.FindChild("Canvas").GetComponent<Menus>().unSelect();
    }
    public virtual void On(Equippable selected) 
    { 

    }
    public virtual void On(Item selected)
    {

    }
    public virtual void On(string name)
    {

    }
    public virtual void Off(string name) 
    { 

    }


    public virtual void On(AbstractAbility p)
    {
       
    }
}
