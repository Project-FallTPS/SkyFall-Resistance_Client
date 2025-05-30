using UnityEngine;

public interface ITransitionStrategy
{
    public bool CanChangeToAttackState(EnemyController self);
    public bool CanChangeToTraceState(EnemyController self);
}
