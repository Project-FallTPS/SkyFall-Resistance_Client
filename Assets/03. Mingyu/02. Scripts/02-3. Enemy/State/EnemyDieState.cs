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
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.die), true);
        _enemyController.EnemyCollider.enabled = false;
        _animatorStateInfo = _enemyController.EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        ReturnToPool();
    }

    public void Update()
    {
        //if (_animatorStateInfo.IsName(nameof(EEnemyAnimationTransitionParam.Die)) && 1.0f <= _animatorStateInfo.normalizedTime)
        //{
        //    ReturnToPool();
        //}
    }

    public void Exit()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.die), false);
        _enemyController.EnemyCollider.enabled = true;
    }

    private void ReturnToPool()
    {
        TargetManager.Instance.RemoveEnemyFromHashSet(_enemyController.gameObject);
        ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Remove(_enemyController.gameObject);
        EnemyPoolManager.Instance.ReturnObject(_enemyController.gameObject, _enemyData.EnemyType);
    }
} 