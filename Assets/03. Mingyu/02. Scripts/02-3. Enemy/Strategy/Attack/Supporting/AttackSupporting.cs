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
                hitCollider.TryGetComponent<EnemyController>(out EnemyController enemyController) &&
                !enemyController.EnemyData.IsShieldActive)
            {
                enemyController.ActivateShield();
            }
        }
    }
}
