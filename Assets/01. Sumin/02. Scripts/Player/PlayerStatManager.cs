using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : Singleton<PlayerStatManager>
{
    [Header("# Project")]
    [SerializeField] private PlayerStatCollectionSO _playerStatCollection; // ¿øº»
    public Dictionary<EStatType, float> StatDict { get; private set; } // Ä³½Ì

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