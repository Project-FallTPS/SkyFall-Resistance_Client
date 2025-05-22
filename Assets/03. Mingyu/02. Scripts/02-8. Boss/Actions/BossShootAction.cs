using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossShoot", story: "사격 공격(페이즈1)", category: "Action", id: "ca6df048f0a2e61bd685e0c865fc7e9a")]
public partial class BossShootAction : Action
{
    public BlackboardVariable<GameObject> _bossGO;
    private BossData _bossData;
    // 데이터 클래스를 따로 파고, BlackboardVariable로 받아오기
    // public BlackboardVariable<BossController> _raycastHit;
    // enemyController처럼 MonoBehaviour 상속해서 보스에 붙어있는 컴포넌트를 참조하는게 나을듯
    // Ray를 쐈을 때 장애물이 감지되면 곡사, 아니면 직사
    protected override Status OnStart()
    {
        _bossData = _bossGO.Value.GetComponent<BossData>();
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

