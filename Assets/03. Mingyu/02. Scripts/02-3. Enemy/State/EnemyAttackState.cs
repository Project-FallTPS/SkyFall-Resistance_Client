using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector.Libs;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;
    private IAttackStrategy _attackStrategy;
    private IEnumerator _attackCoroutine;

    public EnemyAttackState(EnemyController enemyController, IAttackStrategy attackStrategy)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
        _attackStrategy = attackStrategy;
    }

    public void Enter()
    {
        _attackCoroutine = AttackCoroutine();
        _enemyController.StartCoroutineInEnemyState(_attackCoroutine);
    }

    public void Update()
    {
        if (_enemyData.AttackableRange <
            Vector3.Distance(_enemyController.transform.position, _enemyController.Player.transform.position))
        {
            _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Trace]);
        }
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
            _attackStrategy.Attack(_enemyController.transform.position, _enemyController);
            yield return new WaitForSeconds(_enemyController.EnemyData.AttackDelay);
        }
    }
} 