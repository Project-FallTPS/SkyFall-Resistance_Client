using System;
using System.Collections;
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
        Debug.Log("총알 발사(페이즈1)");
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
            ShootBezier(obstacleHit);
        }
        else
        {
            Shoot();
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

    private void Shoot()
    {
        Vector3 bossPosition = _bossTransform.position;
        Vector3 playerPosition = _playerTransform.position;
        Vector3 directionToPlayer = (playerPosition - bossPosition).normalized;
        // 디버그 라인
        Debug.DrawLine(bossPosition, playerPosition, Color.red, 2f);
        
        DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletStraight,
            _bossController.ShootPositionTransform.position,
            Quaternion.LookRotation(directionToPlayer)
        );

        Debug.Log("직선 발사체 발사");
    }

    private void ShootBezier(RaycastHit obstacleHit)
    {
        Vector3 shootStart = _bossController.ShootPositionTransform.position;
        Vector3 shootEnd = _playerTransform.position;

        Bounds obstacleBounds = obstacleHit.collider.bounds;
        Vector3 controlPoint = obstacleBounds.center + Vector3.up * (obstacleBounds.extents.y + 1.5f); 

        
        const int segmentCount = 20;
        Vector3 previousPoint = shootStart;
        for (int i = 1; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 point = CalculateQuadraticBezierPoint(t, shootStart, controlPoint, shootEnd);
            Debug.DrawLine(previousPoint, point, Color.green, 2f);
            previousPoint = point;
        }

        // 발사체 생성
        GameObject bullet = DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletBezier,
            shootStart,
            Quaternion.identity
        );

        if (bullet.TryGetComponent(out BezierBullet bezierBullet))
        {
            bezierBullet.InitializePoints(shootStart, controlPoint, shootEnd);
        }
        Debug.Log("곡사 발사체 발사 (장애물 회피)");
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0
               + 2 * (1 - t) * t * p1
               + Mathf.Pow(t, 2) * p2;
    }
}
