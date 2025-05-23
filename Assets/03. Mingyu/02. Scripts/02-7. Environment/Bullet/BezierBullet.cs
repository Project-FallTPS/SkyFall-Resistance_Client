using System.Collections;
using UnityEngine;

public class BezierBullet : BulletBase
{
    private Vector3 _p0, _p1, _p2;
    private float _t;
    
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

    public void InitializePoints(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        _p0 = p0;
        _p1 = p1;
        _p2 = p2;
        _t = 0f;
    }
    protected override void Update()
    {
        base.Update();
        _t += (_speed * Time.deltaTime) / EstimateCurveLength(20);
    }

    protected override void Move()
    {
        transform.position = CalculateBezierPoint(_t, _p0, _p1, _p2);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0
               + 2 * (1 - t) * t * p1
               + Mathf.Pow(t, 2) * p2;
    }

    private float EstimateCurveLength(int segments)
    {
        float length = 0f;
        Vector3 prev = _p0;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 curr = CalculateBezierPoint(t, _p0, _p1, _p2);
            length += Vector3.Distance(prev, curr);
            prev = curr;
        }
        return length;
    }
}
