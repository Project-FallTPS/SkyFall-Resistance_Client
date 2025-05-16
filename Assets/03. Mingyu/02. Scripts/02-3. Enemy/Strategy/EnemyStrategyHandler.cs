using System.Collections.Generic;
using UnityEngine;

public class EnemyStrategyHandler : MonoBehaviour
{
    [Header("Trace Strategy")]
    private HashSet<ITraceStrategy> _enemyTraceStrategySet = new HashSet<ITraceStrategy>();
    public HashSet<ITraceStrategy> EnemyTraceStrategySet => _enemyTraceStrategySet;

    [Header("Attack Strategy")]
    private Dictionary<EEnemyType, IAttackStrategy> _enemyAttackStrategyDict = new Dictionary<EEnemyType, IAttackStrategy>();
    public Dictionary<EEnemyType, IAttackStrategy> EnemyAttackStrategyDict => _enemyAttackStrategyDict;

    private void Awake()
    {
        _enemyTraceStrategySet.Add(new TraceNormal());
        _enemyTraceStrategySet.Add(new TraceBazier());


        _enemyAttackStrategyDict.Add(EEnemyType.Shooting, new AttackShooting());
        _enemyAttackStrategyDict.Add(EEnemyType.Bombing, new AttackBombing());
    }
}
