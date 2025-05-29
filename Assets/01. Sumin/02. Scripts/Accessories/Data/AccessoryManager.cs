using System.Collections.Generic;
using UnityEngine;

public class AccessoryManager : Singleton<AccessoryManager>
{
    [SerializeField] private AccessoryDataSO _dataSO;

    public Dictionary<EAccessoryType, ActiveAccessory> EquippedAccessories { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        EquippedAccessories = new Dictionary<EAccessoryType, ActiveAccessory>();
    }

    public void Equip(EAccessoryType type, IAccessory obj)
    {
        ActiveAccessory acc = null;

        if (!EquippedAccessories.ContainsKey(type))
        {
            acc = new ActiveAccessory(GetData(obj.Type), obj);
            acc.Count = 1;
            EquippedAccessories.Add(type, acc);

            obj.OnEquip();
        }
        else
        {
            acc = EquippedAccessories[type];
            acc.Count++;

            // 같은 오브젝트면 반납하지 않음
            if (acc.Object != obj)
            {
                MonoBehaviour mono = obj as MonoBehaviour;
                if (mono != null)
                {
                    AccessoryPoolManager.Instance.ReturnObject(mono.gameObject, type);
                }
            }
            acc.Object.OnEquip(); // 기존 것에 대해 다시 호출
        }
    }

    public void UnEquip(EAccessoryType type)
    {
        if (EquippedAccessories.ContainsKey(type))
        {
            ActiveAccessory acc = EquippedAccessories[type];
            acc.Count--;
            if(acc.Count <= 0)
            {
                EquippedAccessories.Remove(type);
            }
        }
    }

    public bool IsEquipped(EAccessoryType type)
    {
        return EquippedAccessories.ContainsKey(type);
    }

    public ActiveAccessory GetAccessory(EAccessoryType type)
    {
        if (EquippedAccessories.TryGetValue(type, out var acc))
        {
            return acc;
        }
        else return null;
    }

    public AccessoryData GetData(EAccessoryType type)
    {
        return _dataSO.GetData(type);
    }

    public List<ActiveAccessory> GetEquippedAccessories(EWeaponType type)
    {
        var filtered = new List<ActiveAccessory>();

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
