using UnityEngine;

public class EnemyDamagedState : IEnemyState
{
    private EnemyController _enemyController;
    private AnimatorStateInfo _animatorStateInfo;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    public EnemyDamagedState(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _skinnedMeshRenderer = enemyController.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void Enter()
    {
        PlayRandomHitAnimation();
        EnemyMaterialHandler.Instance.SetEnemyMaterialColor(_skinnedMeshRenderer, Color.red);
    }

    public void Update()
    {
        _animatorStateInfo = _enemyController.EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        if (1.0f <= _animatorStateInfo.normalizedTime)
        {
            ChangeStateOnDistanceFromPlayer();
        }
    }

    public void Exit()
    {
        EnemyMaterialHandler.Instance.SetEnemyMaterialColor(_skinnedMeshRenderer, Color.white);
    }

    private void PlayRandomHitAnimation()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            _enemyController.EnemyAnimator.SetTrigger(nameof(EEnemyAnimationTransitionParam.HitTriggerLeft));
        }
        else
        {
            _enemyController.EnemyAnimator.SetTrigger(nameof(EEnemyAnimationTransitionParam.HitTriggerRight));
        }
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