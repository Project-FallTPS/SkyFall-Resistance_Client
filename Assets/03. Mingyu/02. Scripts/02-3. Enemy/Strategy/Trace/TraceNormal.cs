using UnityEngine;

public class TraceNormal : ITraceStrategy
{
    public void Trace(EnemyController self)
    {
        Transform enemyTransform = self.transform;
        Transform playerTransform = self.Player.transform;

        Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;

        if (direction != Vector3.zero)
        {
            enemyTransform.rotation = Quaternion.LookRotation(direction);
        }

        enemyTransform.position += direction * self.EnemyData.MoveSpeed * Time.deltaTime;
    }
}
