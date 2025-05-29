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
        _enemyController.StartCoroutineInEnemyState(DieCoroutine());
    }

    public void Update()
    {
    }

    public void Exit()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.die), false);
        _enemyController.EnemyCollider.enabled = true;
        _enemyController.StopAllCoroutines();
    }

    private void TryDropAccessoryBox()
    {
        int rand = Random.Range(0, 100);
        if (rand < _enemyController.EnemyData.AccessoryBoxDropProbability)
        {
            BoxPoolManager.Instance.GetObject(EBoxType.AccessoryBox, _enemyController.transform.position);
        }
    }
    private void ReturnToPool()
    {
        EnemyPoolManager.Instance.ReturnObject(_enemyController.gameObject, _enemyData.EnemyType);
    }

    private IEnumerator DieCoroutine()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.die), true);
        _enemyController.EnemyCollider.enabled = false;
        TargetManager.Instance.RemoveEnemyFromHashSet(_enemyController.gameObject);
        ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Remove(_enemyController.gameObject);
        yield return new WaitForSeconds(1f);
        TryDropAccessoryBox();
        ReturnToPool();
    }
} 