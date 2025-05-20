using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public EWeaponType WeaponType;
    public int CurrentAmmo;  // 변동 가능한 필드
    public List<WeaponStatEntry> Stats;  // ScriptableObject에 저장할 스탯 리스트

    private Dictionary<EStatType, float> _statDict;

    public void Init()
    {
        _statDict = new Dictionary<EStatType, float>();

        foreach (var entry in Stats)
        {
            if (!_statDict.ContainsKey(entry.Type))
                _statDict.Add(entry.Type, entry.Value);
            else
                Debug.LogWarning($"중복된 스탯 타입이 감지됨: {entry.Type}");
        }

        if (_statDict.TryGetValue(EStatType.MaxAmmo, out var maxAmmo))
            CurrentAmmo = (int)maxAmmo;
    }

    public float GetStat(EStatType statType)
    {
        return _statDict != null && _statDict.TryGetValue(statType, out var value) ? value : 0f;
    }
}
