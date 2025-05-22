using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossTraceAction", story: "플레이어 추격(느린 속도로)", category: "Action", id: "8543357cb4dc1bd687cd5a805696705c")]
public partial class BossTraceAction : Action
{
    [SerializeReference]
    public BlackboardVariable<GameObject> _boss;

    private BossController _bossController;
    
    protected override Status OnStart()
    {
        if (_bossController == null)
        {
            _bossController = _boss.Value.GetComponent<BossController>();
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        TracePlayer();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    private void TracePlayer()
    {
        _bossController.NavMeshAgent.SetDestination(_bossController.PlayerTransform.position);
    }
}

