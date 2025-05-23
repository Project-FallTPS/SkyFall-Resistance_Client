using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class TargetManager : Singleton<TargetManager>
{
    [Header("UI & Camera Reference")]
    [SerializeField] private Image _crossHair;
    [SerializeField] private GameObject _targetLockedUIPrefab;
    [SerializeField] private GameObject _originalCrossHairUIPrefab;
    [SerializeField] private RectTransform _uiLockArea; // 범위
    [Header("Target Detection")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance;
    private GameObject _targetLockedUIInstance;
    private Vector3 _targetScreenPos;
    private HashSet<GameObject> _enemiesInFrustum = new HashSet<GameObject>();
    public GameObject Target;
    private Vector2 _uiMinBound;
    private Vector2 _uiMaxBound;
    protected override void Awake()
    {
        base.Awake();
        if (_camera == null) _camera = Camera.main;
        _targetLockedUIInstance = Instantiate(_targetLockedUIPrefab, _uiLockArea.parent);
        _targetLockedUIInstance.SetActive(false);
        MakeAimAreaBound();
    }
    private void Update()
    {
        CheckEnemies();
        SetTarget();
    }
    private void MakeAimAreaBound()
    {
        Vector3[] corners = new Vector3[4];
        _uiLockArea.GetWorldCorners(corners); // 좌하 -> 좌상 -> 우상 -> 우하 (clockwise)
        for (int i = 0; i < 4; i++)
        {
            corners[i] = _camera.WorldToScreenPoint(corners[i]); // World → Screen
        }
        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        _uiMinBound = new Vector2(minX, minY);
        _uiMaxBound = new Vector2(maxX, maxY);
    }
    private void CheckEnemies()
    {
        _enemiesInFrustum.Clear();
        Debug.Log(((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies.Count);
        foreach (var enemy in ((EnemyPoolManager)EnemyPoolManager.Instance).ActiveEnemies)
        {
            if (enemy == null) continue;
            // 1. enemy의 위치를 스크린 좌표로 변환
            Vector3 screenPos = _camera.WorldToScreenPoint(enemy.transform.position);
            // 2. 카메라 앞에 있는지 (z > 0) 확인
            if (screenPos.z <= 0) continue;
            // 3. _uiLockArea 바운더리 안에 들어왔는지 확인
            if (IsInUILockArea(screenPos))
            {
                _enemiesInFrustum.Add(enemy);
            }
        }
    }
    private bool IsInUILockArea(Vector2 screenPos)
    {
        return screenPos.x >= _uiMinBound.x && screenPos.x <= _uiMaxBound.x &&
               screenPos.y >= _uiMinBound.y && screenPos.y <= _uiMaxBound.y;
        
    }
    private void SetTarget()
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;
        foreach (var enemy in _enemiesInFrustum)
        {
            if (enemy == null) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        Target = closest;
        _crossHair.color = Target != null ? Color.red : Color.white;
        // 타겟 UI 위치 업데이트
        if (Target != null)
        {
            _targetScreenPos = _camera.WorldToScreenPoint(Target.transform.position);
            _targetLockedUIInstance.transform.position = _targetScreenPos;
        }
    }
}