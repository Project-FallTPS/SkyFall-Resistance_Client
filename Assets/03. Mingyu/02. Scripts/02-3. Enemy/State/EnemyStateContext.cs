using UnityEngine;

public class EnemyStateContext
{
    private IEnemyState _currentState;
    public IEnemyState CurrentState { get => _currentState; set => _currentState = value; }
    private EnemyController _enemyController;

    public EnemyStateContext(EnemyController controller)
    {
        _enemyController = controller;
    }

    public void ChangeState()
    {
        _currentState = new EnemyTraceState(_enemyController);
        _currentState.Enter();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (!ReferenceEquals(_currentState, null))
        {
            _currentState.Exit();
        }
        _currentState = newState;
        _currentState.Enter();
    }
} 