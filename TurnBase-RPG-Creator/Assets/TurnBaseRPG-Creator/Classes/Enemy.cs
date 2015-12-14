using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : RPGElement
{
    public BattleEnemy BattleEnemy;

    public Enemy()
    {
        BattleEnemy = new BattleEnemy();
    }

    public void Start()
    {
        Vector2 WorldScreen = new Vector2(Camera.main.orthographicSize * 2 / Screen.height * Screen.width, Camera.main.orthographicSize * 2);
        Sprite image = GetComponent<SpriteRenderer>().sprite;
        float imageWidth = image.textureRect.width / 2;
        float imageHeight = image.textureRect.height / 2;

        transform.localScale = new Vector2(WorldScreen.x / Constant.BackgroundWidth, 
                                            WorldScreen.y / Constant.BackgroundHeight);

        Vector2 Position = new Vector2();
        Position.x = (WorldScreen.x / 2) * BattleEnemy.RelativePosition.x + imageWidth - ((1-transform.localScale.x) * imageWidth);
        Position.y = (WorldScreen.y / 2) * BattleEnemy.RelativePosition.y - imageHeight + ((1 - transform.localScale.y) * imageHeight);

        transform.position = Position;
    }

    public AbstractAbility AttackSelected()
    {
        AbstractAbility selected = null;
        foreach (AbstractAbility i in BattleEnemy.Data.Job.Abilities){
            if (BattleEnemy.Data.MP < i.MPCost)
            {
                continue;
            }
            else if (selected != null && selected.AttackPower < i.AttackPower)
            {
                selected = i;
            }
            else
            {
                selected = i;
            }
        }
        return selected;
    }

    public List<AbstractUsable> getItem()
    {
        List<AbstractUsable> returnVal = new List<AbstractUsable>();
        foreach (var i in this.BattleEnemy.Data.Items)
        {
            if (i.ApplyRate >= UnityEngine.Random.Range(1, 100)) {
                AbstractUsable newVal = new AbstractUsable();
                newVal.AreaOfEffect = i.Element.AreaOfEffect;
                newVal.Attribute = i.Element.Attribute;
                newVal.Available = i.Element.Available;
                newVal.Available= i.Element.Available;
                newVal.Description= i.Element.Description;
                newVal.HitRate = i.Element.HitRate;
                newVal.Image= i.Element.Image;
                newVal.ItemName = i.Element.ItemName;
                newVal.KeyItem = i.Element.KeyItem;
                newVal.Power = i.Element.Power;
                newVal.Price= i.Element.Price;
                newVal.States= i.Element.States;
                newVal.StateType= i.Element.StateType;
                returnVal.Add(newVal);
            }
        }
        return returnVal;
    }
}

[Serializable]
public class BattleEnemy
{
    /// <summary>
    /// Información del enemigo
    /// </summary>
    public AbstractEnemy Data;

    /// <summary>
    /// Position donde se presenta el enemigo
    /// </summary>
    public Vector2 EnemyPosition;
    
    public Vector2 RelativePosition;

    public BattleEnemy()
    {
        Data = new AbstractEnemy();
        EnemyPosition = new Vector2();
        RelativePosition = new Vector2();
    }
}

[Serializable]
public class AbstractEnemy : AbstractActor
{
    public int RewardExperience;
    public int RewardCurrency;
    public List<Rate> Items;    

    public AbstractEnemy()
    {
        Items = new List<Rate>();
    }
}

[Serializable]
public class Rate
{
    /// <summary>
    /// Elemento que tiene un porcentage para ser aplicado
    /// </summary>
    public AbstractUsable Element;

    /// <summary>
    /// Porcentage para aplicar el elemento
    /// </summary>
    public float ApplyRate;

    public Rate()
    {
        Element = new AbstractUsable();
    }
}