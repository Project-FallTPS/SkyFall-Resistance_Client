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
        return Status.Success;
    }

    protected override void OnEnd() { }

    public bool CanAttack()
    {
        return true;
    }

    public void Attack()
    {
        if (IsPlayerObscured(out Vector3 hitPoint))
        {
            FireBezierProjectile(hitPoint);
        }
        else
        {
            FireProjectile();
        }
    }

    private bool IsPlayerObscured(out Vector3 hitPoint)
    {
        Vector3 bossPosition = _bossTransform.position;
        Vector3 playerPosition = _playerTransform.position;
        Vector3 directionToPlayer = (playerPosition - bossPosition).normalized;
        float distanceToPlayer = Vector3.Distance(bossPosition, playerPosition);

        if (Physics.Raycast(bossPosition, directionToPlayer, out RaycastHit hit, distanceToPlayer, _obstacleMask))
        {
            hitPoint = hit.point;
            return true;
        }

        hitPoint = Vector3.zero;
        return false;
    }

    private void FireProjectile()
    {
        // 직선 발사체 (예: 기본 발사체)
        Vector3 start = _bossTransform.position;
        Vector3 end = _playerTransform.position;

        Debug.DrawLine(start, end, Color.red, 2f);
        Debug.Log("직선 발사체 발사");
    }

    private void FireBezierProjectile(Vector3 obstacleHitPoint)
    {
        Vector3 start = _bossTransform.position;
        Vector3 end = _playerTransform.position;

        // 장애물 피해서 위쪽으로 휘는 곡선 만들기
        Vector3 control = obstacleHitPoint + Vector3.up * 2f;

        // 디버그용 베지어 경로 시각화
        const int segmentCount = 20;
        Vector3 prevPoint = start;

        for (int i = 1; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 point = CalculateQuadraticBezierPoint(t, start, control, end);
            Debug.DrawLine(prevPoint, point, Color.yellow, 2f);
            prevPoint = point;
        }

        Debug.Log("베지어 곡선 발사체 발사 (장애물 있음)");
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}
