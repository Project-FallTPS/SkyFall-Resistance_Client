using UnityEngine;

public class TraceBezier : ITraceStrategy
{
    Vector3[] points = new Vector3[4];
    private float _t = 0;

    [SerializeField] public float posA = 0.55f;
    [SerializeField] public float posB = 0.45f;

    private Vector3 _prevPosition;
    private bool _isPathSet = false;
    private float _approxCurveLength = 1f; // 보간 속도 보정용

    public void Trace(EnemyController self)
    {
        if (!_isPathSet)
        {
            SetPath(self);
        }

        if (_t > 1f)
        {
            _t = 0f;
            points[0] = self.transform.position;
            points[1] = PointSetting(self.transform.position);
            points[2] = PointSetting(self.Player.transform.position);
        }

        // 속도 보정을 위한 t 증가량 계산
        _t += (self.EnemyData.MoveSpeed * Time.deltaTime) / _approxCurveLength;

        points[3] = self.Player.transform.position;
        DrawTrajectory(self);
    }

    private void SetPath(EnemyController self)
    {
        _isPathSet = true;
        points[0] = self.transform.position; // P0
        points[1] = PointSetting(self.transform.position); // P1
        points[2] = PointSetting(self.Player.transform.position); // P2
        points[3] = self.Player.transform.position; // P3
        _prevPosition = self.transform.position;

        _approxCurveLength = EstimateCurveLength(points, 20); // 20개 샘플로 길이 추정
    }

    Vector3 PointSetting(Vector3 origin)
    {
        float x = posA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        float y = posB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;
        float z = posA * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.z;
        return new Vector3(x, y, z);
    }

    void DrawTrajectory(EnemyController self)
    {
        Vector3 currentPosition = GetBezierPoint(_t);

        Vector3 direction = (currentPosition - _prevPosition).normalized;
        if (direction != Vector3.zero)
        {
            self.transform.rotation = Quaternion.LookRotation(direction);
        }

        self.transform.position = currentPosition;
        _prevPosition = currentPosition;
    }

    Vector3 GetBezierPoint(float t)
    {
        return new Vector3(
            FourPointBezier(points[0].x, points[1].x, points[2].x, points[3].x, t),
            FourPointBezier(points[0].y, points[1].y, points[2].y, points[3].y, t),
            FourPointBezier(points[0].z, points[1].z, points[2].z, points[3].z, t)
        );
    }

    private float FourPointBezier(float a, float b, float c, float d, float t)
    {
        return Mathf.Pow(1 - t, 3) * a
             + 3 * Mathf.Pow(1 - t, 2) * t * b
             + 3 * (1 - t) * Mathf.Pow(t, 2) * c
             + Mathf.Pow(t, 3) * d;
    }

    private float EstimateCurveLength(Vector3[] pts, int segments)
    {
        float length = 0f;
        Vector3 prev = GetBezierPoint(0f);
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 curr = GetBezierPoint(t);
            length += Vector3.Distance(prev, curr);
            prev = curr;
        }
        return length;
    }
}
