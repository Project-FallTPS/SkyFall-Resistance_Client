using UnityEngine;

public class AttackBombing : IAttackStrategy, ITransitionStrategy
{
    public void Attack(EnemyController self)
    {
        ApplyBombDamage(self);
        PlayBombVFX(self.transform.position);
        self.EnemyStateContext.ChangeState(self.EnemyStateDict[EEnemyState.Die]);
    }
    
    public bool CanChangeToAttackState(EnemyController self)
    {
        return Vector3.Distance(self.transform.position, self.Player.transform.position)
               <= self.EnemyData.AttackableRange;
    }

    public bool CanChangeToTraceState(EnemyController self)
    {
        return self.EnemyData.AttackableRange
               < Vector3.Distance(self.transform.position, self.Player.transform.position);
    }
    
    private void ApplyBombDamage(EnemyController enemyController)
    {
        Collider[] hitColiiders = 
            Physics.OverlapSphere
            (enemyController.transform.position, enemyController.EnemyData.ExplosionRadius);

        foreach (Collider hitCollider in hitColiiders)
        {
            if (hitCollider.CompareTag(nameof(ETags.Player)) &&
                hitCollider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(enemyController.EnemyData.AttackDamage);
            }
        }

    }
    private void PlayBombVFX(Vector3 position)
    {
        GameObject vfx = VFXPoolManager.Instance.GetObject(EVFXType.EnemySuicideBombing, position, Quaternion.identity);
        vfx.GetComponent<VFX>().PlayVFX();
    }
}
