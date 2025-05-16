using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemyController;
    private IEnumerator _attackCoroutine;

    public EnemyAttackState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    public void Enter()
    {
        _attackCoroutine = AttackCoroutine();
        _enemyController.StartCoroutineInEnemyState(_attackCoroutine);
    }

    public void Update()
    {
    }

    public void Exit()
    {
        if (!ReferenceEquals(_attackCoroutine, null))
        {
            _enemyController.StopCoroutineInEnemyState(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {

        }
    }
} 