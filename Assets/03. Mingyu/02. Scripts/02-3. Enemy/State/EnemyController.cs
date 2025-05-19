using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("State System")]
    private EnemyStateContext _enemyStateContext;
    public EnemyStateContext EnemyStateContext => _enemyStateContext;

    private Dictionary<EEnemyState, IEnemyState> _enemyStateDict;
    public Dictionary<EEnemyState, IEnemyState> EnemyStateDict { get => _enemyStateDict; set => _enemyStateDict = value; }

    [Header("Components")]
    private CapsuleCollider _enemyCollider;
    public CapsuleCollider EnemyCollider => _enemyCollider;

    private EnemyData _enemyData;
    public EnemyData EnemyData { get => _enemyData; set => _enemyData = value; }

    [Header("References")]
    private GameObject _player;
    public GameObject Player => _player;

    private void Awake()
    {
        _enemyStateContext = new EnemyStateContext(this);
        _enemyStateDict = new Dictionary<EEnemyState, IEnemyState>();
        _enemyCollider = GetComponent<CapsuleCollider>();
        _enemyData = GetComponent<EnemyData>();
        _player = GameObject.FindGameObjectWithTag(nameof(ETags.Player));
    }

    private void OnEnable()
    {
        if (_enemyStateDict.Count != 0)
        {
            _enemyStateContext.ChangeState(_enemyStateDict[EEnemyState.Trace]);
        }
    }
    private void Start()
    {
        _enemyStateDict.Add(EEnemyState.Trace, new EnemyTraceState(this, EnemyStrategyHandler.Instance.PickTraceStrategy()));
        _enemyStateDict.Add(EEnemyState.Attack, new EnemyAttackState(this, EnemyStrategyHandler.Instance.EnemyAttackStrategyDict[_enemyData.EnemyType]));
        _enemyStateDict.Add(EEnemyState.Damaged, new EnemyDamagedState(this));
        _enemyStateDict.Add(EEnemyState.Die, new EnemyDieState(this));

        _enemyStateContext.ChangeState(_enemyStateDict[EEnemyState.Trace]);
    }

    private void Update()
    {
        _enemyStateContext.CurrentState.Update();
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