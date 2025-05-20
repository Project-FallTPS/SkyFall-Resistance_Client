using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponData
{
    //public GameObject Prefab;
    public EWeaponType WeaponType;
    //public List<StatBonus> Stats;
    public float CoolTime;
    public float Damage;
    public int MaxAmmo;
    public int CurrentAmmo;
    public int ReloadInterval;
    public float ExplodeRange;
}