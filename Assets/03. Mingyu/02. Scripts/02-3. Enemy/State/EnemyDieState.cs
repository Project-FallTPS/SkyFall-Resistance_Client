using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;

    private AnimatorStateInfo _animatorStateInfo;

    public EnemyDieState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
    }

    public void Enter()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.Die), true);
        _enemyController.EnemyCollider.enabled = false;
    }

    public void Update()
    {
        _animatorStateInfo = _enemyController.EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (_animatorStateInfo.IsName(nameof(EEnemyAnimationTransitionParam.Die)) && 1.0f <= _animatorStateInfo.normalizedTime)
        {
            ReturnToPool();
        }
    }

    public void Exit()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.Die), false);
        _enemyController.EnemyCollider.enabled = true;
    }

    private void ReturnToPool()
    {
        EnemyPoolManager.Instance.ReturnObject(_enemyController.gameObject, _enemyData.EnemyType);
        ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Remove(_enemyController.gameObject);
    }
} 