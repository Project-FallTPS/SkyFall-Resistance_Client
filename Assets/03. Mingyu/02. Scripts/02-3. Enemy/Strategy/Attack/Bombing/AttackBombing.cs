using UnityEngine;

public class AttackBombing : IAttackStrategy
{
    public void Attack(EnemyController self)
    {
        ApplyBombDamage(self);
        PlayBombVFX(self.transform.position);
        self.EnemyStateContext.ChangeState(self.EnemyStateDict[EEnemyState.Die]);
    }
    private void ApplyBombDamage(EnemyController enemyController)
    {
        Collider[] hitColiiders = 
            Physics.OverlapSphere
            (enemyController.transform.position, enemyController.EnemyData.ExplosionRadius);

        foreach (Collider hitCollider in hitColiiders)
        {
            // ���� Ÿ�� ���� ������ �÷��̾�Ը� ���� �Ǵ°�?
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
