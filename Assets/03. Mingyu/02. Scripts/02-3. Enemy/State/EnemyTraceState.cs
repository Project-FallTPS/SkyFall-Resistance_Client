using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraceState : IEnemyState
{
    private EnemyController _enemyController;
    public EnemyTraceState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }
    public void Enter()
    {
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
} 