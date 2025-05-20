using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : Singleton<WeaponDataManager>
{
    [SerializeField] private WeaponDataSO _weaponDataSO;

    public Dictionary<EWeaponType, WeaponData> WeaponDataDict;

    private void InitWeaponData()
    {
        _weaponDataSO.Init();  // SO 내부의 Init
        WeaponDataDict = new Dictionary<EWeaponType, WeaponData>();

        foreach (var weapon in _weaponDataSO.WeaponDatas)
        {
            weapon.Init();  // StatEntry → Dictionary 변환
            if (!WeaponDataDict.ContainsKey(weapon.WeaponType))
                WeaponDataDict.Add(weapon.WeaponType, weapon);
        }
    }

    public WeaponData GetWeaponData(EWeaponType type)
    {
        if(WeaponDataDict == null)
        {
            InitWeaponData();
        }

        if (WeaponDataDict.TryGetValue(type, out var data))
            return data;

        Debug.LogWarning($"WeaponDataManager: '{type}' 무기 데이터를 찾을 수 없습니다.");
        return null;
    }

    public float GetStat(EWeaponType type, EStatType statType)
    {
        var data = GetWeaponData(type);
        return data != null ? data.GetStat(statType) : 0f;
    }
}
