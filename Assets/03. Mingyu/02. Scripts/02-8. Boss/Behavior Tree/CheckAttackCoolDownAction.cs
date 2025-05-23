using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckAttackCoolDown", story: "공격 쿨다운 체크", category: "Action", id: "d9093c8758ecda16cd0ab6ff852a29d4")]
public partial class CheckAttackCoolDownAction : Action
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
        if (_bossData.LastAttackTime == 0f || _bossData.LastAttackTime + _bossData.AttackCooltime <= Time.time)
        {
            return Status.Success;
        }
        else
        {
            Debug.Log("쿨다운중");
            return Status.Failure;
        }
    }

    protected override void OnEnd()
    {
    }
}

