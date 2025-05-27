using UnityEngine;

public class CurveBullet : BulletBase
{
    private Vector3 _p0, _p1; // 시작점, 끝점
    private Vector3 _m0, _m1; // 탄젠트 벡터 (Hermite)
    private float _t;

    public void InitializeHermite(Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1)
    {
        _p0 = p0;
        _m0 = m0;
        _p1 = p1;
        _m1 = m1;
        _t = 0f;
    }

    protected override void Update()
    {
        base.Update();
        _t += (_speed * Time.deltaTime) / EstimateCurveLength(20);
    }

    protected override void Move()
    {
        transform.position = CalculateHermitePoint(_t, _p0, _m0, _p1, _m1);
    }

    private Vector3 CalculateHermitePoint(float t, Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return (2 * t3 - 3 * t2 + 1) * p0 +
               (t3 - 2 * t2 + t) * m0 +
               (-2 * t3 + 3 * t2) * p1 +
               (t3 - t2) * m1;
    }

    private float EstimateCurveLength(int segments)
    {
        float length = 0f;
        Vector3 prev = _p0;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 curr = CalculateHermitePoint(t, _p0, _m0, _p1, _m1);
            length += Vector3.Distance(prev, curr);
            prev = curr;
        }

        return length;
    }
}