using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    private Tween _sliderTween;
    private Tween _textTween;
    private float _displayedSpeed;

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
        if (y >= 0f)
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
        float normalized = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        _sliderTween?.Kill();
        _sliderTween = DOTween.To(
            () => _speedSlider.value,
            x => _speedSlider.value = x,
            normalized,
            0.25f
        ).SetEase(Ease.InOutSine);
    }

    private void UpdateSpeedText(float targetSpeed)
    {
        if (_speedText == null) return;

        _textTween?.Kill();
        _textTween = DOTween.To(
            () => _displayedSpeed,
            x => {
                _displayedSpeed = x;
                _speedText.text = $"{Mathf.RoundToInt(_displayedSpeed)} km/h";
            },
            targetSpeed,
            0.25f
        ).SetEase(Ease.InOutSine);
    }
}

