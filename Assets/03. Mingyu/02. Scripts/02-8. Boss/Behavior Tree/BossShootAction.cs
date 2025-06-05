using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossShoot", story: "사격 공격(페이즈1)", category: "Action", id: "ca6df048f0a2e61bd685e0c865fc7e9a")]
public partial class BossShootAction : Action, IBossAttack
{
    [SerializeReference]
    public BlackboardVariable<GameObject> _boss;

    private BossController _bossController;
    private BossData _bossData;
    private Transform _bossTransform;
    private Transform _playerTransform;
    private LayerMask _obstacleMask;

    private AnimatorStateInfo stateInfo;

    protected override Status OnStart()
    {
        if (_bossController == null || _bossData == null)
        {
            _bossController = _boss.Value.GetComponent<BossController>();
            _bossData = _bossController.BossData;
            _bossTransform = _bossController.transform;
            _playerTransform = _bossController.PlayerTransform;
            _obstacleMask = LayerMask.GetMask(nameof(ELayers.Obstacle));
        }
        
        if (CanAttack())
        {
            Attack();
            return Status.Running;
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (CheckShootAnimationComplete())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        _bossController.NavMeshAgent.isStopped = false;
    }

    public bool CanAttack()
    {
        return true;
    }

    public void Attack()
    {
        if (IsPlayerObscured(out RaycastHit obstacleHit))
        {
            ShootHemiteCurveBullet(obstacleHit);
        }
        else
        {
            ShootStraightBullet();
        }
        _bossData.LastAttackTime = Time.time;
        _bossController.NavMeshAgent.isStopped = true;
        _bossController.Animator.SetTrigger(nameof(EBossAnimationParam.AttackShootTrigger));
        stateInfo = _bossController.Animator.GetCurrentAnimatorStateInfo(0);
    }

    private bool CheckShootAnimationComplete()
    {
        return _bossData.LastAttackTime + stateInfo.normalizedTime / 2 <= Time.time;
    }
    
    private bool IsPlayerObscured(out RaycastHit obstacleHit)
    {
        Vector3 bossPosition = _bossTransform.position;
        Vector3 playerPosition = _playerTransform.position;
        Vector3 directionToPlayer = (playerPosition - bossPosition).normalized;
        float distanceToPlayer = Vector3.Distance(bossPosition, playerPosition);
        
        if (Physics.Raycast(bossPosition, directionToPlayer, out RaycastHit hit, distanceToPlayer, _obstacleMask))
        {
            obstacleHit = hit;
            return true;
        }

        obstacleHit = default;
        return false;
    }

    private void ShootStraightBullet()
    {
        Vector3 bossPosition = _bossTransform.position;
        Vector3 playerPosition = _playerTransform.position;
        Vector3 directionToPlayer = (playerPosition - bossPosition).normalized;
        
        Debug.DrawLine(bossPosition, playerPosition, Color.red, 2f);

        foreach (Transform shootTransform in _bossController.BulletShootPositions)
        {
            DamageablePoolManager.Instance.GetObject(
                EDamageableType.BossBulletStraight,
                shootTransform.position,
                Quaternion.LookRotation(directionToPlayer)
            );
            VFXPoolManager.Instance.GetObject(
                EVFXType.BossShootMuzzle,
                shootTransform.position,
                Quaternion.LookRotation(directionToPlayer)
            );
        }
    }
    
    private void ShootHemiteCurveBullet(RaycastHit obstacleHit)
    {
        foreach (Transform shootTransform in _bossController.BulletShootPositions)
        {
            Vector3 shootStart = shootTransform.position;
            Vector3 shootEnd = _playerTransform.position;
            Vector3 shootMid = GetRandomMidPoint(obstacleHit);
        
            GameObject bullet = DamageablePoolManager.Instance.GetObject(
                EDamageableType.BossBulletCurve,
                shootStart,
                Quaternion.identity
            );
            if (bullet.TryGetComponent(out CurveBullet hermiteBullet))
            {
                hermiteBullet.InitializePoints(shootStart, shootMid, shootEnd);
            }
            VFXPoolManager.Instance.GetObject(
                EVFXType.BossShootMuzzle,
                shootTransform.position,
                Quaternion.LookRotation((shootStart - shootEnd).normalized)
            );
        }
    }
    
    private Vector3 GetRandomMidPoint(RaycastHit obstacleHit)
    {
        Bounds bound = obstacleHit.collider.bounds;
        Vector3 center = bound.center;
        Vector3 extents = bound.extents;
        float safetyMargin = _bossController.BossData.SafetyMargin;

        Vector3[] candidates = new Vector3[]
        {
            center + new Vector3(extents.x + safetyMargin, 0, 0),     // Right
            center + new Vector3(-(extents.x + safetyMargin), 0, 0),  // Left
            center + new Vector3(0, extents.y + safetyMargin, 0),     // Up
        };

        int index = UnityEngine.Random.Range(0, candidates.Length);
        Vector3 midPoint = candidates[index];
        
        return midPoint;
    }
}
