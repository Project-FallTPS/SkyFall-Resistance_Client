using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossLaser", story: "레이저 공격(페이즈3)", category: "Action", id: "e38d239bc756bf42f463044556b5c9fd")]
public partial class BossLaserAction : Action
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

