using UnityEngine;

public class EnemyTraceState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;
    private ITraceStrategy _traceStrategy;
    public EnemyTraceState(EnemyController enemyController, ITraceStrategy traceStrategy)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
        _traceStrategy = traceStrategy;
    }
    public void Enter()
    {
    }

    public void Update()
    {
        if (Vector3.Distance(_enemyController.transform.position, _enemyController.Player.transform.position)
           <= _enemyData.AttackableRange)
        {
            _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Attack]);
        }
        _traceStrategy.Trace(_enemyController);
    }

    public void Exit()
    {
    }
} 