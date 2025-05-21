using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckAttackCoolDown", story: "공격 쿨다운이 진행중인지 체크", category: "Action", id: "d9093c8758ecda16cd0ab6ff852a29d4")]
public partial class CheckAttackCoolDownAction : Action
{

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

