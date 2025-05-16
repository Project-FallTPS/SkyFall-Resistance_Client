using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatCollectionSO", menuName = "Scriptable Objects/PlayerStatCollectionSO")]

public class PlayerStatCollectionSO : ScriptableObject // 플레이어 전체 스탯 정보
{
    [SerializeField] private List<PlayerStatEntry> StatList;

    public Dictionary<EStatType, float> GetBaseStatDict()
    {
        var dict = new Dictionary<EStatType, float>();
        foreach (var stat in StatList)
        {
            dict[stat.Type] = stat.Value;
        }
        return dict;
    }

    //public float GetValue(EStatType type)
    //{
    //    /*if(Stats.TryGetValue(type, out float value))
    //    {
    //        return value;
    //    }
    //    else
    //    {
    //        return 0f;
    //    }*/
    //    return StatDict[type];
    //}

    //public void SetValue(EStatType type, float value) => StatDict[type] = value;
}