using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour, IDamageable
{
    [Header("Data")]
    [SerializeField]
    private EBossType _bossType;
    public EBossType BossType => _bossType;
    [SerializeField] private BossDataSO _bossDataSO;
    
    private BossData _bossData;
    public BossData BossData { get => _bossData; }
    
    [Header("Component")]
    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; }
    
    private Animator _animator;
    public Animator Animator { get => _animator; }

    [Header("External References")] 
    private Transform _playerTransform;
    public Transform PlayerTransform => _playerTransform;
    
    private void Awake()
    {
        _bossData = _bossDataSO.GetBossData(_bossType);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _bossData.MoveSpeed;
        _animator = GetComponent<Animator>();
        _playerTransform = GameObject.FindGameObjectWithTag(nameof(ETags.Player)).transform;
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
