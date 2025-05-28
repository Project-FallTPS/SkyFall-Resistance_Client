using System.Collections.Generic;
using UnityEngine;

public class AccessoryManager : Singleton<AccessoryManager>
{
    [SerializeField] private AccessoryDataSO _dataSO;

    public Dictionary<EAccessoryType, AccessoryData> EquippedAccessories { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        EquippedAccessories = new Dictionary<EAccessoryType, AccessoryData>();
    }

    public void Equip(EAccessoryType type)
    {
        AccessoryData data = GetData(type);
        if (!EquippedAccessories.ContainsKey(type))
        {
            data.Count = 1;
            EquippedAccessories.Add(type, data);
        }
        else
        {
            data.Count++;
            EquippedAccessories[type] = data;
        }
    }

    public void UnEquip(EAccessoryType type)
    {
        if (EquippedAccessories.ContainsKey(type))
        {
            AccessoryData data = GetData(type);
            data.Count--;
            if(data.Count <= 0)
            {
                EquippedAccessories.Remove(type);
            }
        }
    }

    public AccessoryData GetData(EAccessoryType type)
    {
        return _dataSO.GetData(type);
    }

    public List<AccessoryData> GetEquippedAccessories(EWeaponType type)
    {
        var filtered = new List<AccessoryData>();

        foreach (var accessory in EquippedAccessories.Values)
        {
            string name = accessory.Type.ToString();
            if (name.StartsWith(type.ToString()))
            {
                filtered.Add(accessory);
            }
        }

        return filtered;
    }
}
