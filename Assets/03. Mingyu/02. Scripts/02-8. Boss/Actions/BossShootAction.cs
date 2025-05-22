using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossShoot", story: "사격 공격(페이즈1)", category: "Action", id: "ca6df048f0a2e61bd685e0c865fc7e9a")]
public partial class BossShootAction : Action, IBossAttack
{
    [SerializeReference]
    public BlackboardVariable<GameObject> _boss;
    private BossController _bossController;
    private BossData _bossData;
    
    
    private LayerMask _obstacleMask;
    private Transform _bossTransform;
    private Transform _playerTransform;

    private float _distanceToPlayer;
    protected override Status OnStart()
    {
        if (ReferenceEquals(_bossController, null) || ReferenceEquals(_bossData, null))
        {
            _bossController = _boss.Value.GetComponent<BossController>();
            _bossData = _bossController.BossData;
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
    
    private void TryShootAtPlayer()
    {
        Vector3 bossPosition = _bossController.transform.position;
        Vector3 playerPosition = _playerTransform.position;
        _distanceToPlayer = Vector3.Distance(bossPosition, playerPosition);
        Vector3 directionToPlayer = (playerPosition - bossPosition).normalized;

        float distance = Vector3.Distance(bossPosition, playerPosition);
        bool isObstructed = Physics.Raycast(bossPosition, directionToPlayer, distance, _obstacleMask);

        if (!isObstructed)
        {
            FireProjectile(bossPosition, directionToPlayer);
        }
        else
        {
            Vector3 controlPoint = (bossPosition + playerPosition) * 0.5f + Vector3.up * 5f;
            FireBezierProjectile(bossPosition, controlPoint, playerPosition);
        }
    }

    private void FireProjectile(Vector3 position, Vector3 direction)
    {

    }

    private void FireBezierProjectile(Vector3 start, Vector3 control, Vector3 end)
    {

    }


    public bool CanAttack()
    {
        return true;
    }

    public bool Attack()
    {
        
    }
}

