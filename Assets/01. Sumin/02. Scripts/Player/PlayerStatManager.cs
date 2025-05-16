using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : Singleton<PlayerStatManager>
{
    [Header("# Project")]
    [SerializeField] private PlayerStatCollectionSO _playerStatCollection; // ����
    public Dictionary<EStatType, float> StatDict { get; private set; } // ĳ��

    protected override void Awake()
    {
        base.Awake();
        PerkManager.Instance.CalculateFinalStats(StatDict);
    }

    public float GetStat(EStatType type)
    {
        return StatDict.TryGetValue(type, out var value) ? value : -1f;
    }
}