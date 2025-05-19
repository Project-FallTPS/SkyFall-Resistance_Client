using UnityEngine;

public interface IAttackStrategy
{
    public void Attack(Vector3 position, EnemyController self);
}
