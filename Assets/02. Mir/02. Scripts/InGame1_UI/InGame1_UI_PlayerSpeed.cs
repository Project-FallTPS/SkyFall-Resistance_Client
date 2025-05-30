using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame1_UI_PlayerSpeed : MonoBehaviour
{
    [SerializeField] 
    private PlayerMovement _playerMovement;

    [SerializeField] 
    private Slider _speedSlider;

    [SerializeField] 
    private TextMeshProUGUI _speedText;

    [Header("Speed Settings")]
    [SerializeField] 
    private float minSpeed = 110f;

    [SerializeField] 
    private float midSpeed = 130f;

    [SerializeField] 
    private float maxSpeed = 150f;

    private void Update()
    {
        float y = GetClampedY();
        float speed = CalculateSpeed(y);
        UpdateSlider(speed);
        UpdateSpeedText(speed);
    }

    private float GetClampedY()
    {
        return Mathf.Clamp(_playerMovement.MoveDirection.y, -1f, 1f);
    }

    private float CalculateSpeed(float y)
    {
        // 선형 보간: -1일 때 min, 0일 때 mid, 1일 때 max
        if (0f <= y)
        {
            return Mathf.Lerp(midSpeed, minSpeed, y);
        }
        else
        {
            return Mathf.Lerp(midSpeed, maxSpeed, -y);
        }
    }

    private void UpdateSlider(float speed)
    {
        // 슬라이더 값 갱신 (0~1 정규화)
        float normalized = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        _speedSlider.value = normalized;
    }

    private void UpdateSpeedText(float speed)
    {
        if (!ReferenceEquals(_speedText, null))
        {
            _speedText.text = $"{Mathf.RoundToInt(speed)} km/h";
        }
    }
}
