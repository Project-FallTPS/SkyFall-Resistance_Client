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
            ShootBezier(obstacleHit);
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
        // 디버그 라인
        Debug.DrawLine(bossPosition, playerPosition, Color.red, 2f);
        
        DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletStraight,
            _bossController.ShootPositionTransform.position,
            Quaternion.LookRotation(directionToPlayer)
        );

    }
    
    private void ShootBezier(RaycastHit obstacleHit)
    {
        Vector3 shootStart = _bossController.ShootPositionTransform.position;
        Vector3 shootEnd = _playerTransform.position;

        Vector3 controlPoint = CalculateControlPoint(obstacleHit, shootStart, shootEnd);

        // 시각화
        const int segmentCount = 20;
        Vector3 previousPoint = shootStart;
        for (int i = 1; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 point = CalculateQuadraticBezierPoint(t, shootStart, controlPoint, shootEnd);
            Debug.DrawLine(previousPoint, point, Color.green, 2f);
            previousPoint = point;
        }

        GameObject bullet = DamageablePoolManager.Instance.GetObject(
            EDamageableType.BossBulletBezier,
            shootStart,
            Quaternion.identity
        );

        if (bullet.TryGetComponent(out BezierBullet bezierBullet))
        {
            bezierBullet.InitializePoints(shootStart, controlPoint, shootEnd);
        }
    }

    private Vector3 CalculateControlPoint(RaycastHit obstacleHit, Vector3 shootStart, Vector3 shootEnd)
    {
        Bounds obstacleBounds = obstacleHit.collider.bounds;
        Vector3 obstacleCenter = obstacleBounds.center;
        Transform obstacleTransform = obstacleHit.collider.transform;

        float verticalMarginHigh = UnityEngine.Random.Range(obstacleBounds.size.y * 0.8f, obstacleBounds.size.y * 1.5f);
        float verticalMarginLow = UnityEngine.Random.Range(obstacleBounds.size.y * 0.1f, obstacleBounds.size.y * 0.3f);  // 낮은 높이
        float horizontalMarginX = UnityEngine.Random.Range(obstacleBounds.size.x * 0.8f, obstacleBounds.size.x * 1.5f);

        Vector3 rightDir = obstacleTransform.right;

        float choice = UnityEngine.Random.value;

        if (choice < 0.33f)
        {
            // 위로 꺾기 (높은 y값)
            Vector3 midPoint = (shootStart + shootEnd) * 0.5f;
            return new Vector3(midPoint.x, obstacleBounds.max.y + verticalMarginHigh, midPoint.z);
        }
        else if (choice < 0.66f)
        {
            // 오른쪽으로 꺾기 (낮은 y값)
            return obstacleCenter + rightDir * horizontalMarginX + Vector3.up * verticalMarginLow;
        }
        else
        {
            // 왼쪽으로 꺾기 (낮은 y값)
            return obstacleCenter - rightDir * horizontalMarginX + Vector3.up * verticalMarginLow;
        }
    }
    
    private float GetOffsetDistance(Vector3 dir, Vector3 extents)
    {
        dir = dir.normalized;

        float x = Mathf.Abs(Vector3.Dot(dir, Vector3.right)) * extents.x;
        float y = Mathf.Abs(Vector3.Dot(dir, Vector3.up)) * extents.y;
        float z = Mathf.Abs(Vector3.Dot(dir, Vector3.forward)) * extents.z;

        return Mathf.Max(x, y, z);
    }
    
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0
               + 2 * (1 - t) * t * p1
               + Mathf.Pow(t, 2) * p2;
    }
}
