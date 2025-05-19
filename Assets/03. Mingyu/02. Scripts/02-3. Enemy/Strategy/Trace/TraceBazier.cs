using UnityEngine;

public class TraceBazier : ITraceStrategy
{
    private Vector3 controlPoint1;
    private Vector3 controlPoint2;
    private Vector3 previousEnemyPosition = Vector3.zero;

    private float smoothSpeed = 5f;

    public void Trace(EnemyController self)
    {
        Vector3 enemyPosition = self.transform.position;
        Vector3 playerPosition = self.Player.transform.position;

        // 첫 프레임 초기화
        if (previousEnemyPosition == Vector3.zero)
        {
            previousEnemyPosition = enemyPosition;

            controlPoint1 = enemyPosition + (playerPosition - enemyPosition) * 0.3f + Vector3.up * 2f;
            controlPoint2 = enemyPosition + (playerPosition - enemyPosition) * 0.7f + Vector3.up * 2f;
        }

        // 제어점 부드럽게 보간
        Vector3 targetControlPoint1 = enemyPosition + (playerPosition - enemyPosition) * 0.3f + Vector3.up * 2f;
        Vector3 targetControlPoint2 = enemyPosition + (playerPosition - enemyPosition) * 0.7f + Vector3.up * 2f;

        controlPoint1 = Vector3.Lerp(controlPoint1, targetControlPoint1, Time.deltaTime * smoothSpeed);
        controlPoint2 = Vector3.Lerp(controlPoint2, targetControlPoint2, Time.deltaTime * smoothSpeed);

        // t 값 (0~1)
        float t = Time.deltaTime * smoothSpeed;

        // 베지어 곡선 계산
        Vector3 nextPos = CalculateBezierPoint(t, enemyPosition, controlPoint1, controlPoint2, playerPosition);

        self.transform.position = nextPos;

        UpdateRotation(self, playerPosition);

        previousEnemyPosition = enemyPosition;
    }

    private void UpdateRotation(EnemyController self, Vector3 targetPosition)
    {
        Vector3 lookDir = targetPosition - self.transform.position;
        if (lookDir != Vector3.zero)
        {
            self.transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float ttt = tt * t;
        float uuu = uu * u;

        Vector3 point = uuu * p0;
        point += 3 * uu * t * p1;
        point += 3 * u * tt * p2;
        point += ttt * p3;
        return point;
    }
}
