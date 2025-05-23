using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame1_UI_PlayerSpeed : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private TextMeshProUGUI _speedText; // Optional

    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 110f;
    [SerializeField] private float midSpeed = 130f;
    [SerializeField] private float maxSpeed = 150f;

    private void Update()
    {
        if (_playerMovement == null || _speedSlider == null)
            return;

        float y = Mathf.Clamp(_playerMovement.MoveDirection.y, -1f, 1f);

        // ���� ����: -1�� �� min, 0�� �� mid, 1�� �� max
        float speed;
        if (y >= 0)
            speed = Mathf.Lerp(midSpeed, minSpeed, y); // y: -1~0 �� t: 0~1
        else
            speed = Mathf.Lerp(midSpeed, maxSpeed, -y);     // y: 0~1 �� t: 0~1

        // �����̴� �� ���� (0~1 ����ȭ)
        float normalized = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        _speedSlider.value = normalized;

        // �ؽ�Ʈ ǥ�� (����)
        if (_speedText != null)
            _speedText.text = $"{Mathf.RoundToInt(speed)} km/h";
    }
}
