using System.Collections.Generic;
using UnityEngine;

public struct EquippedAccessory
{
    public AccessoryData Data;
    public IAccessory Object;

    public EquippedAccessory(AccessoryData data, IAccessory obj)
    {
        Data = data;
        Object = obj;
    }
}

public class AccessoryManager : Singleton<AccessoryManager>
{
    [SerializeField] private AccessoryDataSO _dataSO;

    public Dictionary<EAccessoryType, EquippedAccessory> EquippedAccessories { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        EquippedAccessories = new Dictionary<EAccessoryType, EquippedAccessory>();
    }

    public void Equip(EAccessoryType type, IAccessory obj)
    {
        EquippedAccessory acc = new EquippedAccessory(GetData(obj.Type), obj);
        if (!EquippedAccessories.ContainsKey(type))
        {
            acc.Data.Count = 1;
            EquippedAccessories.Add(type, acc);
        }
        else
        {
            acc.Data.Count++;
            EquippedAccessories[type] = acc;
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

    public List<EquippedAccessory> GetEquippedAccessories(EWeaponType type)
    {
        var filtered = new List<EquippedAccessory>();

        foreach (var accessory in EquippedAccessories.Values)
        {
            string name = accessory.Data.Type.ToString();
            if (name.StartsWith(type.ToString()))
            {
                filtered.Add(accessory);
            }
        }

        return filtered;
    }
}
