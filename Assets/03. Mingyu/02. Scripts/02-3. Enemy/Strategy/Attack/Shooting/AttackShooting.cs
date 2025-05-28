using UnityEngine;

public class AttackShooting : IAttackStrategy
{
    public void Attack(EnemyController self)
    {
        foreach (Transform shootTransform in self.BulletShootPositions)
        {
            GameObject bullet = DamageablePoolManager.Instance.GetObject
            (self.EnemyData.DamageableType, shootTransform.position, shootTransform.rotation);
            bullet.GetComponent<StraightBullet>().Damage = self.EnemyData.AttackDamage;
        }
    }
}
