using UnityEngine;

/// <summary>
/// 원형(또는 아무 UI 오브젝트)을 Z축 기준으로 회전시킵니다.
/// • rotationSpeed : 초당 회전 각도(deg/s)
/// • clockwise     : true  → 시계방향(–Z) / false → 반시계방향(+Z)
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UICircleRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private bool clockwise = true;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float dir = clockwise ? -1f : 1f;             // 시계방향은 –Z
        _rect.Rotate(0f, 0f, rotationSpeed * dir * Time.unscaledDeltaTime);
        //   ⤷ Time.unscaledDeltaTime 을 쓰면 게임 일시정지(Pause) 중에도 회전
        //     일반적인 경우엔 Time.deltaTime 으로 바꿔도 무방합니다.
    }

    /// <summary>
    /// 외부에서 속도를 조정하고 싶을 때 호출
    /// </summary>
    public void SetSpeed(float newSpeed) => rotationSpeed = newSpeed;
}