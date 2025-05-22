using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossShoot", story: "사격 공격(페이즈1)", category: "Action", id: "ca6df048f0a2e61bd685e0c865fc7e9a")]
public partial class BossShootAction : Action
{
    [SerializeReference]
    public BlackboardVariable<GameObject> _boss;
    private BossData _bossData;

    protected override Status OnStart()
    {
        _bossData = _boss.Value.GetComponent<BossController>().BossData;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        _bossData.LastAttackTime = Time.time;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

