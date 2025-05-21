using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TargetManager : Singleton<TargetManager>
{
    [Header("UI & Camera Reference")]
    [SerializeField] private Image _crossHair;
    [SerializeField] private GameObject _targetLockedUIPrefab;
    [SerializeField] private GameObject _originalCrossHairUIPrefab;
    [SerializeField] private RectTransform _uiLockArea;

    [Header("Target Detection")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance;

    private GameObject _targetLockedUIInstance;
    private Vector3 _targetScreenPos;

    private HashSet<GameObject> _enemiesInFrustum = new HashSet<GameObject>();
    public GameObject Target { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (_camera == null) _camera = Camera.main;

        _targetLockedUIInstance = Instantiate(_targetLockedUIPrefab, _uiLockArea.parent);
        _targetLockedUIInstance.SetActive(false);
    }

    private void Update()
    {
        SetTarget();
        UpdateLockedUI();
    }

    private void SetTarget()
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (var enemy in _enemiesInFrustum)
        {
            // 화면 안에 있는지 체크
            Vector3 screenPos = _camera.WorldToScreenPoint(enemy.transform.position);
            if (screenPos.z <= 0f) continue;

            // UI 안에 포함되는지 체크
            if (!RectTransformUtility.RectangleContainsScreenPoint(_uiLockArea, screenPos)) continue;

            float dist = Vector3.Distance(_camera.transform.position, enemy.transform.position);
            
            if (dist > _maxDistance) continue;

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy;
                _targetScreenPos = screenPos;
            }
        }

        Target = closest;
        _crossHair.color = Target != null ? Color.red : Color.white;
    }

    private void UpdateLockedUI()
    {
        if (_targetLockedUIInstance == null) return;

        if (Target != null)
        {
            _targetLockedUIInstance.transform.position = _targetScreenPos;
            _targetLockedUIInstance.SetActive(true);
            _originalCrossHairUIPrefab.SetActive(false);
        }
        else
        {
            _targetLockedUIInstance.SetActive(false);
            _originalCrossHairUIPrefab.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            float dist = Vector3.Distance(_camera.transform.position, other.transform.position);
            if (dist <= _maxDistance)
            {
                _enemiesInFrustum.Add(other.gameObject);
                Debug.Log("타겟 감지됨: " + other.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemiesInFrustum.Remove(other.gameObject);
        }
    }
}
