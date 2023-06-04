using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "CreateItem/Card", order = 1)]
public class Card : ScriptableObject
{
    public string cardName;
    [TextArea(3, 6)] public string description;
    public Sprite cardImage;
    public Modifier[] modifiers;

}

[Serializable]
public class Modifier
{
    public ModifierType type;
    [Range(0, 1)] public float multiplier;
}


public enum ModifierType
{
    Health,
    ReloadSpeed,
    PlayerSpeed,
    TimeBetweenShots,
    MagazineCapacity,
    Damage
}