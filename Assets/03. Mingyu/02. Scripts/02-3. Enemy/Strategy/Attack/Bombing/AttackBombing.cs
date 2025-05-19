using UnityEngine;

public class AttackBombing : IAttackStrategy
{
    public void Attack(Vector3 position, EnemyController self)
    {
        ApplyBombDamage(self);
        PlayBombVFX(position);
        self.EnemyStateContext.ChangeState(self.EnemyStateDict[EEnemyState.Die]);
    }
    private void ApplyBombDamage(EnemyController enemyController)
    {
        Collider[] hitColiiders = 
            Physics.OverlapSphere
            (enemyController.transform.position, enemyController.EnemyData.ExplosionRadius);

        foreach (Collider hitCollider in hitColiiders)
        {
            // 자폭 타입 적의 폭발은 플레이어에게만 적용 되는가?
            if (hitCollider.CompareTag(nameof(ETags.Player)))
            {
                if(hitCollider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    // damageable.TakeDamage(enemyController.EnemyData.AttackDamage);
                }
            }
        }

    }
    private void PlayBombVFX(Vector3 position)
    {
        GameObject vfx = VFXPoolManager.Instance.GetObject(EVFXType.EnemySuicideBombing, position, Quaternion.identity);
        vfx.GetComponent<VFX>().PlayVFX();
    }
}
