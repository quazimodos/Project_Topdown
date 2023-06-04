using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunModel
{
    [Header("Visual")]
    public string name;
    public float projectileSize;
    public GameObject gunVisual;
    public int price;
    public Sprite gunSprite;
    [Header("Specs")]
    public Gun.FireMode fireMode;
    public float damage;
    public int magazineCapacity;
    public float reloadTime;
    public float secBetweenShot;
    public int projectileCount;
    public float spreadAngle;

}
