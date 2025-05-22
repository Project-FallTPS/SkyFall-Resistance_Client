using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Serialization;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckAttackCoolDown", story: "공격 쿨다운 체크", category: "Action", id: "d9093c8758ecda16cd0ab6ff852a29d4")]
public partial class CheckAttackCoolDownAction : Action
{
    public BlackboardVariable<GameObject> Self;
    //
    private BossData _bossData;
    
    protected override Status OnStart()
    {
        if (ReferenceEquals(Self, null))
        {
            Debug.Log("Self is Null");
        }
        Debug.Log(Self.Value);
        // bossData = _bossGO.Value.GetComponent<BossController>()?.BossData;
        // Debug.Log($"마지막 공격 시간 in OnStart : {_bossData.LastAttackTime}");
        // Debug.Log($"공격 쿨타임 in OnStart : {_bossData.AttackCooltime}");
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Debug.Log($"현재 시간 : {Time.time}");
        // if (_bossData.LastAttackTime == 0f || _bossData.LastAttackTime + _bossData.AttackCooltime <= Time.time)
        // {
        //     Debug.Log("현재 공격 가능 상태");
        //     return Status.Success;
        // }
        // else
        // {
        //     Debug.Log("쿨다운중");
        //     return Status.Failure;
        // }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

