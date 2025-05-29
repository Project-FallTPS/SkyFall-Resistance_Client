using UnityEngine;

public class EnemyTraceState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;
    private ITraceStrategy _traceStrategy;
    private ITransitionStrategy _transitionStrategy;
    public EnemyTraceState(EnemyController enemyController, ITraceStrategy traceStrategy,
        ITransitionStrategy transitionStrategy)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
        _traceStrategy = traceStrategy;
        _transitionStrategy  = transitionStrategy;
    }
    public void Enter()
    {
    }

    public void Update()
    {
        if (_transitionStrategy.CanChangeToAttackState(_enemyController))
        {
            _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Attack]);
        }
        _traceStrategy.Trace(_enemyController);
    }

    public void Exit()
    {
    }
} 