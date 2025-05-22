using System;
using Unity.Behavior;
using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField]
    private EBossType _bossType;
    public EBossType BossType => _bossType;
    [SerializeField] private BossDataSO _bossDataSO;
    
    private BossData _bossData;
    public BossData BossData { get => _bossData; }

    [Header("External References")] 
    private Transform _playerTransform;
    public Transform PlayerTransform => _playerTransform;
    
    private void Awake()
    {
        _bossData = _bossDataSO.GetBossData(_bossType);
        _playerTransform = GameObject.FindGameObjectWithTag(nameof(ETags.Player)).transform;
        Debug.Log($"마지막 공격 시간 in BossController : {_bossData.LastAttackTime}");
        Debug.Log($"공격 쿨타임 in OnStart : {_bossData.AttackCooltime}");
    }


    public void TakeDamage(float damage)
    {
        _bossData.CurrentHealth -= damage;
        if (_bossData.CurrentHealth <= 0)
        {
            
        }
        else
        {
            
        }
    }
}
