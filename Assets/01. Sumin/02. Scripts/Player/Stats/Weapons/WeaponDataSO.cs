using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Scriptable Objects/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    public List<WeaponData> WeaponDatas;

    private Dictionary<EWeaponType, WeaponData> _weaponDict;

    public void Init()
    {
        _weaponDict = new Dictionary<EWeaponType, WeaponData>();

        foreach (var data in WeaponDatas)
        {
            data.Init(); // 각 무기 스탯 초기화

            if (!_weaponDict.ContainsKey(data.WeaponType))
                _weaponDict.Add(data.WeaponType, data);
        }
    }

    public WeaponData GetWeapon(EWeaponType type)
    {
        if (_weaponDict == null)
        {
            Init();
        }

        if (_weaponDict.TryGetValue(type, out var data))
            return data;

        Debug.LogWarning($"Weapon ID '{type}' not found!");
        return null;
    }
}
