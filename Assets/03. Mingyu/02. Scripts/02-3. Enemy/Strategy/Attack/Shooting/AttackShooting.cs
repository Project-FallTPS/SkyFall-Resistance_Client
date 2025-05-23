using UnityEngine;

public class AttackShooting : IAttackStrategy
{
    public void Attack(Vector3 position, EnemyController self)
    {
        GameObject bullet = DamageablePoolManager.Instance.GetObject(EDamageableType.EnemyBullet, position, self.transform.rotation);
        bullet.GetComponent<StraightBullet>().Damage = self.EnemyData.AttackDamage;
    }
}
