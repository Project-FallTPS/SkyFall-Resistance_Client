using System;
using UnityEngine;

public class TrailMovement : MonoBehaviour
{
    [SerializeField] 
    private float _speed;
    
    private TrailRenderer _trailRenderer;
    private Vector3 _startPosition;
    private Transform _targetTransform;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        _trailRenderer.Clear();
    }

    private void Update()
    {
        MoveToTarget();
    }

    public void SetStartPositionAndTarget(Vector3 startPosition, Transform targetTransform)
    {
        _startPosition = startPosition;
        _targetTransform = targetTransform;
    }

    public void MoveToTarget()
    {
        Vector3 direction = (_targetTransform.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, _targetTransform.position);
        if (distance < 0.1f)
        {
            VFXPoolManager.Instance.ReturnObject(gameObject, EVFXType.EnemySupportTypeShieldTrail);
        }
    }


}