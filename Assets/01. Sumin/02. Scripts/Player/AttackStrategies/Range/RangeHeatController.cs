using System;
using System.Collections;
using UnityEngine;

public class RangeHeatController : MonoBehaviour
{
    public static Action<float, float, bool> OnHeatChanged;

    public float Heat { get; private set; }
    public float MaxHeat = 100f;
    public float HeatPerShot = 5f;
    public float CooldownRate = 25f;
    public float OverHeatTime = 5f;

    [Header("# OverHeat Manage")]
    private bool _isOverheated = false;
    private float _disabledTime;
    private bool _wasOverheated = false;

    private Coroutine _coolingCoroutine;

    private void OnDisable()
    {
        _disabledTime = Time.realtimeSinceStartup;

        if (_isOverheated)
        {
            _wasOverheated = true;
        }

        if (_coolingCoroutine != null)
        {
            StopCoroutine(_coolingCoroutine);
            _coolingCoroutine = null;
        }
    }

    private void OnEnable()
    {
        float timePassed = Time.realtimeSinceStartup - _disabledTime;

        // 비오버히트 상태였으면 그냥 쿨다운 처리
        if (!_wasOverheated)
        {
            if (!_isOverheated && Heat > 0f)
            {
                Heat -= CooldownRate * timePassed;
                Heat = Mathf.Max(Heat, 0f);
                OnHeatChanged?.Invoke(Heat, MaxHeat, _isOverheated);
            }
        }
        else
        {
            // 오버히트 상태였다면, 시간 흐름 고려해서 코루틴 다시 진행
            if (timePassed >= OverHeatTime + 0.5f)
            {
                Heat = 0f;
                _isOverheated = false;
                _wasOverheated = false;
                OnHeatChanged?.Invoke(Heat, MaxHeat, _isOverheated);
            }
            else
            {
                _coolingCoroutine = StartCoroutine(CoOverHeat(timePassed));
            }
        }
    }


    private void Update()
    {
        if (!Mathf.Approximately(Heat, 0f) && !Input.GetMouseButton(0) && !_isOverheated)
        {
            Heat -= CooldownRate * Time.deltaTime;
            Heat = Mathf.Max(Heat, 0f);
            OnHeatChanged?.Invoke(Heat, MaxHeat, _isOverheated);
        }
        Debug.Log(Heat);
    }

    public bool TryConsumeHeat()
    {
        if (_isOverheated)
        {
            return false;
        }

        Heat += HeatPerShot;
        Heat = Mathf.Min(Heat, MaxHeat);
        OnHeatChanged?.Invoke(Heat, MaxHeat, _isOverheated);
        if (Mathf.Approximately(Heat, MaxHeat))
        {
            _coolingCoroutine = StartCoroutine(CoOverHeat(0f));
        }

        return true;
    }

    private IEnumerator CoOverHeat(float timePassed)
    {
        _isOverheated = true;
        _wasOverheated = true;

        // 남은 대기 시간
        float waitTime = Mathf.Max(0f, OverHeatTime - timePassed);
        if (waitTime > 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }

        float duration = 0.5f;
        float elapsed = Mathf.Clamp(timePassed - OverHeatTime, 0f, duration);
        float startHeat = Heat;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Heat = Mathf.Lerp(startHeat, 0f, elapsed / duration);
            OnHeatChanged?.Invoke(Heat, MaxHeat, _isOverheated);
            yield return null;
        }

        Heat = 0f;
        _isOverheated = false;
        _wasOverheated = false;
        _coolingCoroutine = null;
        OnHeatChanged?.Invoke(Heat, MaxHeat, _isOverheated);
    }
}
