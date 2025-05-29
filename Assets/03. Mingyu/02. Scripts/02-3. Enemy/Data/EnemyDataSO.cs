using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> EnemyDatas;
    private Dictionary<EEnemyType, EnemyData> _enemyDict;

    public void Init()
    {
        _enemyDict = new Dictionary<EEnemyType, EnemyData>();
        foreach (var data in EnemyDatas)
        {
            if (!_enemyDict.ContainsKey(data.EnemyType))
            {
                data.NextAttackableTime = 0f;
                data.CurrentHealth = data.MaxHealth;
                data.IsShieldActive = false;
                _enemyDict.Add(data.EnemyType, data);
            }
        }
    }
    public EnemyData GetEnemyData(EEnemyType type)
    {
        if (_enemyDict == null)
        {
            Init();
        }
        if (_enemyDict.TryGetValue(type, out var data))
        {
            return new EnemyData(data);
        }
        Debug.LogWarning($"Enemy ID '{type}' not found!");
        return null;
    }
}
