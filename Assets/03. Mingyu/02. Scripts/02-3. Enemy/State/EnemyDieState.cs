using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;
    private Rigidbody _rigidbody;
    private CapsuleCollider _enemyCollider;

    private AnimatorStateInfo _animatorStateInfo;

    private Vector3 _deathPosition;

    public EnemyDieState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
        _rigidbody = enemyController.Rigidbody;
        _enemyCollider = enemyController.EnemyCollider;
    }

    public void Enter()
    {
        _enemyController.StartCoroutineInEnemyState(DieCoroutine());
        _deathPosition = _enemyController.transform.position;
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
    
    private IEnumerator DieCoroutine()
    {
        _enemyController.EnemyAnimator.SetBool(nameof(EEnemyAnimationTransitionParam.die), true);
        TargetManager.Instance.RemoveEnemyFromHashSet(_enemyController.gameObject);
        ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Remove(_enemyController.gameObject);
        ApplyDeathPhysics();
        yield return new WaitForSeconds(3f);
        TryDropAccessoryBox();
        ReturnToPool();
    }
    
    private void ApplyDeathPhysics()
    {
        _enemyCollider.enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        Vector3 forceDirection = (_deathPosition - _enemyController.Player.transform.position).normalized;
        Vector3 finalForce = forceDirection * Random.Range(10f, 15f);
        Debug.Log(finalForce);
        // _rigidbody.AddForce(finalForce, ForceMode.Impulse);
    }
    
    private void TryDropAccessoryBox()
    {
        int rand = Random.Range(0, 100);
        if (rand < _enemyController.EnemyData.AccessoryBoxDropProbability)
        {
            BoxPoolManager.Instance.GetObject(EBoxType.AccessoryBox, _deathPosition);
        }
    }
    private void ReturnToPool()
    {
        EnemyPoolManager.Instance.ReturnObject(_enemyController.gameObject, _enemyData.EnemyType);
    }
} 