using UnityEngine;

public class TraceBezier : ITraceStrategy
{
    private Vector3[] _points = new Vector3[3];
    private Vector3 _prevControlPoint = new Vector3(0f, 0f, 0f);
    private float _t = 0f;

    private Vector3 _prevPosition;
    private float _approxCurveLength = 1f;

    public void Trace(EnemyController self)
    {
        if (1f < _t)
        {
            SetPath(self);
            _t = 0f;
        }
        _t += (self.EnemyData.MoveSpeed * Time.deltaTime) / _approxCurveLength;
        DrawTrajectory(self);
    }

    private void SetPath(EnemyController self)
    {
        Vector3 startPoint = self.transform.position;
        Vector3 endPoint = self.Player.transform.position;
        Vector3 controlPoint = SetControlPoint(startPoint, endPoint);
        _prevControlPoint = controlPoint;

        _points[0] = startPoint;
        _points[1] = controlPoint;
        _points[2] = endPoint;

        _approxCurveLength = EstimateCurveLength(20);
        _prevPosition = startPoint;
    }
    
    private Vector3 SetControlPoint(Vector3 startPoint, Vector3 endPoint)
    {
        if (_prevControlPoint != Vector3.zero)
        {
            return _prevControlPoint + (startPoint - _prevControlPoint) * 2f;
        }
        else
        {
            Vector3 controlOffset = 
                new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f),Random.Range(-3f, 3f));
            return (startPoint + endPoint) * 0.5f + controlOffset;
        }
    }

    private void DrawTrajectory(EnemyController self)
    {
        Vector3 currentPosition = GetBezierPoint(_t);
        Vector3 direction = (currentPosition - _prevPosition).normalized;

        if (direction != Vector3.zero)
            self.transform.rotation = Quaternion.LookRotation(direction);

        self.transform.position = currentPosition;
        _prevPosition = currentPosition;
    }

    private Vector3 GetBezierPoint(float t)
    {
        return Mathf.Pow(1 - t, 2) * _points[0]
             + 2 * (1 - t) * t * _points[1]
             + Mathf.Pow(t, 2) * _points[2];
    }

    private float EstimateCurveLength(int segments)
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
