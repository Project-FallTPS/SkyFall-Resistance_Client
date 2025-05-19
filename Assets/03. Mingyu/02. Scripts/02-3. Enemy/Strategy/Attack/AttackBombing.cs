using UnityEngine;

public class AttackBombing : IAttackStrategy
{
    public void Attack(Vector3 position, EnemyController self)
    {
        DamageablePoolManager.Instance.GetObject(EDamageableType.EnemyBomb, position);
        self.EnemyStateContext.ChangeState(self.EnemyStateDict[EEnemyState.Die]);
    }
}
