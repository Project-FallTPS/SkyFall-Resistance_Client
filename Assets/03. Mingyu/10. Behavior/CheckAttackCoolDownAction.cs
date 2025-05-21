using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckAttackCoolDown", story: "공격 쿨다운 체크", category: "Action", id: "d9093c8758ecda16cd0ab6ff852a29d4")]
public partial class CheckAttackCoolDownAction : Action
{
    public BlackboardVariable<float> _attackCoolDownTime;
    public BlackboardVariable<float> _lastAttackTime;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_lastAttackTime == 0f || _lastAttackTime + _attackCoolDownTime <= Time.time)
        {
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }

    protected override void OnEnd()
    {
    }
}

