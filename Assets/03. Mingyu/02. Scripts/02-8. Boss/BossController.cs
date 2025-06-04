using System;
using System.Collections.Generic;
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
    
    [SerializeField]
    private List<Transform> _bulletShootPositions;
    public List<Transform> BulletShootPositions { get => _bulletShootPositions; }
    
    [Header("Weakness Points")]
    private List<Collider> _weaknessPoints = new List<Collider>();

    public List<Collider> WeaknessPoints => _weaknessPoints;


    [Header("# UI Event")]
    public Action<float> OnHit;

    // 추가한 내용
    public static Action<float, float, int> OnBossHealthChange;

    private void Awake()
    {
        Init();
    }

    public GameObject GameObject => gameObject;

    public void TakeDamage(float damage)
    {
        _bossData.CurrentHealth -= damage;
        if (_bossData.CurrentHealth <= 0)
        {
            _animator.SetBool(nameof(EBossAnimationParam.Death), true);
            Debug.Log("Boss Dead");
        }
        else
        {
            _animator.SetTrigger(nameof(EBossAnimationParam.HitLeftTrigger));
            Debug.Log("Boss Damaged");
        }
        OnHit?.Invoke(damage);

        // 추가한 부분 <Health Invoke>
        OnBossHealthChange?.Invoke(_bossData.CurrentHealth, _bossData.MaxHealth, _bossData.CurrentPhase);
    }

    private void Init()
    {
        _animator = GetComponent<Animator>();
        _playerTransform = GameObject.FindGameObjectWithTag(nameof(ETags.Player)).transform;
        InitBossData();
        InitNavMesh();
        InitWeaknessColliders();
        AddOnPhaseChangedEvents();
    }

    private void InitBossData()
    {
        _bossData = _bossDataSO.GetBossData(_bossType);
        _bossData.CurrentHealth = _bossData.MaxHealth;
    }
    private void InitNavMesh()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _bossData.MoveSpeed;
        _navMeshAgent.updateRotation = true;
    }

    private void InitWeaknessColliders()
    {
        Collider[] weaknessColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in weaknessColliders)
        {
            _weaknessPoints.Add(collider);
        }
    }

    private void AddOnPhaseChangedEvents()
    {
        _bossData.OnPhaseChanged += ActivateWeaknessColliders;
    }

    public void ActivateWeaknessColliders()
    {
        if (_bossData.CurrentPhase != 3)
        {
            return;
        }

        foreach (var weaknessCollider in _weaknessPoints)
        {
            weaknessCollider.enabled = true;
        }
    }

    public void DeactivateWeaknessColliders()
    {
        foreach (var weaknessCollider in _weaknessPoints)
        {
            weaknessCollider.enabled = false;
        }
    }
}