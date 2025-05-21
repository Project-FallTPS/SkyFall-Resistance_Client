using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossRush", story: "돌진 공격(페이즈2)", category: "Action", id: "e22fa8e1d643ec7c27a970890fa80d0b")]
public partial class BossRushAction : Action
{
    public BlackboardVariable<Transform> _playerTransform;
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

