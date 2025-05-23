using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossDataSO", menuName = "Scriptable Objects/BossDataSO")]
public class BossDataSO : ScriptableObject
{
    public List<BossData> BossDatas;
    private Dictionary<EBossType, BossData> _bossDataDict;

    public void Init()
    {
        _bossDataDict = new Dictionary<EBossType, BossData>();
        foreach (var bossData in BossDatas)
        {
            if (!_bossDataDict.ContainsKey(bossData.BossType))
            {
                _bossDataDict.Add(bossData.BossType, bossData);
            }
                
        }
    }

    public BossData GetBossData(EBossType bossType)
    {
        if (_bossDataDict == null)
        {
            Init();
        }

        if (_bossDataDict.TryGetValue(bossType, out var bossData))
        {
            return new BossData(bossData); // 복사본 반환
        }
        
        Debug.LogWarning($"Boss ID '{bossType}' not found!");
        return null;
    }
}
