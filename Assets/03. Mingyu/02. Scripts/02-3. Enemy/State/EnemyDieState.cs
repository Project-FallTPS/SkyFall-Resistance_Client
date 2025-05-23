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
        Debug.Log($"Enemy Die : {_enemyController.gameObject.name}");
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.Die), true);
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
        Debug.Log("Exit DieState");
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.Die), false);
        _enemyController.EnemyCollider.enabled = true;
    }

    private void ReturnToPool()
    {
        TargetManager.Instance.RemoveEnemyFromHashSet(_enemyController.gameObject);
        ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Remove(_enemyController.gameObject);
        EnemyPoolManager.Instance.ReturnObject(_enemyController.gameObject, _enemyData.EnemyType);
    }
} 