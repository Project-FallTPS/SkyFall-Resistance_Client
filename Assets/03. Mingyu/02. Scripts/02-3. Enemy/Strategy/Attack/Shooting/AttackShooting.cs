using UnityEngine;

public class AttackShooting : IAttackStrategy
{
    public void Attack(Vector3 position, EnemyController self)
    {
        GameObject bullet = DamageablePoolManager.Instance.GetObject(EDamageableType.EnemyBullet, position);
        bullet.GetComponent<Bullet>().Damage = self.EnemyData.AttackDamage;
    }
}
