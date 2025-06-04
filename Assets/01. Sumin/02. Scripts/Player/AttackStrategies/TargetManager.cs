using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TargetManager : Singleton<TargetManager>
{
    [Header("# Detection Activate")]
    private bool _isKatana = false;

    [Header("UI & Camera Reference")]
    [SerializeField] private Image _crossHair;
    [SerializeField] private GameObject _targetLockedUIPrefab;
    [SerializeField] private Image _originalCrossHairUIPrefab;
    [SerializeField] private RectTransform _uiLockArea;
    [SerializeField] private List<CrossHair> _crossHairSprites;

    [Header("Target Detection")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance;

    private GameObject _targetLockedUIInstance;
    private Vector3 _targetScreenPos;

    private HashSet<GameObject> _enemiesInFrustum = new HashSet<GameObject>();
    public GameObject Target;

    protected override void Awake()
    {
        base.Awake();

        if (_camera == null) _camera = Camera.main;

        _targetLockedUIInstance = Instantiate(_targetLockedUIPrefab, _uiLockArea.parent);
        _targetLockedUIInstance.SetActive(false);

        UIEventHandler.Instance.OnPlayerWeaponChange += SetTargetDetectionActivate;
    }

    private void Update()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;
        
        foreach (var enemy in _enemiesInFrustum)
        {
            if (!enemy.activeSelf) continue;
            Vector3 screenPos = _camera.WorldToScreenPoint(enemy.transform.position);
            if (screenPos.z <= 0f) continue;

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
        UpdateLockedUI();
    }

    private void UpdateLockedUI()
    {
        if (_targetLockedUIInstance == null || !_isKatana) return;

        if (Target != null)
        {
            _targetLockedUIInstance.transform.position = _targetScreenPos;
            _targetLockedUIInstance.SetActive(true);
            _originalCrossHairUIPrefab.gameObject.SetActive(false);
        }
        else
        {
            _targetLockedUIInstance.SetActive(false);
            _originalCrossHairUIPrefab.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Enemy)) || other.CompareTag(nameof(ETags.Boss)))
        {
            float dist = Vector3.Distance(_camera.transform.position, other.transform.position);
            if (dist <= _maxDistance)
            {
                _enemiesInFrustum.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Enemy)) || other.CompareTag(nameof(ETags.Boss)))
        {
            _enemiesInFrustum.Remove(other.gameObject);
        }
    }

    public void RemoveEnemyFromHashSet(GameObject target)
    {
        _enemiesInFrustum.Remove(target);
    }

    private void SetTargetDetectionActivate(EWeaponType type)
    {
        if(type == EWeaponType.Range)
        {
            Target = null;
            UpdateLockedUI();
        }

        _isKatana = type == EWeaponType.Katana ? true : false;

        foreach (var ch in _crossHairSprites)
        {
            if(ch.Type == type)
            {
                _originalCrossHairUIPrefab.sprite = ch.Sprite;
            }
        }
    }
}
