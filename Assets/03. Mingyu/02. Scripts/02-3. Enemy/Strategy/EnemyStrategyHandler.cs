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

    public ITraceStrategy PickTraceStrategy()
    {
        int rand = Random.Range(0, 100); // 0부터 99까지
        foreach (var strategy in _enemyTraceStrategySet)
        {
            if (rand < 80 && strategy is TraceNormal)
            {
                return strategy;

            }
            else if (rand >= 80 && strategy is TraceBazier)
            {
                return strategy;

            }
        }
        return null;
    }
}
