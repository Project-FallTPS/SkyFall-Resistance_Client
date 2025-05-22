using System;
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
    
    protected override Status OnStart()
    {
        if (ReferenceEquals(_bossController, null) || ReferenceEquals(_bossData, null))
        {
            _bossController = _boss.Value.GetComponent<BossController>();
            _bossData = _bossController.BossData;
            _bossTransform = _bossController.transform;
            _playerTransform = _bossController.PlayerTransform;
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
        return Status.Success;
    }

    protected override void OnEnd()
    {

    }

    public bool CanAttack()
    {
        float distanceToPlayer = Vector3.Distance(_bossTransform.position, _playerTransform.position);
        return !(_bossData.CurrentPhase < 2) && !(distanceToPlayer < _bossData.MinRushDistance);
    }

    public bool Attack()
    {
        throw new NotImplementedException();
    }
}

