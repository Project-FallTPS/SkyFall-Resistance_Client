using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedState : IEnemyState
{
    private EnemyController _enemyController;
    private AnimatorStateInfo _animatorStateInfo;

    public EnemyDamagedState(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    public void Enter()
    {
        _enemyController.EnemyAnimator.SetTrigger(nameof(EEnemyAnimationTransitionParam.Hit));
    }

    public void Update()
    {
        _animatorStateInfo = _enemyController.EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (_animatorStateInfo.IsName(nameof(EEnemyAnimationTransitionParam.Hit)) && 1.0f <= _animatorStateInfo.normalizedTime)
        {
            ChangeStateOnDistanceFromPlayer();
        }
    }

    public void Exit()
    {
    }

    private void ChangeStateOnDistanceFromPlayer()
    {
        float distance = Vector3.Distance(_enemyController.transform.position, _enemyController.Player.transform.position);
        if (distance <= _enemyController.EnemyData.AttackableRange)
        {
            _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Attack]);
        }
        else
        {
            _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Trace]);
        }
    }
} 