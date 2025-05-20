using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AccessoryData
{
    public EAccessoryType Type;
    public GameObject Prefab;
    public List<WeaponStatEntry> Bonuses;
    private Dictionary<EStatType, float> _dataDict;

    private void Init()
    {
        _dataDict = new Dictionary<EStatType, float>();

        foreach(var bonus in Bonuses)
        {
            _dataDict.Add(bonus.Type, bonus.Value);
        }
    }

    public float GetData(EStatType type)
    {
        if(_dataDict == null)
        {
            Init();
        }

        if(_dataDict.TryGetValue(type, out var data))
        {
            return data;
        }
        else
        {
            return 1f;
        }
    }
} 