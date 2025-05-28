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
            return Status.Running;
        }

        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        _bossData.LastAttackTime = Time.time;
        Attack();
        return Status.Success;
    }

    protected override void OnEnd() { }

    public bool CanAttack()
    {
        return true;
    }

    public void Attack()
    {
        if (IsPlayerObscured(out RaycastHit obstacleHit))
        {
            Debug.Log("곡사");
            ShootHemiteCurveBullet(obstacleHit);
        }
        else
        {
            Debug.Log("직사");
            ShootStraightBullet();
        }
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
        
        DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletStraight,
            _bossController.ShootPositionTransform.position,
            Quaternion.LookRotation(directionToPlayer)
        );

    }
    
    private void ShootHemiteCurveBullet(RaycastHit obstacleHit)
    {
        Vector3 shootStart = _bossController.ShootPositionTransform.position;
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
    }
    
    private Vector3 GetRandomMidPoint(RaycastHit obstacleHit)
    {
        Bounds bound = obstacleHit.collider.bounds;
        Vector3 center = bound.center;
        Vector3 extents = bound.extents;
        float safetyMargin = 5f;

        Vector3[] candidates = new Vector3[]
        {
            center + new Vector3(extents.x + safetyMargin, 0, 0),     // Right
            center + new Vector3(-(extents.x + safetyMargin), 0, 0),  // Left
            center + new Vector3(0, extents.y + safetyMargin, 0),     // Up
        };

        int index = UnityEngine.Random.Range(0, candidates.Length);
        Vector3 midPoint = candidates[index];
        Debug.DrawRay(midPoint, Vector3.up * 0.5f, Color.yellow, 2f);
        Debug.DrawRay(midPoint, Vector3.right * 0.5f, Color.yellow, 2f);
        Debug.DrawRay(midPoint, Vector3.forward * 0.5f, Color.yellow, 2f);
        return midPoint;
    }
}
