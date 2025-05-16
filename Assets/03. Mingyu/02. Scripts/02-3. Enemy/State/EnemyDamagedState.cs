using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedState : IEnemyState
{
    private EnemyController _enemyController;
    private Vector3 _knockbackDirection;

    public EnemyDamagedState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    public void Enter()
    {
        _knockbackDirection = (_enemyController.transform.position - _enemyController.Player.transform.position).normalized;
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
} 