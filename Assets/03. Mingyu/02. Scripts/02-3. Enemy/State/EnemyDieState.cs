using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;
    private IEnumerator _dieCoroutine;

    public EnemyDieState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
    }

    public void Enter()
    {
        _dieCoroutine = DieCoroutine();
        _enemyController.StartCoroutineInEnemyState(_dieCoroutine);
        _enemyController.GetComponent<CharacterController>().enabled = false;
    }

    public void Update()
    {
    }

    public void Exit()
    {
        if (!ReferenceEquals(_dieCoroutine, null))
        {
            _enemyController.StopCoroutineInEnemyState(_dieCoroutine);
            _dieCoroutine = null;
        }
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        EnemyPoolManager.Instance.ReturnObject(_enemyController.gameObject, _enemyData.EnemyType);
        ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Remove(_enemyController.gameObject);
    }
} 