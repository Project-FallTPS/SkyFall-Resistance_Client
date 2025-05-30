using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AttackSupporting : IAttackStrategy, ITransitionStrategy
{
    public void Attack(EnemyController self)
    {
        ApplyShield(self);
    }

    public bool CanChangeToAttackState(EnemyController self)
    {
        return self.EnemyData.NextAttackableTime <= Time.time;
    }

    public bool CanChangeToTraceState(EnemyController self)
    {
        return true;
    }
    
    private void ApplyShield(EnemyController self)
    {
        Collider[] hitColiiders = 
            Physics.OverlapSphere
                (self.transform.position, self.EnemyData.ShieldBuffRadius);
        
        foreach (Collider hitCollider in hitColiiders)
        {
            if (hitCollider.CompareTag(nameof(ETags.Enemy)) &&
                hitCollider.TryGetComponent<EnemyController>(out EnemyController enemyController))
            {
                enemyController.ActivateShield();
                MakeShieldTrail(self, enemyController);
            }
        }
    }

    private void MakeShieldTrail(EnemyController from, EnemyController to)
    {
        Vector3 start = from.transform.position;

        TrailMovement trailMovement =
            VFXPoolManager.Instance
                .GetObject(EVFXType.EnemySupportTypeShieldTrail, start, quaternion.identity)
                .GetComponent<TrailMovement>();
        trailMovement.SetStartPositionAndTarget(from.transform.position, to.transform);
    }
    
}
