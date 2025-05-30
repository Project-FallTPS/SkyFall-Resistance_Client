using System;
using System.Collections;
using UnityEngine;

public class RangeHeatController : MonoBehaviour
{
    /// <summary>
    /// bool로 과열 경고 UI나 머티리얼 빨갛게 띄울거면 ㄱㄱ
    /// </summary>
    public static Action<float, float, bool> OnHeatChanged;

    public float Heat { get; private set; }
    public float MaxHeat = 100f;
    public float HeatPerShot = 5f;
    public float CooldownRate = 25f;
    public float OverHeatTime = 5f;

    public bool IsOverheated = false;

    private void Update()
    {
        if (!Mathf.Approximately(Heat, 0f) && !Input.GetMouseButton(0) && !IsOverheated)
        {
            Heat -= CooldownRate * Time.deltaTime;
            Heat = Mathf.Max(Heat, 0f);
            OnHeatChanged?.Invoke(Heat, MaxHeat, IsOverheated);
        }

        Debug.Log($"과열 : {Heat}");
    }

    /// <summary>
    /// 발사를 시도하고 성공 여부를 반환
    /// </summary>
    public bool TryConsumeHeat()
    {
        if (IsOverheated)
            return false;

        Heat += HeatPerShot;
        Heat = Mathf.Min(Heat, MaxHeat);
        OnHeatChanged?.Invoke(Heat, MaxHeat, IsOverheated);
        if(Mathf.Approximately(Heat, MaxHeat))
        {
            StartCoroutine(CoOverHeat());
        }

        return true;
    }

    private IEnumerator CoOverHeat()
    {
        IsOverheated = true;

        yield return new WaitForSeconds(OverHeatTime);

        float duration = 0.5f;
        float elapsed = 0f;
        float startHeat = Heat;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Heat = Mathf.Lerp(startHeat, 0f, elapsed / duration);
            OnHeatChanged?.Invoke(Heat, MaxHeat, IsOverheated);
            yield return null;
        }

        Heat = 0f;
        IsOverheated = false;
        OnHeatChanged?.Invoke(Heat, MaxHeat, IsOverheated);
    }
}
