using UnityEngine;

public class AttackShooting : IAttackStrategy
{
    public void Attack(Vector3 position, EnemyController self)
    {
        DamageablePoolManager.Instance.GetObject(EDamageableType.EnemyBullet, position);
    }
}
