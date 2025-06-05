using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("State System")]
    private EnemyStateContext _enemyStateContext;
    public EnemyStateContext EnemyStateContext => _enemyStateContext;

    private Dictionary<EEnemyState, IEnemyState> _enemyStateDict;
    public Dictionary<EEnemyState, IEnemyState> EnemyStateDict { get => _enemyStateDict; set => _enemyStateDict = value; }

    [Header("Components")]
    private CapsuleCollider _enemyCollider;
    public CapsuleCollider EnemyCollider => _enemyCollider;
    
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    private Animator _enemyAnimator;
    public Animator EnemyAnimator => _enemyAnimator;

    private GameObject _shieldGameObject;

    [Header("Data")]
    [SerializeField]
    private EEnemyType _enemyType;
    [SerializeField]
    private EnemyDataSO _enemyDataSO;
    [SerializeField]
    private List<Transform> _bulletShootPositions;
    public List<Transform> BulletShootPositions { get => _bulletShootPositions; }

    private EnemyData _enemyData;
    public EnemyData EnemyData { get => _enemyData; set => _enemyData = value; }

    [Header("External References")]
    private GameObject _player;
    public GameObject Player => _player;

    private void Awake()
    {
        _enemyStateContext = new EnemyStateContext(this);
        _enemyStateDict = new Dictionary<EEnemyState, IEnemyState>();

        _enemyCollider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _enemyAnimator = GetComponent<Animator>();
        _shieldGameObject
            = GetComponentInChildren<ParticleSystem>().gameObject;
        _shieldGameObject.SetActive(false);
        _enemyData = _enemyDataSO.GetEnemyData(_enemyType);
        _player = GameObject.FindGameObjectWithTag(nameof(ETags.Player));
    }

    private void OnEnable()
    {
        
    }
    public void Init()
    {
        if (_enemyStateDict.Count == 0)
        {
            Debug.Log("적 상태 딕셔너리 초기화");
            _enemyStateDict.Add(EEnemyState.Trace, new EnemyTraceState
            (this, 
                EnemyStrategyHandler.Instance.PickTraceStrategy(), 
                EnemyStrategyHandler.Instance.EnemyTransitionStrategyDict[_enemyData.EnemyType]));
            _enemyStateDict.Add(EEnemyState.Attack, new EnemyAttackState
            (this, 
                EnemyStrategyHandler.Instance.EnemyAttackStrategyDict[_enemyData.EnemyType],
                EnemyStrategyHandler.Instance.EnemyTransitionStrategyDict[_enemyData.EnemyType]));
            _enemyStateDict.Add(EEnemyState.Damaged, new EnemyDamagedState(this));
            _enemyStateDict.Add(EEnemyState.Die, new EnemyDieState(this));
        }
        _enemyData.AdjustEnemyDataOnWave(WaveManager.Instance.CurrentWaveData.EnemyStatMultiplier);
        _enemyData.Init();
        _shieldGameObject.SetActive(false);
        _enemyStateContext.ChangeState(_enemyStateDict[EEnemyState.Trace]);
    }

    private void Update()
    {
        _enemyStateContext.CurrentState.Update();
    }

    public GameObject GameObject => gameObject;
    
    public void TakeDamage(float damage)
    {
        if (_enemyData.IsShieldActive)
        {
            DeactivateShield();
            return;
        }

        SoundManager.Instance.PlaySfx(ESfxType.EnemyHit1, transform.position);
        
        _enemyData.CurrentHealth -= damage;
        if (_enemyData.CurrentHealth <= 0)
        {
            _enemyStateContext.ChangeState(_enemyStateDict[EEnemyState.Die]);
        }
        else
        {
            _enemyStateContext.ChangeState(_enemyStateDict[EEnemyState.Damaged]);
        }
    }

    public void ActivateShield()
    {
        _enemyData.IsShieldActive = true;
        _shieldGameObject.SetActive(true);
    }

    public void DeactivateShield()
    {
        _enemyData.IsShieldActive = false;
        _shieldGameObject.SetActive(false);
    }
    
    public void StartCoroutineInEnemyState(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void StopCoroutineInEnemyState(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }
} 