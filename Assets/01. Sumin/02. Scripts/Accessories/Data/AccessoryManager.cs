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
        if (!EquippedAccessories.ContainsKey(type))
        {
            EquippedAccessories.Add(type, GetData(type));
        }
    }

    public void UnEquip(EAccessoryType type)
    {
        if (EquippedAccessories.ContainsKey(type))
        {
            EquippedAccessories.Remove(type);
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
