using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AccessoryData
{
    public EAccessoryType Type;
    public GameObject Prefab;
    public List<WeaponStatEntry> Bonuses;
} 