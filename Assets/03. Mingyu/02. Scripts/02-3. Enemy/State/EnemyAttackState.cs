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
    private float _nextAttackTime;

    public EnemyAttackState(EnemyController enemyController, IAttackStrategy attackStrategy)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
        _attackStrategy = attackStrategy;
    }

    public void Enter()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.Idle), true);
    }

    public void Update()
    {
        if (_enemyData.AttackableRange <
            Vector3.Distance(_enemyController.transform.position, _enemyController.Player.transform.position))
        {
            _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Trace]);
        }
        TryAttack();
    }

    public void Exit()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.Idle), false);

    }
    private void TryAttack()
    {
        if (Time.time < _nextAttackTime)
        {
            return;
        }
        _enemyController.EnemyAnimator.SetTrigger(nameof(EEnemyAnimationTransitionParam.Attack));
        _attackStrategy.Attack(_enemyController.transform.position, _enemyController);
        _nextAttackTime = Time.time + _enemyData.AttackDelay;
    }
} 