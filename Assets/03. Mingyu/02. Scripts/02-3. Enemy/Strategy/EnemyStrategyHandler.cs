using System.Collections.Generic;
using UnityEngine;

public class EnemyStrategyHandler : Singleton<EnemyStrategyHandler>
{
    [Header("Trace Strategy")]
    private HashSet<ITraceStrategy> _enemyTraceStrategySet = new HashSet<ITraceStrategy>();
    public HashSet<ITraceStrategy> EnemyTraceStrategySet => _enemyTraceStrategySet;

    [Header("Attack Strategy")]
    private Dictionary<EEnemyType, IAttackStrategy> _enemyAttackStrategyDict = new Dictionary<EEnemyType, IAttackStrategy>();
    public Dictionary<EEnemyType, IAttackStrategy> EnemyAttackStrategyDict => _enemyAttackStrategyDict;

    protected override void Awake()
    {
        base.Awake();
        _enemyTraceStrategySet.Add(new TraceNormal());
        _enemyTraceStrategySet.Add(new TraceBezier());
        _enemyAttackStrategyDict.Add(EEnemyType.Shooting, new AttackShooting());
        _enemyAttackStrategyDict.Add(EEnemyType.Bombing, new AttackBombing());
    }

    public ITraceStrategy PickTraceStrategy()
    {
        int rand = Random.Range(0, 100);
        foreach (var strategy in _enemyTraceStrategySet)
        {
            if (rand < 10 && strategy is TraceNormal)
            {
                return new TraceNormal();

            }
            else if (10 <= rand && strategy is TraceBezier)
            {
                return new TraceBezier();

            }
        }
        return null;
    }
}
