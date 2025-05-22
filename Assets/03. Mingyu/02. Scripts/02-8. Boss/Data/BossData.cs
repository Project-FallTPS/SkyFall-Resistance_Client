using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossData
{
    private EBossType _bossType;
    public EBossType BossType { get => _bossType; set => _bossType = value; }

    [Header("Basic")] 
    public float MaxHealth;

    [SerializeField] 
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0f, MaxHealth);
    }

    public float MoveSpeed;

    [Header("Attack")]
    public float AttackDamage;
    public float AttackCooltime;
    private float _lastAttackTime = 0f;
    public float LastAttackTime
    {
        get => _lastAttackTime;
        set => _lastAttackTime = value;
    }

    [Header("Phase")] 
    public float MaxPhase;

    private float _currentPhase;
    public float CurrentPhase
    {
        get => _currentPhase;
        set => _currentPhase = Mathf.Clamp(value, 0f, MaxPhase);
    }

    public List<float> PhaseChangeHealth;

    [Header("Attack Logic")] 
    public float MaxRushDistance;
    public float MinRushDistance;
    public float ProjectileSpeed;
    
    public BossData(BossData original)
    {
        BossType = original.BossType;
        MaxHealth = original.MaxHealth;
        CurrentHealth = original.CurrentHealth;
        MoveSpeed = original.MoveSpeed;
        AttackDamage = original.AttackDamage;
        AttackCooltime = original.AttackCooltime;
        LastAttackTime = original.LastAttackTime;
    }
}
