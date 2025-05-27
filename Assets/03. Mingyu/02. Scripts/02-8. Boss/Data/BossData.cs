using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

[System.Serializable]
public class BossData
{
    private EBossType _bossType;
    public EBossType BossType { get => _bossType; set => _bossType = value; }

    [Header("Basic")]
    [Tooltip("보스의 최대 체력")]
    public float MaxHealth;
    
    [Tooltip("보스의 현재 체력")]
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0f, MaxHealth);
            if ((_currentHealth / MaxHealth * 100f) <= PhaseChangeHealthPercentage[_currentPhase - 1])
            {
                CurrentPhase++;
            }
        }
    }

    [Tooltip("보스의 기본 이동 속도")]
    public float MoveSpeed;

    [Header("Attack")]
    [Tooltip("공격 시 데미지")]
    public float AttackDamage;
    [Tooltip("공격 패턴 쿨타임 - 모든 공격 패턴이 공유")]
    public float AttackCooltime;
    private float _lastAttackTime = 0f;
    public float LastAttackTime
    {
        get => _lastAttackTime;
        set => _lastAttackTime = value;
    }
    
    [Header("Attack Logic - Rush")] 
    [Tooltip("돌진 공격이 가능한 보스 - 플레이어간 최대 거리")]
    public float MaxRushDistance;
    [Tooltip("돌진 공격이 가능한 보스 - 플레이어간 최소 거리")]
    public float MinRushDistance;
    [Tooltip("돌진 속도 - 기본 이동 속도에 곱해지는 값")]
    public float RushSpeedMultiplier;
    [Tooltip("돌진 전 준비 속도(뒤로 이동하며 기 모으기) - 기본 이동 속도에 나눠지는 값")]
    public float RushSpeedDivisorForWindup;
    [Tooltip("돌진 공격의 전조 최대 이동 거리")]
    public float WindupDistance;
    [FormerlySerializedAs("WindupTime")] [Tooltip("돌진 공격의 전조 시간")]
    public float BeforeRushDelay;
    
    [Header("Attack Logic - Laser")]
    [Tooltip("레이저 공격이 가능한 보스 - 플레이어간 최소 거리")]
    public float MinLaserDistance;
    [Tooltip("레이저 공격의 전조 시간")]
    public float WindupTimeForLaser;
    [Tooltip("레이저의 사거리")]
    public float LaserRange;
    [Tooltip("레이저의 지속 시간")]
    public float LaserDuration;

    [Header("Phase")] 
    [Tooltip("보스의 최대 페이즈")]
    public int MaxPhase;

    [Header("System - Weakness Attack")] 
    public float WeaknessAttackDamageMultiplier;

    public float NormalAttackDamageDivisor;

    private int _currentPhase = 1;
    public int CurrentPhase
    {
        get => _currentPhase;
        set
        {
            _currentPhase = Mathf.Clamp(value, 0, MaxPhase);
            PhaseManager.Instance.CurrentPhase = _currentPhase;
            OnPhaseChanged?.Invoke();
        }
    }
    public Action OnPhaseChanged;

    [Tooltip("페이즈가 전환되는 보스 체력 비율, 항상 리스트 사이즈는 (페이즈 수 - 1) 이어야 한다.")]
    public List<float> PhaseChangeHealthPercentage;
    
    public BossData(BossData original)
    {
        BossType = original.BossType;
        MaxHealth = original.MaxHealth;
        MoveSpeed = original.MoveSpeed;
        AttackDamage = original.AttackDamage;
        AttackCooltime = original.AttackCooltime;
        LastAttackTime = original.LastAttackTime;
        
        MaxRushDistance = original.MaxRushDistance;
        MinRushDistance = original.MinRushDistance;
        RushSpeedMultiplier = original.RushSpeedMultiplier;
        RushSpeedDivisorForWindup = original.RushSpeedDivisorForWindup;
        WindupDistance = original.WindupDistance;
        BeforeRushDelay = original.BeforeRushDelay;
        MinLaserDistance = original.MinLaserDistance;
        MaxPhase = original.MaxPhase;
        PhaseChangeHealthPercentage = original.PhaseChangeHealthPercentage;
        
        MinLaserDistance = original.MinLaserDistance;
        WindupTimeForLaser = original.WindupTimeForLaser;
        LaserRange = original.LaserRange;
        LaserDuration = original.LaserDuration;

        WeaknessAttackDamageMultiplier = original.WeaknessAttackDamageMultiplier;
        NormalAttackDamageDivisor = original.NormalAttackDamageDivisor;
    }
}
