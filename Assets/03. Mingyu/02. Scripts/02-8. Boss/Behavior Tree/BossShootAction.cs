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
            ShootCurve(obstacleHit);
        }
        else
        {
            Debug.Log("직사");
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
        
        Debug.DrawLine(bossPosition, playerPosition, Color.red, 2f);
        
        DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletStraight,
            _bossController.ShootPositionTransform.position,
            Quaternion.LookRotation(directionToPlayer)
        );

    }
    
    private void ShootCurve(RaycastHit obstacleHit)
    {
        Vector3 shootStart = _bossController.ShootPositionTransform.position;
        Vector3 shootEnd = _playerTransform.position;

        // 탄젠트는 단순히 방향 * 강도로 설정 (이건 이후 조정 가능)
        Vector3 toPlayer = (shootEnd - shootStart).normalized;
        Vector3 m0 = toPlayer * 5f; // 시작 속도
        Vector3 m1 = Vector3.up * 5f; // 끝 속도 (위로 살짝 튀는 식)

        
        const int segmentCount = 20;
        Vector3 previousPoint = shootStart;
        for (int i = 1; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 point = CalculateHermitePoint(t, shootStart, m0, shootEnd, m1);
            Debug.DrawLine(previousPoint, point, Color.yellow, 2f);
            previousPoint = point;
        }

        GameObject bullet = DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletCurve,
            shootStart,
            Quaternion.identity
        );

        if (bullet.TryGetComponent(out CurveBullet curveBullet))
        {
            curveBullet.InitializeHermite(shootStart, m0, shootEnd, m1);
        }
    }

    private Vector3 CalculateHermitePoint(float t, Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return (2 * t3 - 3 * t2 + 1) * p0 +
               (t3 - 2 * t2 + t) * m0 +
               (-2 * t3 + 3 * t2) * p1 +
               (t3 - t2) * m1;
    }
}
