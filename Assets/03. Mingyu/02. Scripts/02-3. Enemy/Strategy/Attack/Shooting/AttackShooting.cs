using UnityEngine;

public class AttackShooting : IAttackStrategy, ITransitionStrategy
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
}
