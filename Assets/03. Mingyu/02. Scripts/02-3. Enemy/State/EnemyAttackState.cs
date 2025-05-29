using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemyController;
    private EnemyData _enemyData;
    private IAttackStrategy _attackStrategy;
    private ITransitionStrategy _transitionStrategy;
    private float _nextAttackableTime;

    public EnemyAttackState(EnemyController enemyController, IAttackStrategy attackStrategy,
        ITransitionStrategy transitionStrategy)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.EnemyData;
        _attackStrategy = attackStrategy;
        _transitionStrategy = transitionStrategy;
    }
    
    public void Enter()
    {
        _enemyController.StartCoroutineInEnemyState(AttackCoroutine());
    }

    public void Update()
    {
        LookAtPlayer();
    }

    public void Exit()
    {
        _enemyController.StopAllCoroutines();
    }
    
    private IEnumerator AttackCoroutine()
    {
        while (_transitionStrategy.CanChangeToTraceState(_enemyController))
        {
            yield return new WaitUntil(() => _nextAttackableTime <= Time.time);
            _enemyController.EnemyAnimator.SetTrigger(nameof(EEnemyAnimationTransitionParam.attack));
            _attackStrategy.Attack(_enemyController);
            _nextAttackableTime = Time.time + _enemyData.AttackDelay;
        }
        _enemyController.EnemyStateContext.ChangeState(_enemyController.EnemyStateDict[EEnemyState.Trace]);
    }
    
    private void LookAtPlayer()
    {
        Vector3 direction = 
            (_enemyController.Player.transform.position - _enemyController.transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
        
            _enemyController.transform.rotation = Quaternion.Slerp(
                _enemyController.transform.rotation,
                targetRotation,
                5f * Time.deltaTime);
        }
    }
} 