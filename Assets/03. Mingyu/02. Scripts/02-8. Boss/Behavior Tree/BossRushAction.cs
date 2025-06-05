using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossRush", story: "돌진 공격(페이즈2)", category: "Action", id: "e22fa8e1d643ec7c27a970890fa80d0b")]
public partial class BossRushAction : Action, IBossAttack
{
    [SerializeReference]
    public BlackboardVariable<GameObject> _boss;
    private BossController _bossController;
    private BossData _bossData;
    
    private Transform _bossTransform;
    private Transform _playerTransform;

    private Vector3 _rushDestination;
    private float _originalSpeed;

    private VFX _bossRushVFX;
    
    protected override Status OnStart()
    {
        if (ReferenceEquals(_bossController, null) || ReferenceEquals(_bossData, null))
        {
            _bossController = _boss.Value.GetComponent<BossController>();
            _bossData = _bossController.BossData;
            _bossTransform = _bossController.transform;
            _playerTransform = _bossController.PlayerTransform;
            _originalSpeed = _bossController.BossData.MoveSpeed;
            GetBossRushVFX();
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
        if (CheckRushComplete())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private void GetBossRushVFX()
    {
        VFX[] vfxes = _bossController.GetComponentsInChildren<VFX>();
        foreach (VFX vfx in vfxes)
        {
            if (vfx.VFXType == EVFXType.BossRush)
            {
                _bossRushVFX = vfx;
            }
        }
    }
    
    public bool CanAttack()
    {
        float distanceToPlayer = Vector3.Distance(_bossTransform.position, _playerTransform.position);
        return (2 <= _bossData.CurrentPhase) && (_bossData.MinRushDistance < distanceToPlayer) &&
            (distanceToPlayer <= _bossData.MaxRushDistance);
    }

    public void Attack()
    {
        _bossController.StartCoroutine(WindupCoroutine());
    }
    
    private IEnumerator WindupCoroutine()
    {
        Windup();
        // 3. Windup 지점에 도달했거나, Windup 시간이 다 되었는지 체크
        float timer = 0f;
        while (timer < _bossData.BeforeRushDelay && 
               !_bossController.NavMeshAgent.pathPending 
               && 0.1f < _bossController.NavMeshAgent.remainingDistance)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Rush();
    }

    private void Windup()
    {
        // 1. 플레이어 반대 방향 계산
        _bossController.NavMeshAgent.ResetPath();
        Vector3 directionToPlayer = (_playerTransform.position - _bossTransform.position).normalized;
        Vector3 preparationDestination = _bossTransform.position - directionToPlayer * _bossData.WindupDistance;

        // 2. 느린 속도로 돌진할 방향의 뒤로 이동
        _bossController.NavMeshAgent.speed /= _bossData.RushSpeedDivisorForWindup;
        _bossController.NavMeshAgent.SetDestination(preparationDestination);
    }    
    private void Rush()
    {
        // 4. 매우 빠른 속도로 플레이어를 향해 돌진
        _bossRushVFX.gameObject.SetActive(true);
        _bossController.NavMeshAgent.speed = _originalSpeed * _bossData.RushSpeedMultiplier;
        _rushDestination = new Vector3(_playerTransform.position.x, 0f, _playerTransform.position.z);
        _bossController.NavMeshAgent.SetDestination(_rushDestination);
    }

    private bool CheckRushComplete()
    {
        if (Vector3.Distance(_bossTransform.position, _rushDestination) <= 0.2f)
        {
            _bossData.LastAttackTime = Time.time;
            _bossController.NavMeshAgent.speed = _originalSpeed;
            _bossController.StopAllCoroutines();
            _bossRushVFX.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}

