﻿using UnityEngine;
using System.Collections;
/// <summary>
/// Constantes de TurnBase-RPG-Creator.
/// </summary>
public class Constant{
    /// <summary>
    /// Layers que utiliza RPG-Creator para ordenar los objetos en la scene.
    /// </summary>
    #region Layers
    public const string LAYER_ITEM = "Items";
    public const string LAYER_TILE = "Tile";
    public const string LAYER_ACTOR = "Actors";
    #endregion
    /// <summary>
	/// La altura maxima del mapa
	/// </summary>
	public const int MAX_MAP_HEIGTH = 100;
	/// <summary>
	/// Ancho maximo del mapa
	/// </summary>
	public const int MAX_MAP_WIDTH = 100;
	/// <summary>
	/// Altura Minima del mapa
	/// </summary>
	public const int MIN_MAP_HEIGTH = 13;
	/// <summary>
	/// Ancho Minimo del mapa.
	/// </summary>
	public const int MIN_MAP_WIDTH = 17;
	/// <summary>
	/// Altura de la imagen del RPGInspector
	/// </summary>
	public const int INSPECTOR_IMAGE_HEIGTH = 150;
	/// <summary>
	/// Ancho de la imagen del RPGInspector
	/// </summary>
	public const int INSPECTOR_IMAGE_WIDTH = 150;
    /// <summary>
    /// Ancho de los sprites de los elementos del juego
    /// </summary>
    public const int SpriteWidth = 64;
    /// <summary>
    /// Alto de los sprites de los elementos del juego
    /// </summary>
    public const int SpriteHeight = 64;
    /// <summary>
    /// Ancho del fondo de pantalla del combate
    /// </summary>
    public const int BackgroundWidth = 1044;
    /// <summary>
    /// Alto del fondo de pantalla del combate
    /// </summary>
    public const int BackgroundHeight= 580;

    /// <summary>
    /// Crea el marco de la imagen del equipamiento
    /// </summary>
    /// <returns>Rectángulo con sus coordenadas definidas</returns>
    public static Rect GetTextureCoordinate(Sprite Image)
    {
        return new Rect(Image.textureRect.x / Image.texture.width,
                        Image.textureRect.y / Image.texture.height,
                        Image.textureRect.width / Image.texture.width,
                        Image.textureRect.height / Image.texture.height);
    }
    /// <summary>
    /// Tipos de items.
    /// </summary>
    public enum ItemType
    { 
        Armor,
        Usable,
        Weapon
    };

    public enum AttackType
    {
        MagicAttack,
        NormalAttack
    }
    public enum AOE
    {
        OneEnemy,
        TwoEnemies,
        AllEnemies,
        SingleAlly,
        TwoAllies,
        AllAllies,
        Self
    };

    public enum OffenseDefense
    {
        Offensive,
        Defensive
    };

    public enum ActionType
    {
        None,
        TargetOnlyEnemy,
        TargetOnlyAlly,
        TargetMyself,
        DoNothing
    };

    public enum DamageHeal
    {
        Damage,
        Heal
    };

    public enum ItemAvailable
    {
        Always,
        OnlyBattle,
        OnlyMenu,
        Never
    };

    public enum Attribute
    {
        None,
        HP,
        MP,
        Attack,
        Defense,
        Magic,
        MagicDefense,
        Agility,
        Luck,
        MaxHP,
        MaxMP
    };

    public enum MapObjectType
    {
        Door=9,
        House,
        Pickup,
        Obstacle,
        Tile,
        Wall
    };

    public enum EnemyType
    {
        Follower,
        Random,
        Stationary
    };
}
