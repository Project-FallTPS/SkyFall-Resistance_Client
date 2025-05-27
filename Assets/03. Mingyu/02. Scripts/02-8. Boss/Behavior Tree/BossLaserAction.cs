using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossLaser", story: "레이저 공격(페이즈3)", category: "Action", id: "e38d239bc756bf42f463044556b5c9fd")]
public partial class BossLaserAction : Action, IBossAttack
{
    [SerializeReference]
    public BlackboardVariable<GameObject> _boss;
    private BossController _bossController;
    private BossData _bossData;
    
    private Transform _bossTransform;
    private Transform _playerTransform;

    private Vector3 _directionToPlayer;
    private Vector3 _laserEndPosition;
    private LayerMask _hitMask;
    private bool _isLaserDisappeared;
    
    protected override Status OnStart()
    {
        if (ReferenceEquals(_bossController, null) || ReferenceEquals(_bossData, null))
        {
            _bossController = _boss.Value.GetComponent<BossController>();
            _bossData = _bossController.BossData;
            _bossTransform = _bossController.transform;
            _playerTransform = _bossController.PlayerTransform;
            _hitMask = LayerMask.GetMask(nameof(ELayers.Player), nameof(ELayers.Boss));
        }
        
        if (CanAttack())
        {
            _isLaserDisappeared = false;
            Attack();
            return Status.Running;
        }
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (CheckLaserComplete())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    public bool CanAttack()
    {
        float distanceToPlayer = Vector3.Distance(_bossTransform.position, _playerTransform.position);
        return _bossData.CurrentPhase == 3 && _bossData.MaxRushDistance < distanceToPlayer;
    }

    public void Attack()
    {
        if (_bossController.NavMeshAgent.hasPath)
        {
            _bossController.NavMeshAgent.ResetPath();
        }
        _bossController.StartCoroutine(WindupCoroutine());
    }

    private bool CheckLaserComplete()
    {
        if (_isLaserDisappeared)
        {
            _bossData.LastAttackTime = Time.time;
            _bossController.StopAllCoroutines();
            return true;
        }
        return false;
    }

    private IEnumerator WindupCoroutine()
    {
        float timer = 0f;
        while (timer < _bossData.WindupTimeForLaser)
        {
            Windup();
            timer += Time.deltaTime;
            yield return null;
        }
        Laser();
    }
    
    private void Windup()
    {
        RefreshLaserVector();
        LookPlayer();
        // TODO : 기모으는 VFX, 애니메이션
    }
    
    private void RefreshLaserVector()
    {
        _directionToPlayer = (_playerTransform.position - _bossTransform.position).normalized;
        _laserEndPosition = _bossTransform.position + _directionToPlayer * _bossData.LaserRange;
    }
    
    private void LookPlayer()
    {
        Vector3 lookTarget = _playerTransform.position;
        lookTarget.y = _bossTransform.position.y;
        _bossTransform.LookAt(lookTarget);
    }
    
    private void Laser()
    {
        if (Physics.Raycast(_bossTransform.position, _directionToPlayer, 
                out RaycastHit hitInfo, _bossData.LaserRange, _hitMask))
        {
            _laserEndPosition = hitInfo.point;
        }

        GameObject laserObject = ActivateLaser();
        _bossController.StartCoroutine(LaserLifeCycle(laserObject));
    }

    private GameObject ActivateLaser()
    {
        LineRenderer lineRenderer = 
            DamageablePoolManager.Instance.GetObject(EDamageableType.BossLaser, _bossTransform.position)
                .GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, _bossTransform.position);
        lineRenderer.SetPosition(1, _laserEndPosition);
        return lineRenderer.gameObject;
    }
    private IEnumerator LaserLifeCycle(GameObject laserObject)
    {
        float timer = 0f;
        while (timer < _bossData.LaserDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        DamageablePoolManager.Instance.ReturnObject(laserObject, EDamageableType.BossLaser);
        _isLaserDisappeared = true;
    }
}

