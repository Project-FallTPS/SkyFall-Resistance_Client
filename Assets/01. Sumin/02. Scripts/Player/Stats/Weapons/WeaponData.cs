using UnityEngine;

[System.Serializable]
public class WeaponData
{
    //public GameObject Prefab;
    public EWeaponType WeaponType;
    public float CoolTime;
    public float Damage;
    public int MaxAmmo;
    public int CurrentAmmo;
    public int ReloadInterval;
    public float ExplodeRange;
}