using UnityEngine;

public class TraceNormal : ITraceStrategy
{
    private Transform enemyTransform;
    private Transform playerTransform;
    private Vector3 direction;
    
    public void Trace(EnemyController self)
    {
        enemyTransform = self.transform;
        playerTransform = self.Player.transform;
        direction = (playerTransform.position - enemyTransform.position).normalized;

        if (direction != Vector3.zero)
        {
            Vector3 nextPosition = enemyTransform.position + direction * self.EnemyData.MoveSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            self.Rigidbody.MovePosition(nextPosition);
            self.Rigidbody.MoveRotation(targetRotation);
        }
    }
}
