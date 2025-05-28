using UnityEngine;

public class HermiteBullet : BulletBase
{
    // Test 용
    public Transform StartPoint;
    public Transform MidPoint;
    public Transform EndPoint;

    private Vector3 _startPosition;
    private Vector3 _midPosition;
    private Vector3 _endPosition;

    private Vector3 _startTangent;
    private Vector3 _midTangent;
    private Vector3 _endTangent;

    private float _t = 0f;
    private int _phase = 0;     // 0 : start -> control, 1 : control -> end
    private float _startToMidLength;
    private float _midToEndLength;
    

    private void Start()
    {
        InitializePoints(StartPoint.position, MidPoint.position, EndPoint.position);
    }

    protected override void Update()
    {
        base.Update();

        if (_phase == 0)
        {
            _t += (_speed * Time.deltaTime) / _startToMidLength;
        }
        else if (_phase == 1)
        {
            _t += (_speed * Time.deltaTime) / _midToEndLength;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Player)))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
            DamageablePoolManager.Instance.ReturnObject(gameObject, _damageableType);
        }
    }

    protected override void Move()
    {
        if (_phase == 0)
        {
            Vector3 position = Hermite(_t, _startPosition, _midPosition, _startTangent, _midTangent);
            transform.position = position;
            if (_t >= 1f)
            {
                _phase = 1;
                _t = 0f;
            }
        }
        else if (_phase == 1)
        {
            Vector3 position = Hermite(_t, _midPosition, _endPosition, _midTangent, _endTangent);
            transform.position = position;
            if (_t >= 1f)
            {
                _phase = 2;
                _t = 0f;
            }
        }
    }

    public void InitializePoints(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        _startPosition = p0;
        _midPosition = p1;
        _endPosition = p2;
        _t = 0f;

        Vector3 startToEnd = (_endPosition - _startPosition).normalized;
        _midTangent = startToEnd * 2 *_speed;
        _startTangent = (_midPosition - _startPosition) - startToEnd * _speed;
        _endTangent = (_endPosition - _midPosition) - startToEnd * _speed;

        _startToMidLength = EstimateCurveLength(20, _startPosition, _midPosition, _startTangent, _midTangent);
        _midToEndLength = EstimateCurveLength(20, _midPosition, _endPosition, _midTangent, _endTangent);
    }

    private Vector3 Hermite(float t, Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float h00 = 2 * t3 - 3 * t2 + 1;
        float h10 = t3 - 2 * t2 + t;
        float h01 = -2 * t3 + 3 * t2;
        float h11 = t3 - t2;

        return h00 * p0 + h10 * m0 + h01 * p1 + h11 * m1;
    }

    private float EstimateCurveLength(int segments, Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1)
    {
        float length = 0f;
        Vector3 prev = p0;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 curr = Hermite(t, p0, p1, m0, m1);
            length += Vector3.Distance(prev, curr);
            prev = curr;
        }
        return length;
    }
}
